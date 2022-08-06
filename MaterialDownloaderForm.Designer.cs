namespace AmbientCGMaterialDownloader
{
    partial class MaterialDownloaderForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.categoryCbxList = new System.Windows.Forms.CheckedListBox();
            this._1kCbx = new System.Windows.Forms.CheckBox();
            this._2kCbx = new System.Windows.Forms.CheckBox();
            this._4kCbx = new System.Windows.Forms.CheckBox();
            this._8kCbx = new System.Windows.Forms.CheckBox();
            this.jpgCbx = new System.Windows.Forms.CheckBox();
            this.pngCbx = new System.Windows.Forms.CheckBox();
            this.unzipCbx = new System.Windows.Forms.CheckBox();
            this.delPreviewImgCbx = new System.Windows.Forms.CheckBox();
            this.delUsdaFileCbx = new System.Windows.Forms.CheckBox();
            this.delUsdcFileCbx = new System.Windows.Forms.CheckBox();
            this.downloadPgb = new System.Windows.Forms.ProgressBar();
            this.downloadBtn = new System.Windows.Forms.Button();
            this.downloadInfoLbl = new System.Windows.Forms.Label();
            this.outPathFbd = new System.Windows.Forms.FolderBrowserDialog();
            this.catDownloadPgb = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // categoryCbxList
            // 
            this.categoryCbxList.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.categoryCbxList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.categoryCbxList.FormattingEnabled = true;
            this.categoryCbxList.Location = new System.Drawing.Point(4, 3);
            this.categoryCbxList.Name = "categoryCbxList";
            this.categoryCbxList.Size = new System.Drawing.Size(196, 306);
            this.categoryCbxList.TabIndex = 0;
            // 
            // _1kCbx
            // 
            this._1kCbx.AutoSize = true;
            this._1kCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this._1kCbx.Checked = true;
            this._1kCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this._1kCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._1kCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._1kCbx.Location = new System.Drawing.Point(206, 34);
            this._1kCbx.Name = "_1kCbx";
            this._1kCbx.Size = new System.Drawing.Size(36, 19);
            this._1kCbx.TabIndex = 3;
            this._1kCbx.Text = "1K";
            this._1kCbx.UseVisualStyleBackColor = false;
            // 
            // _2kCbx
            // 
            this._2kCbx.AutoSize = true;
            this._2kCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this._2kCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._2kCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._2kCbx.Location = new System.Drawing.Point(251, 34);
            this._2kCbx.Name = "_2kCbx";
            this._2kCbx.Size = new System.Drawing.Size(36, 19);
            this._2kCbx.TabIndex = 4;
            this._2kCbx.Text = "2K";
            this._2kCbx.UseVisualStyleBackColor = false;
            // 
            // _4kCbx
            // 
            this._4kCbx.AutoSize = true;
            this._4kCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this._4kCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._4kCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._4kCbx.Location = new System.Drawing.Point(296, 34);
            this._4kCbx.Name = "_4kCbx";
            this._4kCbx.Size = new System.Drawing.Size(36, 19);
            this._4kCbx.TabIndex = 5;
            this._4kCbx.Text = "4K";
            this._4kCbx.UseVisualStyleBackColor = false;
            // 
            // _8kCbx
            // 
            this._8kCbx.AutoSize = true;
            this._8kCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this._8kCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._8kCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._8kCbx.Location = new System.Drawing.Point(341, 34);
            this._8kCbx.Name = "_8kCbx";
            this._8kCbx.Size = new System.Drawing.Size(36, 19);
            this._8kCbx.TabIndex = 6;
            this._8kCbx.Text = "8K";
            this._8kCbx.UseVisualStyleBackColor = false;
            // 
            // jpgCbx
            // 
            this.jpgCbx.AutoSize = true;
            this.jpgCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.jpgCbx.Checked = true;
            this.jpgCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.jpgCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jpgCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.jpgCbx.Location = new System.Drawing.Point(206, 74);
            this.jpgCbx.Name = "jpgCbx";
            this.jpgCbx.Size = new System.Drawing.Size(42, 19);
            this.jpgCbx.TabIndex = 7;
            this.jpgCbx.Text = "JPG";
            this.jpgCbx.UseVisualStyleBackColor = false;
            // 
            // pngCbx
            // 
            this.pngCbx.AutoSize = true;
            this.pngCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pngCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pngCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pngCbx.Location = new System.Drawing.Point(251, 74);
            this.pngCbx.Name = "pngCbx";
            this.pngCbx.Size = new System.Drawing.Size(47, 19);
            this.pngCbx.TabIndex = 8;
            this.pngCbx.Text = "PNG";
            this.pngCbx.UseVisualStyleBackColor = false;
            // 
            // unzipCbx
            // 
            this.unzipCbx.AutoSize = true;
            this.unzipCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.unzipCbx.Checked = true;
            this.unzipCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.unzipCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unzipCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.unzipCbx.Location = new System.Drawing.Point(206, 113);
            this.unzipCbx.Name = "unzipCbx";
            this.unzipCbx.Size = new System.Drawing.Size(53, 19);
            this.unzipCbx.TabIndex = 9;
            this.unzipCbx.Text = "Unzip";
            this.unzipCbx.UseVisualStyleBackColor = false;
            this.unzipCbx.CheckedChanged += new System.EventHandler(this.unzipCbx_CheckedChanged);
            // 
            // delPreviewImgCbx
            // 
            this.delPreviewImgCbx.AutoSize = true;
            this.delPreviewImgCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.delPreviewImgCbx.Checked = true;
            this.delPreviewImgCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.delPreviewImgCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delPreviewImgCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.delPreviewImgCbx.Location = new System.Drawing.Point(206, 152);
            this.delPreviewImgCbx.Name = "delPreviewImgCbx";
            this.delPreviewImgCbx.Size = new System.Drawing.Size(136, 19);
            this.delPreviewImgCbx.TabIndex = 11;
            this.delPreviewImgCbx.Text = "Delete Preview Image";
            this.delPreviewImgCbx.UseVisualStyleBackColor = false;
            // 
            // delUsdaFileCbx
            // 
            this.delUsdaFileCbx.AutoSize = true;
            this.delUsdaFileCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.delUsdaFileCbx.Checked = true;
            this.delUsdaFileCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.delUsdaFileCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delUsdaFileCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.delUsdaFileCbx.Location = new System.Drawing.Point(206, 177);
            this.delUsdaFileCbx.Name = "delUsdaFileCbx";
            this.delUsdaFileCbx.Size = new System.Drawing.Size(108, 19);
            this.delUsdaFileCbx.TabIndex = 12;
            this.delUsdaFileCbx.Text = "Delete USDA file";
            this.delUsdaFileCbx.UseVisualStyleBackColor = false;
            // 
            // delUsdcFileCbx
            // 
            this.delUsdcFileCbx.AutoSize = true;
            this.delUsdcFileCbx.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.delUsdcFileCbx.Checked = true;
            this.delUsdcFileCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.delUsdcFileCbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delUsdcFileCbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.delUsdcFileCbx.Location = new System.Drawing.Point(206, 202);
            this.delUsdcFileCbx.Name = "delUsdcFileCbx";
            this.delUsdcFileCbx.Size = new System.Drawing.Size(108, 19);
            this.delUsdcFileCbx.TabIndex = 13;
            this.delUsdcFileCbx.Text = "Delete USDC file";
            this.delUsdcFileCbx.UseVisualStyleBackColor = false;
            // 
            // downloadPgb
            // 
            this.downloadPgb.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.downloadPgb.ForeColor = System.Drawing.SystemColors.GrayText;
            this.downloadPgb.Location = new System.Drawing.Point(206, 272);
            this.downloadPgb.Name = "downloadPgb";
            this.downloadPgb.Size = new System.Drawing.Size(166, 10);
            this.downloadPgb.TabIndex = 14;
            this.downloadPgb.Visible = false;
            // 
            // downloadBtn
            // 
            this.downloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.downloadBtn.Location = new System.Drawing.Point(206, 288);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(166, 23);
            this.downloadBtn.TabIndex = 15;
            this.downloadBtn.Text = "Download";
            this.downloadBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new System.EventHandler(this.downloadBtn_Click);
            // 
            // downloadInfoLbl
            // 
            this.downloadInfoLbl.AutoSize = true;
            this.downloadInfoLbl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.downloadInfoLbl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.downloadInfoLbl.Location = new System.Drawing.Point(206, 238);
            this.downloadInfoLbl.Name = "downloadInfoLbl";
            this.downloadInfoLbl.Size = new System.Drawing.Size(22, 15);
            this.downloadInfoLbl.TabIndex = 16;
            this.downloadInfoLbl.Text = "---";
            this.downloadInfoLbl.Visible = false;
            // 
            // catDownloadPgb
            // 
            this.catDownloadPgb.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.catDownloadPgb.ForeColor = System.Drawing.SystemColors.GrayText;
            this.catDownloadPgb.Location = new System.Drawing.Point(206, 256);
            this.catDownloadPgb.Name = "catDownloadPgb";
            this.catDownloadPgb.Size = new System.Drawing.Size(166, 10);
            this.catDownloadPgb.TabIndex = 17;
            this.catDownloadPgb.Visible = false;
            // 
            // MaterialDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(384, 322);
            this.Controls.Add(this.catDownloadPgb);
            this.Controls.Add(this.downloadInfoLbl);
            this.Controls.Add(this.downloadBtn);
            this.Controls.Add(this.downloadPgb);
            this.Controls.Add(this.delUsdcFileCbx);
            this.Controls.Add(this.delUsdaFileCbx);
            this.Controls.Add(this.delPreviewImgCbx);
            this.Controls.Add(this.unzipCbx);
            this.Controls.Add(this.pngCbx);
            this.Controls.Add(this.jpgCbx);
            this.Controls.Add(this._8kCbx);
            this.Controls.Add(this._4kCbx);
            this.Controls.Add(this._2kCbx);
            this.Controls.Add(this._1kCbx);
            this.Controls.Add(this.categoryCbxList);
            this.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MaterialDownloaderForm";
            this.Text = "AmbientCG Texture Downloader";
            this.Load += new System.EventHandler(this.TextureDownloaderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckedListBox categoryCbxList;
        private CheckBox _1kCbx;
        private CheckBox _2kCbx;
        private CheckBox _4kCbx;
        private CheckBox _8kCbx;
        private CheckBox jpgCbx;
        private CheckBox pngCbx;
        private CheckBox unzipCbx;
        private CheckBox delPreviewImgCbx;
        private CheckBox delUsdaFileCbx;
        private CheckBox delUsdcFileCbx;
        private ProgressBar downloadPgb;
        private Button downloadBtn;
        private Label downloadInfoLbl;
        private FolderBrowserDialog outPathFbd;
        private ProgressBar catDownloadPgb;
    }
}