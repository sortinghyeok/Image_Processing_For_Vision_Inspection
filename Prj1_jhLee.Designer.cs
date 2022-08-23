
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
            this.btn_Gaussian = new System.Windows.Forms.Button();
            this.btn_Laplace = new System.Windows.Forms.Button();
            this.btn_FFT = new System.Windows.Forms.Button();
            this.btn_Template = new System.Windows.Forms.Button();
            this.photo_Executed = new System.Windows.Forms.PictureBox();
            this.btn_OpenImage = new System.Windows.Forms.Button();
            this.btn_SaveImage = new System.Windows.Forms.Button();
            this.btn_Expand = new System.Windows.Forms.Button();
            this.btn_Contract = new System.Windows.Forms.Button();
            this.btn_HistoEqualizer = new System.Windows.Forms.Button();
            this.btn_OtsuThresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.photo_bmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.photo_Executed)).BeginInit();
            this.SuspendLayout();
            // 
            // photo_bmp
            // 
            this.photo_bmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photo_bmp.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.photo_bmp.Location = new System.Drawing.Point(12, 25);
            this.photo_bmp.Name = "photo_bmp";
            this.photo_bmp.Size = new System.Drawing.Size(500, 500);
            this.photo_bmp.TabIndex = 0;
            this.photo_bmp.TabStop = false;
            // 
            // btn_Gaussian
            // 
            this.btn_Gaussian.Location = new System.Drawing.Point(1050, 322);
            this.btn_Gaussian.Name = "btn_Gaussian";
            this.btn_Gaussian.Size = new System.Drawing.Size(126, 35);
            this.btn_Gaussian.TabIndex = 2;
            this.btn_Gaussian.Text = "필터 가우스";
            this.btn_Gaussian.UseVisualStyleBackColor = true;
            // 
            // btn_Laplace
            // 
            this.btn_Laplace.Location = new System.Drawing.Point(1050, 363);
            this.btn_Laplace.Name = "btn_Laplace";
            this.btn_Laplace.Size = new System.Drawing.Size(126, 35);
            this.btn_Laplace.TabIndex = 3;
            this.btn_Laplace.Text = "필터 라플라스";
            this.btn_Laplace.UseVisualStyleBackColor = true;
            // 
            // btn_FFT
            // 
            this.btn_FFT.Location = new System.Drawing.Point(1050, 404);
            this.btn_FFT.Name = "btn_FFT";
            this.btn_FFT.Size = new System.Drawing.Size(126, 35);
            this.btn_FFT.TabIndex = 4;
            this.btn_FFT.Text = "필터 FFT";
            this.btn_FFT.UseVisualStyleBackColor = true;
            // 
            // btn_Template
            // 
            this.btn_Template.Location = new System.Drawing.Point(1050, 445);
            this.btn_Template.Name = "btn_Template";
            this.btn_Template.Size = new System.Drawing.Size(126, 35);
            this.btn_Template.TabIndex = 5;
            this.btn_Template.Text = "템플릿 매칭";
            this.btn_Template.UseVisualStyleBackColor = true;
            // 
            // photo_Executed
            // 
            this.photo_Executed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photo_Executed.Location = new System.Drawing.Point(530, 25);
            this.photo_Executed.Name = "photo_Executed";
            this.photo_Executed.Size = new System.Drawing.Size(500, 500);
            this.photo_Executed.TabIndex = 6;
            this.photo_Executed.TabStop = false;
            // 
            // btn_OpenImage
            // 
            this.btn_OpenImage.Location = new System.Drawing.Point(1050, 27);
            this.btn_OpenImage.Name = "btn_OpenImage";
            this.btn_OpenImage.Size = new System.Drawing.Size(126, 35);
            this.btn_OpenImage.TabIndex = 7;
            this.btn_OpenImage.Text = "이미지1 열기";
            this.btn_OpenImage.UseVisualStyleBackColor = true;
            this.btn_OpenImage.Click += new System.EventHandler(this.btn_OpenImage_Click);
            // 
            // btn_SaveImage
            // 
            this.btn_SaveImage.Location = new System.Drawing.Point(1050, 68);
            this.btn_SaveImage.Name = "btn_SaveImage";
            this.btn_SaveImage.Size = new System.Drawing.Size(126, 35);
            this.btn_SaveImage.TabIndex = 8;
            this.btn_SaveImage.Text = "이미지2 저장";
            this.btn_SaveImage.UseVisualStyleBackColor = true;
            // 
            // btn_Expand
            // 
            this.btn_Expand.Location = new System.Drawing.Point(1050, 126);
            this.btn_Expand.Name = "btn_Expand";
            this.btn_Expand.Size = new System.Drawing.Size(126, 35);
            this.btn_Expand.TabIndex = 9;
            this.btn_Expand.Text = "팽창";
            this.btn_Expand.UseVisualStyleBackColor = true;
            this.btn_Expand.Click += new System.EventHandler(this.btn_Expand_Click);
            // 
            // btn_Contract
            // 
            this.btn_Contract.Location = new System.Drawing.Point(1050, 167);
            this.btn_Contract.Name = "btn_Contract";
            this.btn_Contract.Size = new System.Drawing.Size(126, 35);
            this.btn_Contract.TabIndex = 10;
            this.btn_Contract.Text = "수축";
            this.btn_Contract.UseVisualStyleBackColor = true;
            this.btn_Contract.Click += new System.EventHandler(this.btn_Contract_Click);
            // 
            // btn_HistoEqualizer
            // 
            this.btn_HistoEqualizer.Location = new System.Drawing.Point(1050, 223);
            this.btn_HistoEqualizer.Name = "btn_HistoEqualizer";
            this.btn_HistoEqualizer.Size = new System.Drawing.Size(126, 35);
            this.btn_HistoEqualizer.TabIndex = 11;
            this.btn_HistoEqualizer.Text = "히스토그램 평활화";
            this.btn_HistoEqualizer.UseVisualStyleBackColor = true;
            // 
            // btn_OtsuThresh
            // 
            this.btn_OtsuThresh.Location = new System.Drawing.Point(1050, 264);
            this.btn_OtsuThresh.Name = "btn_OtsuThresh";
            this.btn_OtsuThresh.Size = new System.Drawing.Size(126, 35);
            this.btn_OtsuThresh.TabIndex = 12;
            this.btn_OtsuThresh.Text = "오츠 이진화";
            // 
            // Prj1_jhLee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 547);
            this.Controls.Add(this.btn_OtsuThresh);
            this.Controls.Add(this.btn_HistoEqualizer);
            this.Controls.Add(this.btn_Contract);
            this.Controls.Add(this.btn_Expand);
            this.Controls.Add(this.btn_SaveImage);
            this.Controls.Add(this.btn_OpenImage);
            this.Controls.Add(this.photo_Executed);
            this.Controls.Add(this.btn_Template);
            this.Controls.Add(this.btn_FFT);
            this.Controls.Add(this.btn_Laplace);
            this.Controls.Add(this.btn_Gaussian);
            this.Controls.Add(this.photo_bmp);
            this.Name = "Prj1_jhLee";
            this.Text = "Prj1_JhLee";
            this.Load += new System.EventHandler(this.Prj1_jhLee_Load);
            ((System.ComponentModel.ISupportInitialize)(this.photo_bmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.photo_Executed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox photo_bmp;
        private System.Windows.Forms.Button btn_Gaussian;
        private System.Windows.Forms.Button btn_Laplace;
        private System.Windows.Forms.Button btn_FFT;
        private System.Windows.Forms.Button btn_Template;
        private System.Windows.Forms.PictureBox photo_Executed;
        private System.Windows.Forms.Button btn_OpenImage;
        private System.Windows.Forms.Button btn_SaveImage;
        private System.Windows.Forms.Button btn_Expand;
        private System.Windows.Forms.Button btn_Contract;
        private System.Windows.Forms.Button btn_HistoEqualizer;
        private System.Windows.Forms.Button btn_OtsuThresh;
    }
}

