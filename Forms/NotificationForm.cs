using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Youpload.Forms
{
    public partial class NotificationForm : Form
    {
        private int windowOffset = 3;

        public NotificationForm(Image data, String text)
        {
            InitializeComponent();
            pictureBox_Img.Image = data;
            pictureBox_Img.SizeMode = PictureBoxSizeMode.StretchImage;
            lbl_UploadStr.Text = text;

            Point position = Helpers.GetPosition(ContentAlignment.BottomRight, new Point(windowOffset, windowOffset), Screen.PrimaryScreen.WorkingArea.Size, this.Size);
            NativeMethods.SetWindowPos(Handle, (IntPtr)SpecialWindowHandles.HWND_TOPMOST, position.X + Screen.PrimaryScreen.WorkingArea.X,
                position.Y + Screen.PrimaryScreen.WorkingArea.Y, this.Width, this.Height, SetWindowPosFlags.SWP_NOACTIVATE);

            tDuration.Start();

            //Audio Pop
            System.IO.Stream str = Properties.Resources.pop;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void tDuration_Tick(object sender, EventArgs e)
        {
            tDuration.Stop();
            Close();
        }

        private void NotificationForm_Click(object sender, EventArgs e)
        {
            tDuration.Stop();
            Close();
        }
    }
}
