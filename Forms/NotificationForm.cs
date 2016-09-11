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
    public class NotificationForm : Form
    {
        const int FORM_WIDTH = 360;
        const int FORM_HEIGHT = 100;

        public int Duration { get; private set; }
        public int FadeDuration { get; private set; }

        private int windowOffset = 3;
        private int fadeInterval = 50;
        private float opacityDecrement;
        private Font textFont;
        private int textPadding = 5;    

        private int iconSize = 40;

        //Notification Data
        private string displayText;
        private Bitmap displayIcon;

        public NotificationForm(int duration, int fadeDuration, ContentAlignment placement, Bitmap icon, string text)
        {
            InitializeComponent();

            displayIcon = icon;
            displayText = text;

            Duration = duration;
            FadeDuration = fadeDuration;              

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            opacityDecrement = (float)fadeInterval / FadeDuration;

            textFont = new Font("Arial", 10);

            Size size = new Size( FORM_WIDTH, FORM_HEIGHT );
        
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

            Rectangle fullRect = new Rectangle(0, 0, rect.Width + iconSize, rect.Height);
            Rectangle textRect = new Rectangle(iconSize + 12, 12, rect.Width - (iconSize + 12), rect.Height);
            Rectangle iconRect = new Rectangle(12, 15, iconSize, iconSize);

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 31, 31, 31)))
            {
                g.FillRectangle(brush, fullRect);
            }
                       
            g.DrawImage(displayIcon, iconRect);
            g.DrawString(displayText, textFont, Brushes.DarkGray, textRect.Offset(-textPadding));       

            //Display Border
            g.DrawRectangleProper(Pens.Gray, rect);
        }  

        private void NotificationForm_MouseClick(object sender, MouseEventArgs e)
        {                
            Close();
        }

        public static void Show(int duration, int fadeDuration, ContentAlignment placement, Bitmap icon ,string content)
        {
            if (duration > 0 || fadeDuration > 0)
            {       
                NotificationForm form = new NotificationForm(duration, fadeDuration, placement, icon , content);
                NativeMethods.ShowWindow(form.Handle, (int)WindowShowStyle.ShowNoActivate);            
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
