namespace Youpload.Forms
{
    partial class NotificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl_UploadStr = new System.Windows.Forms.Label();
            this.tDuration = new System.Windows.Forms.Timer(this.components);
            this.pictureBox_Img = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Img)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_UploadStr
            // 
            this.lbl_UploadStr.AutoSize = true;
            this.lbl_UploadStr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UploadStr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.lbl_UploadStr.Location = new System.Drawing.Point(158, 48);
            this.lbl_UploadStr.Name = "lbl_UploadStr";
            this.lbl_UploadStr.Size = new System.Drawing.Size(96, 15);
            this.lbl_UploadStr.TabIndex = 0;
            this.lbl_UploadStr.Text = "Server response";
            // 
            // tDuration
            // 
            this.tDuration.Interval = 2500;
            this.tDuration.Tick += new System.EventHandler(this.tDuration_Tick);
            // 
            // pictureBox_Img
            // 
            this.pictureBox_Img.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Img.Name = "pictureBox_Img";
            this.pictureBox_Img.Size = new System.Drawing.Size(140, 92);
            this.pictureBox_Img.TabIndex = 1;
            this.pictureBox_Img.TabStop = false;
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(434, 114);
            this.Controls.Add(this.pictureBox_Img);
            this.Controls.Add(this.lbl_UploadStr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationForm";
            this.ShowInTaskbar = false;
            this.Text = "FormTest";
            this.Click += new System.EventHandler(this.NotificationForm_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Img)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_UploadStr;
        private System.Windows.Forms.PictureBox pictureBox_Img;
        private System.Windows.Forms.Timer tDuration;
    }
}