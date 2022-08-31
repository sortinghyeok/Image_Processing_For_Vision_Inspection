
namespace Assignment
{
    partial class Prj1_jhLee
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.photo_bmp = new System.Windows.Forms.PictureBox();
            this.btn_Laplace = new System.Windows.Forms.Button();
            this.btn_FFT = new System.Windows.Forms.Button();
            this.btn_Template = new System.Windows.Forms.Button();
            this.btn_OpenImage = new System.Windows.Forms.Button();
            this.btn_SaveImage = new System.Windows.Forms.Button();
            this.btn_Expand = new System.Windows.Forms.Button();
            this.btn_Contract = new System.Windows.Forms.Button();
            this.btn_HistoEqualizer = new System.Windows.Forms.Button();
            this.btn_OtsuThresh = new System.Windows.Forms.Button();
            this.btn_Gauss = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.photo_Executed = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_partSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.photo_bmp)).BeginInit();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photo_Executed)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // photo_bmp
            // 
            this.photo_bmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photo_bmp.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.photo_bmp.Location = new System.Drawing.Point(3, 20);
            this.photo_bmp.Name = "photo_bmp";
            this.photo_bmp.Size = new System.Drawing.Size(825, 825);
            this.photo_bmp.TabIndex = 0;
            this.photo_bmp.TabStop = false;
            this.photo_bmp.Paint += new System.Windows.Forms.PaintEventHandler(this.photo_bmp_Paint);
            this.photo_bmp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.photo_bmp_MouseDown);
            this.photo_bmp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.photo_bmp_MouseMove);
            this.photo_bmp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.photo_bmp_MouseUp);
            // 
            // btn_Laplace
            // 
            this.btn_Laplace.Location = new System.Drawing.Point(1746, 414);
            this.btn_Laplace.Name = "btn_Laplace";
            this.btn_Laplace.Size = new System.Drawing.Size(126, 35);
            this.btn_Laplace.TabIndex = 3;
            this.btn_Laplace.Text = "필터 라플라스";
            this.btn_Laplace.UseVisualStyleBackColor = true;
            this.btn_Laplace.Click += new System.EventHandler(this.btn_Laplace_Click);
            // 
            // btn_FFT
            // 
            this.btn_FFT.Location = new System.Drawing.Point(1746, 455);
            this.btn_FFT.Name = "btn_FFT";
            this.btn_FFT.Size = new System.Drawing.Size(126, 35);
            this.btn_FFT.TabIndex = 4;
            this.btn_FFT.Text = "!필터 FFT";
            this.btn_FFT.UseVisualStyleBackColor = true;
            this.btn_FFT.Click += new System.EventHandler(this.btn_FFT_Click);
            // 
            // btn_Template
            // 
            this.btn_Template.Location = new System.Drawing.Point(1746, 496);
            this.btn_Template.Name = "btn_Template";
            this.btn_Template.Size = new System.Drawing.Size(126, 35);
            this.btn_Template.TabIndex = 5;
            this.btn_Template.Text = "!템플릿 매칭";
            this.btn_Template.UseVisualStyleBackColor = true;
            this.btn_Template.Click += new System.EventHandler(this.btn_Template_Click);
            // 
            // btn_OpenImage
            // 
            this.btn_OpenImage.Location = new System.Drawing.Point(1746, 12);
            this.btn_OpenImage.Name = "btn_OpenImage";
            this.btn_OpenImage.Size = new System.Drawing.Size(126, 35);
            this.btn_OpenImage.TabIndex = 7;
            this.btn_OpenImage.Text = "이미지1 열기";
            this.btn_OpenImage.UseVisualStyleBackColor = true;
            this.btn_OpenImage.Click += new System.EventHandler(this.btn_OpenImage_Click);
            // 
            // btn_SaveImage
            // 
            this.btn_SaveImage.Location = new System.Drawing.Point(1746, 53);
            this.btn_SaveImage.Name = "btn_SaveImage";
            this.btn_SaveImage.Size = new System.Drawing.Size(126, 35);
            this.btn_SaveImage.TabIndex = 8;
            this.btn_SaveImage.Text = "이미지2 저장";
            this.btn_SaveImage.UseVisualStyleBackColor = true;
            this.btn_SaveImage.Click += new System.EventHandler(this.btn_SaveImage_Click);
            // 
            // btn_Expand
            // 
            this.btn_Expand.Location = new System.Drawing.Point(1746, 177);
            this.btn_Expand.Name = "btn_Expand";
            this.btn_Expand.Size = new System.Drawing.Size(126, 35);
            this.btn_Expand.TabIndex = 9;
            this.btn_Expand.Text = "팽창";
            this.btn_Expand.UseVisualStyleBackColor = true;
            this.btn_Expand.Click += new System.EventHandler(this.btn_Expand_Click_1);
            // 
            // btn_Contract
            // 
            this.btn_Contract.Location = new System.Drawing.Point(1746, 218);
            this.btn_Contract.Name = "btn_Contract";
            this.btn_Contract.Size = new System.Drawing.Size(126, 35);
            this.btn_Contract.TabIndex = 10;
            this.btn_Contract.Text = "수축";
            this.btn_Contract.UseVisualStyleBackColor = true;
            this.btn_Contract.Click += new System.EventHandler(this.btn_Contract_Click);
            // 
            // btn_HistoEqualizer
            // 
            this.btn_HistoEqualizer.Location = new System.Drawing.Point(1746, 274);
            this.btn_HistoEqualizer.Name = "btn_HistoEqualizer";
            this.btn_HistoEqualizer.Size = new System.Drawing.Size(126, 35);
            this.btn_HistoEqualizer.TabIndex = 11;
            this.btn_HistoEqualizer.Text = "히스토그램 평활화";
            this.btn_HistoEqualizer.UseVisualStyleBackColor = true;
            this.btn_HistoEqualizer.Click += new System.EventHandler(this.btn_HistoEqualizer_Click);
            // 
            // btn_OtsuThresh
            // 
            this.btn_OtsuThresh.Location = new System.Drawing.Point(1746, 315);
            this.btn_OtsuThresh.Name = "btn_OtsuThresh";
            this.btn_OtsuThresh.Size = new System.Drawing.Size(126, 35);
            this.btn_OtsuThresh.TabIndex = 12;
            this.btn_OtsuThresh.Text = "오츠 이진화";
            this.btn_OtsuThresh.Click += new System.EventHandler(this.btn_OtsuThresh_Click);
            // 
            // btn_Gauss
            // 
            this.btn_Gauss.Location = new System.Drawing.Point(1746, 373);
            this.btn_Gauss.Name = "btn_Gauss";
            this.btn_Gauss.Size = new System.Drawing.Size(126, 35);
            this.btn_Gauss.TabIndex = 13;
            this.btn_Gauss.Text = "필터 가우스";
            this.btn_Gauss.UseVisualStyleBackColor = true;
            this.btn_Gauss.Click += new System.EventHandler(this.btn_Gauss_Click);
            // 
            // Panel1
            // 
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.photo_bmp);
            this.Panel1.Location = new System.Drawing.Point(12, 12);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(850, 850);
            this.Panel1.TabIndex = 14;
            // 
            // photo_Executed
            // 
            this.photo_Executed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photo_Executed.Location = new System.Drawing.Point(3, 20);
            this.photo_Executed.Name = "photo_Executed";
            this.photo_Executed.Size = new System.Drawing.Size(825, 825);
            this.photo_Executed.TabIndex = 6;
            this.photo_Executed.TabStop = false;
            this.photo_Executed.Paint += new System.Windows.Forms.PaintEventHandler(this.photo_Executed_Paint);
            this.photo_Executed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.photo_Executed_MouseDown);
            this.photo_Executed.MouseMove += new System.Windows.Forms.MouseEventHandler(this.photo_Executed_MouseMove);
            this.photo_Executed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.photo_Executed_MouseUp);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.photo_Executed);
            this.panel2.Location = new System.Drawing.Point(881, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(850, 850);
            this.panel2.TabIndex = 15;
            // 
            // btn_partSave
            // 
            this.btn_partSave.Location = new System.Drawing.Point(1746, 113);
            this.btn_partSave.Name = "btn_partSave";
            this.btn_partSave.Size = new System.Drawing.Size(126, 35);
            this.btn_partSave.TabIndex = 16;
            this.btn_partSave.Text = "!보이는 곳 저장";
            this.btn_partSave.UseVisualStyleBackColor = true;
            this.btn_partSave.Click += new System.EventHandler(this.btn_partSave_Click);
            // 
            // Prj1_jhLee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1884, 911);
            this.Controls.Add(this.btn_partSave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.btn_Gauss);
            this.Controls.Add(this.btn_OtsuThresh);
            this.Controls.Add(this.btn_HistoEqualizer);
            this.Controls.Add(this.btn_Contract);
            this.Controls.Add(this.btn_Expand);
            this.Controls.Add(this.btn_SaveImage);
            this.Controls.Add(this.btn_OpenImage);
            this.Controls.Add(this.btn_Template);
            this.Controls.Add(this.btn_FFT);
            this.Controls.Add(this.btn_Laplace);
            this.Name = "Prj1_jhLee";
            this.Text = "Prj1_JhLee";
            this.Load += new System.EventHandler(this.Prj1_jhLee_Load);
            ((System.ComponentModel.ISupportInitialize)(this.photo_bmp)).EndInit();
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.photo_Executed)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox photo_bmp;
        private System.Windows.Forms.Button btn_Laplace;
        private System.Windows.Forms.Button btn_FFT;
        private System.Windows.Forms.Button btn_Template;
        private System.Windows.Forms.Button btn_OpenImage;
        private System.Windows.Forms.Button btn_SaveImage;
        private System.Windows.Forms.Button btn_Expand;
        private System.Windows.Forms.Button btn_Contract;
        private System.Windows.Forms.Button btn_HistoEqualizer;
        private System.Windows.Forms.Button btn_OtsuThresh;
        private System.Windows.Forms.Button btn_Gauss;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.PictureBox photo_Executed;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_partSave;
    }
}

