using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.VisionAlgorithm
{
    class GaussianFilter
    {
        public static Bitmap ApplyGaussianFilter(Bitmap bmpData)
        {
            double[,] kernelArray = new double[3, 3]  { { 1, 2, 1 },
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
                                newValue += ((*(center + MATROWS * LockedImage.Stride + MATCOLS)) * kernelArray[MATROWS + radius, MATCOLS + radius]);

                            }
                        }

                        *dptr = (byte)(newValue / 16);
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

            //커널의 너비로 오프셋을 정하는 방법의 하나로, 커널 값의 변수를 바꿀 경우 다른 코드를 변경할 필요 없이 사용 가능하다. 같은 기능을 하지만 직접 상수리터럴을 주어 구현한 것은 하단에 있다.
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
    }
}
