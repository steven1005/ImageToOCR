namespace ImageToOCR
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.btnBrowerImg = new System.Windows.Forms.Button();
            this.tbCaptureResult = new System.Windows.Forms.TextBox();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarLab = new System.Windows.Forms.ToolStripStatusLabel();
            this.ckbCopyRectBitmap = new System.Windows.Forms.CheckBox();
            this.timerRectCapture = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(0, 0);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(345, 302);
            this.pbImage.TabIndex = 1;
            this.pbImage.TabStop = false;
            this.pbImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.pbImage_DragDrop);
            this.pbImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.pbImage_DragEnter);
            // 
            // btnBrowerImg
            // 
            this.btnBrowerImg.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBrowerImg.Location = new System.Drawing.Point(351, 248);
            this.btnBrowerImg.Name = "btnBrowerImg";
            this.btnBrowerImg.Size = new System.Drawing.Size(146, 54);
            this.btnBrowerImg.TabIndex = 0;
            this.btnBrowerImg.Text = "選擇圖片";
            this.btnBrowerImg.UseVisualStyleBackColor = true;
            this.btnBrowerImg.Click += new System.EventHandler(this.btnBrowerImg_Click);
            // 
            // tbCaptureResult
            // 
            this.tbCaptureResult.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbCaptureResult.Location = new System.Drawing.Point(0, 308);
            this.tbCaptureResult.Name = "tbCaptureResult";
            this.tbCaptureResult.Size = new System.Drawing.Size(345, 36);
            this.tbCaptureResult.TabIndex = 2;
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(352, 311);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(144, 32);
            this.cmbLanguage.TabIndex = 3;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLab});
            this.statusBar.Location = new System.Drawing.Point(0, 359);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(509, 22);
            this.statusBar.TabIndex = 4;
            this.statusBar.Text = "statusBar";
            // 
            // statusBarLab
            // 
            this.statusBarLab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarLab.Name = "statusBarLab";
            this.statusBarLab.Size = new System.Drawing.Size(41, 17);
            this.statusBarLab.Text = "Status";
            // 
            // ckbCopyRectBitmap
            // 
            this.ckbCopyRectBitmap.AutoSize = true;
            this.ckbCopyRectBitmap.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ckbCopyRectBitmap.Location = new System.Drawing.Point(359, 14);
            this.ckbCopyRectBitmap.Name = "ckbCopyRectBitmap";
            this.ckbCopyRectBitmap.Size = new System.Drawing.Size(125, 28);
            this.ckbCopyRectBitmap.TabIndex = 5;
            this.ckbCopyRectBitmap.Text = "框選貼上";
            this.ckbCopyRectBitmap.UseVisualStyleBackColor = true;
            this.ckbCopyRectBitmap.CheckedChanged += new System.EventHandler(this.ckbCopyRectBitmap_CheckedChanged);
            // 
            // timerRectCapture
            // 
            this.timerRectCapture.Interval = 1000;
            this.timerRectCapture.Tick += new System.EventHandler(this.timerRectCapture_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 381);
            this.Controls.Add(this.ckbCopyRectBitmap);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.tbCaptureResult);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.btnBrowerImg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Button btnBrowerImg;
        private System.Windows.Forms.TextBox tbCaptureResult;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLab;
        private System.Windows.Forms.CheckBox ckbCopyRectBitmap;
        private System.Windows.Forms.Timer timerRectCapture;
    }
}

