using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Assignment
{
 

    public partial class Prj1_jhLee : Form
    {
        int oriWidth;
        public Prj1_jhLee()
        {
       
            InitializeComponent();
      
        }

        private void ZoomIn()
        {
            double ZOOM_FACTOR = 1.15;
            int max_value = 2;

            if (photo_Executed.Width < (max_value *oriWidth))
            {
                photo_Executed.Width = Convert.ToInt32(photo_Executed.Width * ZOOM_FACTOR);
                photo_Executed.Height = Convert.ToInt32(photo_Executed.Height * ZOOM_FACTOR);
                photo_Executed.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void ZoomOut()
        {
            double ZOOM_FACTOR = 1.15;

            if (200 < (photo_Executed.Width))
            {
                photo_Executed.SizeMode = PictureBoxSizeMode.Zoom;
                photo_Executed.Width = Convert.ToInt32(photo_Executed.Width / ZOOM_FACTOR);
                photo_Executed.Height = Convert.ToInt32(photo_Executed.Height / ZOOM_FACTOR);
            }
        }

        private void Prj1_jhLee_Load(object sender, EventArgs e)
        {

        }

        private unsafe void btn_OpenImage_Click(object sender, EventArgs e)
        {
            try
            {  
                OpenFileDialog ofd = new OpenFileDialog();
                string filter = "Bitmap (*.bmp, *dib) | *.bmp; *.div";
                ofd.Filter = filter;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var filePath = ofd.FileName; //파일 경로
                
                    if (!filePath.EndsWith(".bmp"))
                    {
                        MessageBox.Show("알림", "파일 형식이 맞지 않습니다.");
                        return;
                    }
                    Method met = new Method();
                  
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {             
                        byte[] b = new byte[fs.Length];

                        //img = Image.FromStream(fs);
                        fs.Read(b, 0, b.Length);

                        BinaryReader br = new BinaryReader(fs);
                        List<uint> list = met.ParsingBMPHeader(br);

                        met.ParsingBMPHeader(br);
                        Bitmap bm = (Bitmap)Method.ImageFromRawArray(b, (int)list[1], (int)list[2], PixelFormat.Format8bppIndexed, (int)list[6]);
                        bm.RotateFlip(RotateFlipType.Rotate180FlipX);
                    
                        if (bm.PixelFormat != PixelFormat.Format8bppIndexed) throw new InvalidOperationException();
                       
                        photo_bmp.Image = bm;
                        photo_Executed.Image = bm;

                        while(photo_bmp.Width >= 500)
                        {
                            photo_bmp.SizeMode = PictureBoxSizeMode.Zoom;
                            photo_bmp.Width = Convert.ToInt32(photo_bmp.Width / 1.1);
                            photo_bmp.Height = Convert.ToInt32(photo_bmp.Height/ 1.1);
                        }
                        while (photo_Executed.Width >= 500)
                        {
                            photo_Executed.SizeMode = PictureBoxSizeMode.Zoom;
                            photo_Executed.Width = Convert.ToInt32(photo_Executed.Width / 1.1);
                            photo_Executed.Height = Convert.ToInt32(photo_Executed.Height / 1.1);
                        }
                        fs.Close();
                        fs.Dispose();
                        //photo_bmp.Image = Image.FromStream(memoryStream);
                        oriWidth = bm.Width;

                    }
                    //fs.CopyTo(memoryStream);
                    //
                    // met.ParsingBMPHeader(br);

                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString(),"Error 발생");
                Console.WriteLine(err.ToString());
            }
            
          
            
        }

        private void btn_Expand_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void btn_Contract_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }
    }
    public class MousePoint
    {
        int x;
        int y;
        public MousePoint(int mx, int my)
        {
            x = mx;
            y = my;
        }
    }
}
