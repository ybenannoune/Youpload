using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;

namespace Youpload.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if(hotkeyControl_AreaScreenshot.Hotkey == Keys.None || hotkeyControl_AreaScreenshot.Hotkey == Keys.None)
            {
                MessageBox.Show(this, "You can't use invalid hotkeys");
                e.Cancel = true;
            }           
            */

            Program.globalSetting.AreaHotkey = hotkeyControl_AreaScreenshot.GetHotkeyInfo();
            Program.globalSetting.ClipboardHotkey = hotkeyControl_SendClipboard.GetHotkeyInfo();

            if (Directory.Exists(txt_SaveFolder.Text) )
            {
                Program.globalSetting.PersonnalFolder = txt_SaveFolder.Text;
            }              
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            hotkeyControl_AreaScreenshot.SetHotkeyInfo(Program.globalSetting.AreaHotkey);
            hotkeyControl_SendClipboard.SetHotkeyInfo(Program.globalSetting.ClipboardHotkey);
            txt_SaveFolder.Text = Program.globalSetting.PersonnalFolder;
        }

        private void btn_SelectSaveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if( folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txt_SaveFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btn_runStartup_Click(object sender, EventArgs e)
        {        
            //  The added registry key isn't working

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (isElevated)
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp.GetValue("Youpload") != null)
                {
                    rkApp.DeleteValue("Youpload", false);
                    MessageBox.Show("Youpload entry is deleted!");
                }
                else
                {
                    rkApp.SetValue("Youpload", Application.ExecutablePath);
                    MessageBox.Show("Youpload entry has been added");
                }
            }
            else
            {
                MessageBox.Show("Please restart the application with admin rights");
            }
        }
    }
}
