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
    class OtsuBinarization
    {
        public static Bitmap OtsuThresholding(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;

            Bitmap res_img = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData image_data = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData res_data = res_img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];

            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            img.UnlockBits(image_data);

            double[] histogram = new double[256];
            for (int i = 0; i < bytes; i++)
            {
                histogram[buffer[i]]++;
            }

            double globalMean = 0;
            for (int i = 0; i < 256; i++)
            {
                globalMean += i * histogram[i];
            }
            int total = width * height;
            double sumB = 0;
            double weightBackground = 0;
            double weightForeground = 0;

            double varMax = 0;
            double threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                weightBackground += histogram[t];
                if (weightBackground == 0) continue;

                weightForeground = total - weightBackground;
                if (weightForeground == 0) break;

                sumB += (t * histogram[t]);

                double meanBackground = sumB / weightBackground;
                double meanForeground = (globalMean - sumB) / weightForeground;

                //클래스간 분산 계산
                double varBetween = weightBackground * weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);

                // 클래스간 분산이 최대값을 넘으면 갱신, threshold 갱신
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

            //Marshal.Copy(result, 0, res_data.Scan0, bytes); 불가. 대용량 데이터는 한번에 부를 수 없음

            var ptr = res_data.Scan0;
            for (var i = 0; i < height; i++)
            {
                Marshal.Copy(result, i * width, ptr, width);
                ptr += width;
            }

            res_img.UnlockBits(res_data);

            return res_img;
        }

    }
}
