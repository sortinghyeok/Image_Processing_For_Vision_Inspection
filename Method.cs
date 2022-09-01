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
        class FFTElement
        {
            public double re = 0.0;
            public double im = 0.0;
            public FFTElement next;
            public uint revTgt;
        }
        private uint m_logN = 0;
        private uint m_N = 0;
        private FFTElement[] m_X;

        private uint header_size;
        private uint pixel_width;
        private uint pixel_height;
        private ushort pixel_depth;
        private uint row_size;
        private uint padding_bits;
        private uint padding_bytes;
        private uint width_bytes;
        private uint pixels;

        private uint BitReverse(uint x, uint numBits)
        {
            uint y = 0;
            for (uint i = 0; i < numBits; i++)
            {
                y <<= 1;
                y |= x & 0x0001;
                x >>= 1;
            }
            return y;
        }
        public void FFTFilter(uint logN)
        {
            m_logN = logN;
            m_N = (uint)(1 << (int)m_logN);

            m_X = new FFTElement[m_N];
            for(uint k = 0; k<m_N; k++)
            {
                m_X[k] = new FFTElement();
            }
            for(uint k = 0; k<m_N-1; k++)
            {
                m_X[k].next = m_X[k + 1];
            }
            for(uint k = 0; k< m_N; k++)
            {
                m_X[k].revTgt = BitReverse(k, logN);
            }
        }

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

                  
    }

}
