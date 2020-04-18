using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using ShareX.ScreenCaptureLib;
using ShareX.HelpersLib;
using ShareX;
using System.Net;
using System.IO;
using MovablePython;

namespace Youpload.Forms
{  
    public partial class MainForm : Form
    {
        //
        private readonly int MAX_HISTORY = 10;

        //Hotkeys
        private Hotkey m_HotkeyAreaScreenshot;
        private Hotkey m_HotkeyClipBoard;
        private RegionCaptureTransparentForm rectangleTransparent;

        class UploadStatus
        {
            public enum UPLOAD_STATUS
            {
                OK = 200,
                ERROR = 500
            }

            public UploadStatus.UPLOAD_STATUS status { get; set; }
            public string data { get; set; }
        }

        private ImageTools.ImageSettings m_ImageSettings = new ImageTools.ImageSettings()
        {
            ImageFormat = EImageFormat.PNG,
            ImageJPEGQuality = 80,
            ImageAutoUseJPEG = true,
            ImageAutoUseJPEGSize = 150 //kilo-bytes
        };
        private Screenshot m_ScreenshotSettings = new Screenshot()
        {
            CaptureCursor = true,
            CaptureClientArea = false,
            RemoveOutsideScreenArea = true,
            CaptureShadow = true,
            ShadowOffset = 20,
            AutoHideTaskbar = false
        };

        public MainForm()
        {
            InitializeComponent();
            notifyIcon.Visible = true;    
            notifyIcon.Icon = this.Icon;
            RegisterHotKeys();
            Hide();
        }

        private void RegisterHotKeys()
        {
            m_HotkeyAreaScreenshot = new Hotkey(Program.youploadSettings.AreaHotkey.Hotkey);
            m_HotkeyAreaScreenshot.Pressed += captureAreaToolStripMenuItem_Click;
            if (!m_HotkeyAreaScreenshot.GetCanRegister(this))
            {
                Console.WriteLine("Already registered");
            }
            else
            {
                m_HotkeyAreaScreenshot.Register(this);
            }

            m_HotkeyClipBoard = new Hotkey(Program.youploadSettings.ClipboardHotkey.Hotkey);
            m_HotkeyClipBoard.Pressed += sendClipboardToolStripMenuItem1_Click;
            if (!m_HotkeyClipBoard.GetCanRegister(this))
            {
                Console.WriteLine("Already registered");
            }
            else
            {
                m_HotkeyClipBoard.Register(this);
            }
        }

       
        private void toolStripMenuHistory_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            Clipboard.SetText(item.Text);
        }

        private void pushIntoHistory(Image data, string text)
        {
            if (toolStripMenuHistory.DropDownItems.Count > MAX_HISTORY)
                toolStripMenuHistory.DropDownItems.RemoveAt(0);

            toolStripMenuHistory.DropDownItems.Add(text, data, new EventHandler(toolStripMenuHistory_Click));
        }

        private void UploadFile(string uploadUrl,string filePath, bool fileIsImage = false)
        {
            Image notifImage = (fileIsImage == true) ? Image.FromFile(filePath) : Properties.Resources.publications_clipboard;
            String notifText = "No response from server";
            using (var webClient = new WebClient())
            {
                try
                {
                    byte[] reponse = webClient.UploadFile(uploadUrl, filePath);
                    string str = Encoding.UTF8.GetString(reponse);
                    UploadStatus uploadStatus = JsonConvert.DeserializeObject<UploadStatus>(str);             
                    notifText = uploadStatus.data.ToString();
                    Clipboard.SetText(uploadStatus.data);
                }
                catch
                {
            
                }
                webClient.Dispose();
            }                  
           
            NativeMethods.ShowWindow(new NotificationForm(notifImage, notifText).Handle, (int)WindowShowStyle.ShowNoActivate);
            pushIntoHistory(notifImage, notifText);
        }
         

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Unregister Hotkeys 
            m_HotkeyAreaScreenshot.Unregister();
            m_HotkeyClipBoard.Unregister();

            //Show settings form
            new SettingsForm().ShowDialog();

            //Update current hotkeys
            m_HotkeyAreaScreenshot.KeyCode = Program.youploadSettings.AreaHotkey.KeyCode;
            m_HotkeyClipBoard.KeyCode = Program.youploadSettings.ClipboardHotkey.KeyCode;

            //Reenable hotkeys
            m_HotkeyAreaScreenshot.Register(this);
            m_HotkeyClipBoard.Register(this);

            //Save
            Program.youploadSettings.SaveSettings();
        }

        private void ScreenshotDone(Image img)
        {
            if (img != null)
            {
                ImageData imageData = ImageTools.PrepareImage(img, m_ImageSettings);
                string filePath = Program.youploadSettings.PersonnalFolder + "\\Screenshots\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "." + imageData.ImageFormat.ToString().ToLower();
                bool imageSaved = imageData.Write(filePath);
                if (imageSaved)
                {
                    UploadFile(Program.youploadSettings.UploadUrl,filePath,true);               
                }
            }
        }

        private void captureDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image img = m_ScreenshotSettings.CaptureFullscreen();
            if (img != null)
            {
                ScreenshotDone(img);
            }
        }

        private void uploadFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                UploadFile(Program.youploadSettings.UploadUrl, fileDialog.FileName);               
            }
        }

        private void sendClipboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string clipBoardContent = Clipboard.GetText();
            if (!string.IsNullOrEmpty(clipBoardContent))
            {
                string filePath = Program.youploadSettings.PersonnalFolder + "\\Clipboards\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
                Helpers.CreateDirectoryFromFilePath(filePath);
                File.WriteAllText(filePath, clipBoardContent);
                UploadFile(Program.youploadSettings.UploadUrl, filePath);               
            }
            else
            {
                NativeMethods.ShowWindow(new NotificationForm(Properties.Resources.publications_clipboard, "No textual clipboard available").Handle, (int)WindowShowStyle.ShowNoActivate);
            }
        }      

        private void captureAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(rectangleTransparent == null)
            {
                using (rectangleTransparent = new RegionCaptureTransparentForm())
                {
                    if (rectangleTransparent.ShowDialog() == DialogResult.OK)
                    {
                        Image img = rectangleTransparent.GetAreaImage(m_ScreenshotSettings);
                        ScreenshotDone(img);
                    }
                }
                rectangleTransparent.Dispose();
                rectangleTransparent = null;
            }        
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_HotkeyAreaScreenshot.Unregister();
            m_HotkeyClipBoard.Unregister();
            Application.Exit();
        }
    }
}
