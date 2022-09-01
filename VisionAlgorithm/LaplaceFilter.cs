using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.VisionAlgorithm
{
    class LaplaceFilter
    {
        public static Bitmap ApplyLaplaceFilter(Bitmap bmpData)
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
