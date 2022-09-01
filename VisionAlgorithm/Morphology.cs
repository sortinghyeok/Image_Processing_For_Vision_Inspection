using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.VisionAlgorithm
{
    class Morphology //Contains Dilate, Erosion Operations
    {
        public static Bitmap Dilate(Bitmap bmpData)
        {
            // Create Destination bitmap.
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);

            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0,
                bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format8bppIndexed);

            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width,
                copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
           
            byte[,] kernelArray = new byte[5, 5] {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
                };

            int size = 5;
            //byte max, clrValue;
            int radius = (size - 1) / 2;
            //int ir, jr;

            unsafe
            {
                Parallel.For(radius, DestData.Height - radius, (int ROWS) => {
                    for (int COLS = radius; COLS < DestData.Stride - radius; COLS++)
                    {
                        byte max = 0;

                        for (int MATROWS = -radius; MATROWS <= radius; MATROWS++)
                        {

                            byte* tempPtr = (byte*)LockedImage.Scan0 + ((ROWS + MATROWS) * LockedImage.Stride);


                            for (int MATCOLS = -radius; MATCOLS <= radius; MATCOLS++)
                            {
                                if (kernelArray[MATROWS + radius, MATCOLS + radius] == 1)
                                {
                                    max = Math.Max(max, tempPtr[COLS + MATCOLS]);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        var dptr = (byte*)DestData.Scan0;
                        *(dptr + ROWS * DestData.Stride + COLS) = max;
                    }
                });
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
            byte[,] kernelArray = new byte[5, 5] {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
                };
            // Create Destination bitmap.
            Bitmap copiedImage = new Bitmap(bmpData.Width, bmpData.Height, PixelFormat.Format8bppIndexed);

            // Take source bitmap data.
            BitmapData LockedImage = bmpData.LockBits(new Rectangle(0, 0,
                bmpData.Width, bmpData.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format8bppIndexed);

            BitmapData DestData = copiedImage.LockBits(new Rectangle(0, 0, copiedImage.Width,
                copiedImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int size = 5;
            int radius = (size - 1) / 2;
            DateTime pt;
            unsafe
            {
                pt = DateTime.Now;

                //컬럼 루프
                Parallel.For(radius, DestData.Height - radius, (int ROWS) =>
                {
                    //로우 루프
                    for (int COLS = radius; COLS < DestData.Width - radius; COLS++)
                    {
                        byte min = 255;

                        for (int MATROWS = -radius; MATROWS <= radius; MATROWS++)
                        {
                            byte* tempPtr = (byte*)LockedImage.Scan0 + ((ROWS + MATROWS) * LockedImage.Stride);

                            for (int MATCOLS = -radius; MATCOLS <= radius; MATCOLS++)
                            {
                                if (kernelArray[radius + MATROWS, MATCOLS + radius] == 1)
                                {
                                    min = Math.Min(min, tempPtr[COLS + MATCOLS]);
                                }
                                else
                                {
                                    continue;
                                }
                                //주변 화소 값을 통한 min 갱신


                            }
                        }
                        var dptr = (byte*)DestData.Scan0;
                        *(dptr + ROWS * DestData.Stride + COLS) = min;
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

            Console.WriteLine("Time Cost : " + (DateTime.Now.Ticks / 10000 - pt.Ticks / 10000));
            // return dilated bitmap.
            return copiedImage;
        }
    }
}
