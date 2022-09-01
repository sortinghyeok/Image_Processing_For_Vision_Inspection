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
        private int oriWidth;

        private Point bmpBeginPoint = Point.Empty;
        private Point bmpMovePoint = Point.Empty;
        bool mouseDownFlag = false;
        bool mouseMoveFlag = false;

        private Point executedBeginPoint = Point.Empty;
        private Point executedMovePoint = Point.Empty;
        string currentFilepath = "";

        private void photo_Executed_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Expand();
            }
            else if (e.Delta < 0)
            {
                Contract();
            }
        }
        public Prj1_jhLee()
        {            
            InitializeComponent();
            photo_Executed.MouseWheel += new MouseEventHandler(photo_Executed_MouseWheel);
            photo_Executed.SizeMode = PictureBoxSizeMode.StretchImage;

        }
        
        public void bringBtnsToFront()
        {
            btn_Contract.BringToFront();
            btn_Expand.BringToFront();
            btn_FFT.BringToFront();
            btn_Gauss.BringToFront();
            btn_HistoEqualizer.BringToFront();
            btn_BinaryLaplace.BringToFront();
            btn_OtsuThresh.BringToFront();
            btn_Template.BringToFront();
            btn_OpenImage.BringToFront();
            btn_SaveImage.BringToFront();
        }

        private void Expand()
        {
            double ZOOM_FACTOR = 1.15;
            int max_value = 5;

            if (photo_Executed.Width < (max_value * oriWidth))
            {
                photo_Executed.Width = Convert.ToInt32(photo_Executed.Width * ZOOM_FACTOR);
                photo_Executed.Height = Convert.ToInt32(photo_Executed.Height * ZOOM_FACTOR);

                photo_Executed.SizeMode = PictureBoxSizeMode.Zoom;
                bringBtnsToFront();
            }
        }

        private void Contract()
        {
            double ZOOM_FACTOR = 1.15;

            if (photo_Executed.Width > (200))
            {
                photo_Executed.Width = Convert.ToInt32(photo_Executed.Width / ZOOM_FACTOR);
                photo_Executed.Height = Convert.ToInt32(photo_Executed.Height / ZOOM_FACTOR);

                photo_Executed.SizeMode = PictureBoxSizeMode.Zoom;
                bringBtnsToFront();
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
                    currentFilepath = ofd.FileName;
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
                        Bitmap bm;
                        try
                        {
                            bm = new Bitmap(fs);
                        }
                        catch (Exception openingError)
                        {
                            Console.WriteLine("Error : " + openingError.ToString());
                     
                            bm = (Bitmap)Method.ImageFromRawArray(b, (int)list[1], (int)list[2], PixelFormat.Format8bppIndexed, (int)list[6]);
                        }

                        //bm.RotateFlip(RotateFlipType.RotateNoneFlipX);

                        if (bm.PixelFormat != PixelFormat.Format8bppIndexed) throw new InvalidOperationException();

                        photo_bmp.Image = bm;
                        photo_Executed.Image = bm;

                        while (photo_bmp.Width >= 800)
                        {
                            photo_bmp.SizeMode = PictureBoxSizeMode.Zoom;
                            photo_bmp.Width = Convert.ToInt32(photo_bmp.Width / 1.1);
                            photo_bmp.Height = Convert.ToInt32(photo_bmp.Height / 1.1);
                        }
                        while (photo_Executed.Width >= 800)
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
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error 발생");
                Console.WriteLine(err.ToString());
            }


        }

        private void btn_Expand_Click(object sender, EventArgs e)
        {
            Expand();
        }

        private void btn_SaveImage_Click(object sender, EventArgs e)
        {
            Bitmap bm = (Bitmap)photo_Executed.Image.Clone();
            bm.Save("C:\\Users\\jhlee98\\Desktop\\" + "savedImage.bmp", ImageFormat.Bmp);
        }

 

        private void photo_bmp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownFlag = true;
                bmpBeginPoint = new Point(e.Location.X - bmpMovePoint.X, e.Location.Y - bmpMovePoint.Y);
            }
        }

        private void photo_bmp_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownFlag = false;
        }

        private void photo_bmp_MouseMove(object sender, MouseEventArgs e)
        {
            mouseMoveFlag = true;
            if (mouseDownFlag)
            {
                bmpMovePoint = new Point(e.Location.X - bmpBeginPoint.X, e.Location.Y - bmpBeginPoint.Y);
                photo_bmp.Invalidate();

            }
        }

        private void photo_bmp_Paint(object sender, PaintEventArgs e)
        {
            if (mouseDownFlag)
            {
                e.Graphics.Clear(Color.White);

                if (this.photo_bmp.Image != null)
                    e.Graphics.DrawImage(this.photo_bmp.Image, bmpMovePoint);
            }

        }

        private void photo_Executed_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownFlag = true;
                executedBeginPoint = new Point(e.Location.X - executedMovePoint.X, e.Location.Y - executedMovePoint.Y);
            }
        }

        private void photo_Executed_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownFlag)
            {
                executedMovePoint = new Point(e.Location.X - executedBeginPoint.X, e.Location.Y - executedBeginPoint.Y);
                photo_Executed.Invalidate();
            }
        }

        private void photo_Executed_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownFlag = false;
        }

        private void photo_Executed_Paint(object sender, PaintEventArgs e)
        {
            if (mouseDownFlag & mouseMoveFlag)
            {
                e.Graphics.Clear(Color.White);

                if (this.photo_Executed.Image != null)
                {
                    Rectangle r = new Rectangle();
                    r.Width = photo_Executed.Width - 5;
                    r.Height = photo_Executed.Height - 5;
                    r.Location = executedMovePoint;
                    e.Graphics.DrawImage(this.photo_Executed.Image, r);

                }

            }
        }

        private void btn_OtsuThresh_Click(object sender, EventArgs e)
        {
         
            photo_Executed.Image = VisionAlgorithm.OtsuBinarization.OtsuThresholding((Bitmap)photo_Executed.Image);
        }

        

        private void btn_HistoEqualizer_Click(object sender, EventArgs e)
        {
            photo_Executed.Image = VisionAlgorithm.HistogramEqualization.hist_Equalizer((Bitmap)photo_Executed.Image);
        }

        private void btn_Gauss_Click(object sender, EventArgs e)
        {
            //double[,] kernelMap = Method.GaussianKernel(3, 0.8); // division params : 2* pi * 2 * 2
            //photo_Executed.Image = Method.GaussianConvolve((Bitmap)photo_Executed.Image, kernelMap);
            photo_Executed.Image = VisionAlgorithm.GaussianFilter.ApplyGaussianFilter((Bitmap)photo_Executed.Image);
        }

        private void btn_Template_Click(object sender, EventArgs e)
        {

        }

        private void btn_FFT_Click(object sender, EventArgs e)
        {

        }

        private void btn_BinLaplace_Click(object sender, EventArgs e)
        {
            photo_Executed.Image = VisionAlgorithm.LaplaceFilter.ApplyBinaryLaplaceFilter((Bitmap)photo_Executed.Image);
        }

        private void btn_GenLaplace_Click(object sender, EventArgs e)
        {
            photo_Executed.Image = VisionAlgorithm.LaplaceFilter.ApplyRGBLaplacian((Bitmap)photo_Executed.Image);
        }
        private void btn_Dilate_Click_1(object sender, EventArgs e)
        {
            photo_Executed.Image = VisionAlgorithm.Morphology.Dilate((Bitmap)photo_Executed.Image);
        }

        private void btn_Contract_Click(object sender, EventArgs e)
        {
            photo_Executed.Image = VisionAlgorithm.Morphology.Erode((Bitmap)photo_Executed.Image);
           
        }

        private void btn_partSave_Click(object sender, EventArgs e)
        {
            string Filepath = "C:\\Users\\jhlee98\\Desktop\\" + "savedImage.bmp";
            if (Filepath!= null)
            {

                Bitmap bitmap = new Bitmap(825, 825, PixelFormat.Format16bppGrayScale);

                Graphics g = Graphics.FromImage(bitmap);
                   
                g.CopyFromScreen(885, 20, 0, 0, bitmap.Size);
                    

                bitmap.Save(Filepath, ImageFormat.Bmp);
                
                
            }
    
        }

        
    }


}

