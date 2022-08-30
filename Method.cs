using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Assignment
{
    class Method
    {
        private uint header_size;
        private uint pixel_width;
        private uint pixel_height;
        private ushort pixel_depth;
        private uint row_size;
        private uint padding_bits;
        private uint padding_bytes;
        private uint width_bytes;
        private uint pixels;

        public List<uint> ParsingBMPHeader(BinaryReader br) //binary file로부터 비트 스트림을 가진 객체 br을 parameter로 받음
        {
            //bitmap의 width, height, bit lock의 이미지 모드 옵션, 그리고 
            br.BaseStream.Seek(14, SeekOrigin.Begin); // 14번 바이트 주소를 찾아 스트림 내 위치를 설정. 헤더로 바로 접근
            header_size = br.ReadUInt32(); //헤더 사이즈가 적힌 4바이트에 대하여 값 저장
            pixel_width = br.ReadUInt32(); //픽셀의 넓이가 적힌 4바이트에 대하여 값 저장
            pixel_height = br.ReadUInt32();// 픽셀의 넓이가 적힌 4바이트에 대하여 값 저장
            br.ReadUInt16();//bit planes에 대하여 스킵, 항상 1이므로 굳이 저장하지 않음
            pixel_depth = br.ReadUInt16();//비트 수준에 대하여 2바이트 저장
            row_size = (((pixel_depth * pixel_width) + 31) / 32) * 4;
            padding_bits = row_size * 8 - ((pixel_width * pixel_depth));
            padding_bytes = row_size - ((pixel_width * pixel_depth) / 8);
            width_bytes = (pixel_width * pixel_depth) / 8;
            pixels = pixel_height * pixel_width;
            PrintToConsole();

            List<uint> list = new List<uint> { header_size, pixel_width, pixel_height, pixel_depth, pixels, row_size, padding_bits, padding_bytes };
            return list;
        }
        public void PrintToConsole()
        {
            Console.Write("Header size : ");
            Console.WriteLine(header_size);
            Console.Write("Pixel Width : ");
            Console.WriteLine(pixel_width);
            Console.Write("Pixel Height: ");
            Console.WriteLine(pixel_height);
            Console.Write("Pixel Depth : ");
            Console.WriteLine(pixel_depth);
            Console.Write("Pixels : ");
            Console.WriteLine(pixels);
            Console.Write("Row size : ");
            Console.WriteLine(row_size);
            Console.Write("Padding bits : ");
            Console.WriteLine(padding_bits);
            Console.Write("Padding bytes : ");
            Console.WriteLine(padding_bytes);
        }
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        public static Image ImageFromRawArray(byte[] arr, int width, int height, PixelFormat pixelFormat, int padding)
        {
            var output = new Bitmap(width, height, pixelFormat);
            Console.WriteLine("Applied Width, Height : " + width + " " + height + " " + pixelFormat);
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, output.PixelFormat);

            // Row-by-row copy
            var arrRowLength = width;//Image.GetPixelFormatSize(output.PixelFormat) / 8;
            Console.WriteLine("Arr Row Length : " + arrRowLength);
            var ptr = bmpData.Scan0;
            for (var i = 0; i < height; i++)
            {
                Marshal.Copy(arr, i * arrRowLength, ptr, arrRowLength);
                ptr += arrRowLength;
            }

            var newPalette = output.Palette;
            for (int index = 0; index < output.Palette.Entries.Length; ++index)
            {
                //모노 이미지로 변환해주지 않으면 칼라이미지가 깨진채로 사용된다.

                newPalette.Entries[index] = Color.FromArgb(index, index, index);

                //위의 코드를 수행하지 않고 아래의 주석 코드를 적용하여 그레이스케일을 억지로 적용하면, 깨진 색에 노이즈가 낀 이미지에 그레이스케일이 적용된다.
                /*
                var entry = output.Palette.Entries[index];
                var gray = ((entry.R + entry.G + entry.B)) / 3;
                newPalette.Entries[index] = Color.FromArgb(gray, gray, gray);
                */
            }

            output.Palette = newPalette;
            output.UnlockBits(bmpData);
            return output;
        }

        public static Bitmap Dilate(Bitmap bmpData)
        {
            // Create Destination bitmap.
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);

            // Take source bitmap data.
            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0,
                bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format8bppIndexed);

            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width,
                copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            byte[,] maskMatrix = new byte[5, 5] {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
                };

            int size = 5;
            //byte max, clrValue;
            int radius = size / 2;
            //int ir, jr;

            unsafe
            {
                Parallel.For(radius, DestData.Height - radius, (int ROWS) => {
      
                    byte* ptr = (byte*)LockedImage.Scan0 + (ROWS * LockedImage.Stride);
                    byte* dstPtr = (byte*)DestData.Scan0 + (ROWS * LockedImage.Stride);

 
                    for (int COLS = radius; COLS < DestData.Width - radius; COLS++)
                    {
                        byte max = 0;
                        byte clrValue = 0;
            
                        for (int MATROWS = -radius; MATROWS <= radius; MATROWS++)
                        {
        
                            byte* tempPtr = (byte*)LockedImage.Scan0 + ((ROWS + MATROWS) * LockedImage.Stride);


                            for (int MATCOLS = -radius; MATCOLS <= radius; MATCOLS++)
                            {
                                clrValue = tempPtr[COLS + MATCOLS];

                                if (max < clrValue)
                                {
                                    if (maskMatrix[MATROWS+radius, MATCOLS+radius] != 0)
                                        max = clrValue;
                                }
                            }
                        }

                        *dstPtr = max;

                        ptr += 1;
                        dstPtr += 1;
                    }
                });//for (int cols = radius; cols < DestData.Height - radius; cols++)
               
            }
          
            var newPalette = copiedImage.Palette;
            for (int index = 0; index < copiedImage.Palette.Entries.Length; ++index)
            {
                newPalette.Entries[index] = Color.FromArgb(index, index, index);
            }

            copiedImage.Palette = newPalette;
            // Dispose all Bitmap data.
            bmpData.UnlockBits(LockedImage);
            copiedImage.UnlockBits(DestData);

            // return dilated bitmap.
            return copiedImage;
        }
        public static Bitmap Erode(Bitmap bmpData)
        {

            // Create Destination bitmap.
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);

            // Take source bitmap data.
            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0,
                bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format8bppIndexed);

            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width,
                copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
           
            int size = 5;
            int radius = size / 2;
            DateTime pt;
            unsafe
            {
                pt = DateTime.Now;
             
                //컬럼 루프
                Parallel.For(radius, DestData.Height - radius, (int ROWS) =>
                {
                    byte* ptr = (byte*)LockedImage.Scan0 + (ROWS * LockedImage.Stride);
                    byte* dstPtr = (byte*)DestData.Scan0 + (ROWS * LockedImage.Stride);

                    //로우 루프
                    for (int COLS = radius; COLS < DestData.Width - radius; COLS++)
                    {
                        byte min = 255;
                        byte clrValue = 0;

                        for (int MATROWS = -radius; MATROWS <= radius; MATROWS++)
                        {
                            byte* tempPtr = (byte*)LockedImage.Scan0 + ((ROWS + MATROWS) * LockedImage.Stride);

                            for (int MATCOLS = -radius; MATCOLS <= radius; MATCOLS++)
                            {
                                //주변 화소 값을 통한 min 갱신
                                clrValue = tempPtr[COLS + MATCOLS];
                                min = Math.Min(min, clrValue);

                            }
                        }

                        *dstPtr = min;

                        ptr += 1;
                        dstPtr += 1;
                    }
                });//for (int cols = radius; cols < DestData.Height - radius; cols++)
            }
            
            var newPalette = copiedImage.Palette;
            for (int index = 0; index < copiedImage.Palette.Entries.Length; ++index)
            {
                newPalette.Entries[index] = Color.FromArgb(index, index, index);
            }

            copiedImage.Palette = newPalette;
            bmpData.UnlockBits(LockedImage);
            copiedImage.UnlockBits(DestData);

            Console.WriteLine("Time Cost : " + (DateTime.Now.Ticks / 10000 - pt.Ticks/10000));
            // return dilated bitmap.
            return copiedImage;
        }

        public static Bitmap hist_Equalizer(Bitmap bmpData)
        {
            int width = bmpData.Width;
            int height = bmpData.Height;
            Bitmap res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData sd = bmpData.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int bytes = sd.Stride * sd.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(sd.Scan0, buffer, 0, bytes);
            bmpData.UnlockBits(sd);
            int current = 0;
            double[] pn = new double[256];
            for (int p = 0; p < bytes; p++)
            {
                pn[buffer[p]]++;
            }
            for (int prob = 0; prob < pn.Length; prob++)
            {
                pn[prob] /= (width * height);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * sd.Stride + x;
                    double sum = 0;
                    for (int i = 0; i < buffer[current]; i++)
                    {
                        sum += pn[i];
                    }
                    
                    result[current] = (byte)Math.Floor(255 * sum);
                    
                    //result[current + 3] = 255;
                }
            }
       
            BitmapData rd = res.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(result, 0, rd.Scan0, bytes);
            var ptr = rd.Scan0;
            for (var i = 0; i < bmpData.Height; i++)
            {
                Marshal.Copy(result, i * bmpData.Width, ptr, bmpData.Width);
                ptr += bmpData.Width;
            }
            var newPalette = res.Palette;
            for (int index = 0; index < res.Palette.Entries.Length; ++index)
            {
                newPalette.Entries[index] = Color.FromArgb(index, index, index);
            }

            res.Palette = newPalette;

            res.UnlockBits(rd);
            return res;
        }

        public static Bitmap OtsuThresholding(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;

            Bitmap res_img = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData image_data = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData res_data = res_img.LockBits(
              new Rectangle(0, 0, width, height),
              ImageLockMode.WriteOnly,
              PixelFormat.Format8bppIndexed);


            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];

            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            img.UnlockBits(image_data);

            double[] histogram = new double[256];
            for(int i = 0; i<bytes; i++)
            {
                histogram[buffer[i]]++;
            }

            double global_mean = 0;
            for (int i = 0; i < 256; i++)
            {
                global_mean += i * histogram[i];
            }
            int total = width*height;
            double sumB = 0;
            double wB = 0;
            double wF = 0;

            double varMax = 0;
            double threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                wB += histogram[t];               // Weight Background
                if (wB == 0) continue;

                wF = total - wB;                 // Weight Foreground
                if (wF == 0) break;

                sumB += (t * histogram[t]);

                double mB = sumB / wB;            // Mean Background
                double mF = (global_mean - sumB) / wF;    // Mean Foreground

                // Calculate Between Class Variance
                double varBetween = wB * wF * (mB - mF) * (mB - mF);

                // Check if new maximum found
                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = t;
                }
            }

            for (int i = 0; i < bytes; i++)
            {
                result[i] = (byte)((buffer[i] > threshold) ? 255 : 0);
            }

          
            //Marshal.Copy(result, 0, res_data.Scan0, bytes);

            var ptr = res_data.Scan0;
            for (var i = 0; i < height; i++)
            {
                Marshal.Copy(result, i * width, ptr, width);
                ptr += width;
            }

            res_img.UnlockBits(res_data);

            return res_img;
        }

        //kernel 구현 시도중 실패...
        public static double[,] GaussianKernel(int length, double weight)
        {
            double[,] kernel = new double[length, length];
            double kernelSum = 0;
            int offset = (length - 1) / 2;
            double distance = 0;
            double constant = 1d / (2 * Math.PI * weight * weight);
            for (int y = -offset; y <= offset; y++)
            {
                for (int x = -offset; x <= offset; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * weight * weight);
                    kernel[y + offset, x + offset] = constant * Math.Exp(-distance);
                    kernelSum += kernel[y + offset, x + offset];
                }
            }
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    kernel[y, x] = kernel[y, x] * 1d / kernelSum;
                }
            }
            return kernel;
        }

        public static Bitmap GaussianConvolve(Bitmap srcImage, double[,] kernel)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;

            Bitmap resultImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData srcData = srcImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int bytes = srcData.Width * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];

            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            srcImage.UnlockBits(srcData);

            //get amount of array in dimension 0
            int offset = (kernel.GetLength(0) - 1) /2;


            double kcenter_data = 0;
            int kcenter = 0;
    
            for (int rows = offset; rows < height - offset; rows++)
            {
                int pvalue = 0;
                for (int cols = offset; cols < width - offset; cols++)
                {
                    kcenter = rows * width + cols;
                    for (int matRows = -offset; matRows <= offset; matRows++)
                    {
                        for (int matCols = -offset; matCols <= offset; matCols++)
                        {
                            pvalue = kcenter + matRows * width + matCols;      
                            //double kernelValue = kernel[matRows + offset, matCols + offset];
                            kcenter_data += (double)(buffer[pvalue]) * kernel[matRows + offset, matCols + offset];                         
                        }
                    }
                    result[kcenter] = (byte)(kcenter_data);
                }
            }

            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            var ptr = resultData.Scan0;
            for (var i = 0; i < height; i++)
            {
                Marshal.Copy(result, i * width, ptr, width);
                ptr += width;
            }

            var newPalette = resultImage.Palette;
            for (int index = 0; index < resultImage.Palette.Entries.Length; ++index)
            {
                newPalette.Entries[index] = Color.FromArgb(index, index, index);
            }

            resultImage.Palette = newPalette;

            resultImage.UnlockBits(resultData);
            return resultImage;

        }

        public static Bitmap GaussianFilter(Bitmap bmpData) {
            double[,] maskMatrix = new double[3, 3]  { { 1, 2, 1 }, 
                                                        { 2, 4, 2 },
                                                        {1, 2, 1} };

            int width = bmpData.Width;
            int height = bmpData.Height;
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);
            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0, bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width, copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int size = 3;
            int radius = size / 2;

            unsafe
            {
                Parallel.For(0, height, (int row) => {
                    byte* tempPtr = (byte*)DestData.Scan0 + row * DestData.Stride;
                    for (int j = 0; j < width; j++)
                    {
                        tempPtr[j] = 0;
                    }
                });
            }
       
            unsafe
            {
                Parallel.For(radius, DestData.Height - radius, (int ROWS) =>
                {
                    double newValue;
                    for (int COLS = radius; COLS < DestData.Stride - radius; COLS++)
                    {
                        newValue = 0;
                        byte* center = (byte*)LockedImage.Scan0 + (ROWS * LockedImage.Stride) + COLS;
                        byte* dptr = (byte*)DestData.Scan0 + (ROWS * DestData.Stride) + COLS;

                        for (int MATROWS = -radius; MATROWS <= radius; MATROWS++)
                        {
                            for (int MATCOLS = -radius; MATCOLS <= radius; MATCOLS++)
                            {
                                newValue += ((*(center + MATROWS*LockedImage.Stride + MATCOLS)) * maskMatrix[MATROWS + radius, MATCOLS + radius]);

                            }
                        }
                       
                        *dptr = (byte)(newValue/16);
                    }
                });          
                
            }
            
            
            var newPalette = copiedImage.Palette;
            for (int index = 0; index < copiedImage.Palette.Entries.Length; ++index)
            {
                newPalette.Entries[index] = Color.FromArgb(index, index, index);
            }

            copiedImage.Palette = newPalette;
            bmpData.UnlockBits(LockedImage);
            copiedImage.UnlockBits(DestData);

            return copiedImage;     
        }

        public static Bitmap LaplaceFilter(Bitmap bmpData)
        {
            int[,] maskMatrix = new int[3, 3]  { { 0, 1, 0 },
                                                        { 1, -4, 1 },
                                                        {0, 1, 0} };

            int width = bmpData.Width;
            int height = bmpData.Height;
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);
            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0, bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width, copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int size = 3;
            int radius = size / 2;

            int gray = 0;


            unsafe
            {
                var ptr = (byte*)LockedImage.Scan0;
                var cptr = (byte*)DestData.Scan0;
                
                for (int row = 0; row < height; row++)
                {
                    byte* tempPtr = (byte*)DestData.Scan0 + row * DestData.Stride;
                    for (int col = 0; col < width; col++)
                    {

                        cptr[col] = 0;
                    }
                }

                for (int row = 1; row<height-1; row++)
                {
                    for(int col = 1; col<width-1; col++)
                    {
                        gray = *(ptr + row * DestData.Stride + col - 1) + *(ptr + (row-1)* DestData.Stride + col) - 
                            4 * (*(ptr + DestData.Stride * row + col)) + *(ptr + (row+1)* DestData.Stride + col) + *(ptr + row* DestData.Stride + col+1); //bytes[i, j - 1] + bytes[i - 1, j] - 4 * bytes[i, j] + bytes[i + 1, j] + bytes[i, j + 1];
                        gray += 128;
                        if(gray > 158)
                        {
                            gray = 0;
                        }
                        else
                        {
                            gray = 255;
                        }
                        *(cptr + row * DestData.Stride + col) = (byte)gray;//result[i*width + j] = (byte)gray;
                    }
                }
            }        

            bmpData.UnlockBits(LockedImage);
            copiedImage.UnlockBits(DestData);

            return copiedImage;
        }
        
    }

}
