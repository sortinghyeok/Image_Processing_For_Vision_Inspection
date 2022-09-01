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
    class HistogramEqualization
    {
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
            int byteOffset = 0;
            double[] pixelDistribution = new double[256];

            Parallel.For(0, bytes, (int p) => {
                pixelDistribution[buffer[p]]++;
            });

            Parallel.For(0, pixelDistribution.Length, (int idx) =>
            {
                pixelDistribution[idx] /= (width * height);
            }); 
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byteOffset = y * sd.Stride + x;
                    double sum = 0;
                    for (int i = 0; i < buffer[byteOffset]; i++)
                    {
                        sum += pixelDistribution[i];
                    }

                    result[byteOffset] = (byte)Math.Floor(255 * sum);

                }
            }

            BitmapData rd = res.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
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
    }
}
