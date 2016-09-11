using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace Youpload.Forms
{  
    public class UploadNotificationForm : Form
    {   
        public UploadNotificationFormConfig ToastConfig { get; private set; }

        public int Duration { get; private set; }
        public int FadeDuration { get; private set; }

        private int windowOffset = 3;  
        private int fadeInterval = 50;
        private float opacityDecrement;
        private Font textFont;
        private int textPadding = 5;
        private int urlPadding = 3;
        private Size textRenderSize;

        public UploadNotificationForm(int duration, int fadeDuration, ContentAlignment placement, Size size, UploadNotificationFormConfig config)
        {
            InitializeComponent();                        

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            Duration = duration;
            FadeDuration = fadeDuration;

            opacityDecrement = (float)fadeInterval / FadeDuration;

            ToastConfig = config;
            textFont = new Font("Arial", 10);

            if (config.Image != null)
            {
                config.Image = ImageHelpers.ResizeImageLimit(config.Image, size);
                config.Image = ImageHelpers.DrawCheckers(config.Image);
                size = new Size(config.Image.Width + 2, config.Image.Height + 2);
            }
            else if (!string.IsNullOrEmpty(config.Text))
            {
                textRenderSize = Helpers.MeasureText(config.Text, textFont, size.Width - textPadding * 2);
                size = new Size(textRenderSize.Width + textPadding * 2, textRenderSize.Height + textPadding * 2 + 2);
            }

            Point position = Helpers.GetPosition(placement, new Point(windowOffset, windowOffset), Screen.PrimaryScreen.WorkingArea.Size, size);

            NativeMethods.SetWindowPos(Handle, (IntPtr)SpecialWindowHandles.HWND_TOPMOST, position.X + Screen.PrimaryScreen.WorkingArea.X,
                position.Y + Screen.PrimaryScreen.WorkingArea.Y, size.Width, size.Height, SetWindowPosFlags.SWP_NOACTIVATE);

            if (Duration <= 0)
            {
                DurationEnd();
            }
            else
            {
                tDuration.Interval = Duration;
                tDuration.Start();
            }
        }

        private void tDuration_Tick(object sender, EventArgs e)
        {
            DurationEnd();
        }

        private void DurationEnd()
        {         
            tDuration.Stop();
       
            StartClosing();            
        }

        private void StartClosing()
        {
            if (FadeDuration <= 0)
            {
                Close();
            }
            else
            {
                Opacity = 1;
                tOpacity.Interval = fadeInterval;
                tOpacity.Start();
            }
        }

        private void tOpacity_Tick(object sender, EventArgs e)
        {
            if (Opacity > opacityDecrement)
            {
                Opacity -= opacityDecrement;
            }
            else
            {
                Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle rect = e.ClipRectangle;

            if (ToastConfig.Image != null)
            {
                g.DrawImage(ToastConfig.Image, 1, 1, ToastConfig.Image.Width, ToastConfig.Image.Height);

                if (!string.IsNullOrEmpty(ToastConfig.URL))
                {
                    Rectangle textRect = new Rectangle(0, 0, rect.Width, 40);

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                    {
                        g.FillRectangle(brush, textRect);
                    }

                    g.DrawString(ToastConfig.URL, textFont, Brushes.White, textRect.Offset(-urlPadding));
                }
            }   
                  

            g.DrawRectangleProper(Pens.Black, rect);
        }  

        private void NotificationForm_MouseClick(object sender, MouseEventArgs e)
        {       
            if (e.Button == MouseButtons.Left)
            {
                URLHelpers.OpenURL(ToastConfig.FilePath);
            }

            Close();
        }


        public static void Show(int duration, int fadeDuration, ContentAlignment placement, Size size, UploadNotificationFormConfig config)
        {
            if ((duration > 0 || fadeDuration > 0) && size.Width > 0 && size.Height > 0)
            {
                config.Image = ImageHelpers.LoadImage(config.FilePath);

                if (config.Image != null || !string.IsNullOrEmpty(config.Text))
                {
                    UploadNotificationForm form = new UploadNotificationForm(duration, fadeDuration, placement, size, config);
                    NativeMethods.ShowWindow(form.Handle, (int)WindowShowStyle.ShowNoActivate);
                }
            }
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.Timer tDuration;
        private System.Windows.Forms.Timer tOpacity;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (ToastConfig != null)
            {
                ToastConfig.Dispose();
            }

            if (textFont != null)
            {
                textFont.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tDuration = new System.Windows.Forms.Timer(this.components);
            this.tOpacity = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tDuration
            // 
            this.tDuration.Tick += new System.EventHandler(this.tDuration_Tick);
            // 
            // tOpacity
            // 
            this.tOpacity.Tick += new System.EventHandler(this.tOpacity_Tick);
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotificationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "NotificationForm";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotificationForm_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code
    }
}
