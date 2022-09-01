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
    class LaplaceFilter
    {
        private static Bitmap ApplyConvolutionFilter(
              Bitmap sourceBitmap,
              double[,] filterArray,
              double factor = 1,
              int bias = 0,
              bool grayscale = false
        )
        {
            BitmapData sourceBitmapData = sourceBitmap.LockBits
            (
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );

            byte[] sourceByteArray = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            byte[] targetByteArray = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];

            Marshal.Copy(sourceBitmapData.Scan0, sourceByteArray, 0, sourceByteArray.Length);

            sourceBitmap.UnlockBits(sourceBitmapData);

            if (grayscale == true)
            {
                float rgb = 0;

                for (int k = 0; k < sourceByteArray.Length; k += 4)
                {
                    rgb = sourceByteArray[k] * 0.11f;
                    rgb += sourceByteArray[k + 1] * 0.59f;
                    rgb += sourceByteArray[k + 2] * 0.3f;

                    sourceByteArray[k] = (byte)rgb;
                    sourceByteArray[k + 1] = sourceByteArray[k];
                    sourceByteArray[k + 2] = sourceByteArray[k];
                    sourceByteArray[k + 3] = 255;
                }
            }

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filterArray.GetLength(1);
            int filterHeight = filterArray.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int sourceOffset = 0;
            int targetOffset = 0;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    targetOffset = offsetY * sourceBitmapData.Stride + offsetX * 4;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            sourceOffset = targetOffset + (filterX * 4) + (filterY * sourceBitmapData.Stride);

                            blue += (double)(sourceByteArray[sourceOffset]) *
                                     filterArray[filterY + filterOffset, filterX + filterOffset];

                            green += (double)(sourceByteArray[sourceOffset + 1]) *
                                     filterArray[filterY + filterOffset, filterX + filterOffset];

                            red += (double)(sourceByteArray[sourceOffset + 2]) *
                                     filterArray[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;

                    if (blue > 255)
                    {
                        blue = 255;
                    }
                    else if (blue < 0)
                    {
                        blue = 0;
                    }

                    if (green > 255)
                    {
                        green = 255;
                    }
                    else if (green < 0)
                    {
                        green = 0;
                    }

                    if (red > 255)
                    {
                        red = 255;
                    }
                    else if (red < 0)
                    {
                        red = 0;
                    }

                    targetByteArray[targetOffset] = (byte)blue;
                    targetByteArray[targetOffset + 1] = (byte)green;
                    targetByteArray[targetOffset + 2] = (byte)red;
                    targetByteArray[targetOffset + 3] = 255;
                }
            }

            Bitmap targetBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData targetBitmapData = targetBitmap.LockBits
            (
                new Rectangle(0, 0, targetBitmap.Width, targetBitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );

            Marshal.Copy(targetByteArray, 0, targetBitmapData.Scan0, targetByteArray.Length);

            targetBitmap.UnlockBits(targetBitmapData);

            return targetBitmap;
        }
        public static double[,] kernelMatrix
        {
            get
            {
                return new double[,]
                {
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, 24, -1, -1 },
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1 }
                };
            }
        }
        public static Bitmap ApplyRGBLaplacian(Bitmap bmpData)
        { 
            Bitmap targetBitmap = ApplyConvolutionFilter
            (
                bmpData,
                kernelMatrix,
                1.0,
                0,
                true
            );

            return targetBitmap;
        }


        public static Bitmap ApplyBinaryLaplaceFilter(Bitmap bmpData)
        {
            int[,] kernelMatrix = new int[3, 3]  { { 0, 1, 0 },
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

                Parallel.For(0, height, (int row) =>
                {
                    byte* tempPtr = (byte*)DestData.Scan0 + row * DestData.Stride;
                    for (int col = 0; col < width; col++)
                    {

                        cptr[col] = 0;
                    }
                });

                Parallel.For(1, height - 1, (int row) =>
                {
                    for (int col = 1; col < width - 1; col++)
                    {
                        gray = *(ptr + row * DestData.Stride + col - 1) + *(ptr + (row - 1) * DestData.Stride + col) -
                            4 * (*(ptr + DestData.Stride * row + col)) + *(ptr + (row + 1) * DestData.Stride + col) + *(ptr + row * DestData.Stride + col + 1); //bytes[i, j - 1] + bytes[i - 1, j] - 4 * bytes[i, j] + bytes[i + 1, j] + bytes[i, j + 1];
                        gray += 128;
                        if (gray > 158)
                        {
                            gray = 0;
                        }
                        else
                        {
                            gray = 255;
                        }
                        *(cptr + row * DestData.Stride + col) = (byte)gray;//result[i*width + j] = (byte)gray;
                    }
                });
            }

            bmpData.UnlockBits(LockedImage);
            copiedImage.UnlockBits(DestData);

            return copiedImage;
        }
    }
}
       

