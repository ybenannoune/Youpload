using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Youpload.Hotkeys;
using ShareX.ScreenCaptureLib;
using ShareX.HelpersLib;

namespace Youpload.Forms
{
    public partial class MainForm : Form
    {
        //Hotkeys
        private GlobalHotkey m_HotkeyAreaScreenshot;
        private GlobalHotkey m_HotkeyClipBoard;

        //Image Settings
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
        }        

        private void MainForm_Load(object sender, EventArgs e)
        {          
            RegisterHotKeys();
            Hide();                  
        }

        private void ShowNotification( string text, bool useBallonTip = false)
        {
            //For some reason, ShowBallonTip isn't working on all machines
            if( useBallonTip )
            {       
                notifyIcon.ShowBalloonTip(10000,"Youpload",text,ToolTipIcon.Info);                
            }
            else
            {
                NotificationForm.Show(1000, 300, ContentAlignment.BottomRight, global::Youpload.Properties.Resources.info_outline, text);
            }           
        }

        #region HotKeysRegistering

        private void RegisterHotKeys()
        {
            m_HotkeyAreaScreenshot = new GlobalHotkey(Program.globalSetting.AreaHotkey);
            m_HotkeyAreaScreenshot.Pressed += Hk_AreaScreenshot;
            if (!m_HotkeyAreaScreenshot.GetCanRegister(this))
            {
                Console.WriteLine("Already registered");
            }
            else
            {
                m_HotkeyAreaScreenshot.Register(this);
            }

            
            m_HotkeyClipBoard = new GlobalHotkey(Program.globalSetting.ClipboardHotkey);
            m_HotkeyClipBoard.Pressed += Hk_ClipBoard;
            if (!m_HotkeyClipBoard.GetCanRegister(this))
            {
                Console.WriteLine("Already registered");
            }
            else
            {
                m_HotkeyClipBoard.Register(this);
            }            
        }   

        #endregion

        private void UploadFile(string path)
        {       
            byte[] reponse = null;
            if (UploadHelper.UploadFile(Program.globalSetting.UploadUrl, path,ref reponse))
            {
                string str = Encoding.UTF8.GetString(reponse);
                UploadStatus upload = JsonConvert.DeserializeObject<UploadStatus>(str);             

                if ( upload.status == UploadStatus.STATUS_OK)
                {
                    ClipboardHelper.WriteUrl(upload.data);
                    ShowNotification(upload.data);    
                }
                else if(upload.status == UploadStatus.STATUS_ERROR)
                {
                    ShowNotification("Upload failed reason :" + upload.data);                   
                }                
            }
            else
            {
                ShowNotification("No response from server");         
            }    
        }
                
        private void SendClipBoard()
        {
            string clipBoardContent = Clipboard.GetText();
            if (!string.IsNullOrEmpty(clipBoardContent))
            {
                string filePath = Program.globalSetting.PersonnalFolder + "\\Clipboards\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
                Helpers.CreateDirectoryFromFilePath(filePath);
                System.IO.File.WriteAllText(filePath, clipBoardContent);        
                UploadFile(filePath);
            }
            else
            {
                ShowNotification("Your clipboard is not a string or empty");               
            }
        }

        private void ScreenshotDone(Image img)
        {
            if (img != null)
            {
                ImageData imageData = ImageTools.PrepareImage(img, m_ImageSettings);

                string filePath = Program.globalSetting.PersonnalFolder + "\\Screenshots\\" +  DateTime.Now.ToString("yyyyMMdd-HHmmss") + "." + imageData.ImageFormat.ToString().ToLower();

                bool imageSaved = imageData.Write(filePath);

                if (imageSaved)
                {
                    UploadFile(filePath);
                }
            }
        }

        private void ScreenshotArea()
        {       
            using (RegionCaptureTransparentForm rectangleTransparent = new RegionCaptureTransparentForm())
            {
                if (rectangleTransparent.ShowDialog() == DialogResult.OK)
                {
                    Image img = rectangleTransparent.GetAreaImage(m_ScreenshotSettings);            
                    ScreenshotDone(img);
                }
            }
        }
        

        #region HotkeysEvents

        private void Hk_AreaScreenshot(object sender, HandledEventArgs handledEventArgs)
        {
            ScreenshotArea();
        }

        private void Hk_ClipBoard(object sender, HandledEventArgs handledEventArgs)
        {
            SendClipBoard();
        }

        #endregion
        

        #region ToolSriptMenuEvents
        
        private void areaScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenshotArea();
        }
        
        private void sendClipBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendClipBoard();
        }
   
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                UploadFile(fileDialog.FileName);
            }
        }

        private void fullScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image img = m_ScreenshotSettings.CaptureFullscreen();          
            if (img != null)
            {
                ScreenshotDone(img);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Unregister Hotkeys 
            m_HotkeyAreaScreenshot.Unregister();
            m_HotkeyClipBoard.Unregister();

            //Show settings form
            new Settings().ShowDialog();

            //Update current hotkeys
            m_HotkeyAreaScreenshot.UpdateHotkey(Program.globalSetting.AreaHotkey);
            m_HotkeyClipBoard.UpdateHotkey(Program.globalSetting.ClipboardHotkey);

            //Reenable hotkeys
            m_HotkeyAreaScreenshot.Register(this);
            m_HotkeyClipBoard.Register(this);

            //Save
            Program.globalSetting.SaveSettings();
        }

        #endregion  
    }
}
