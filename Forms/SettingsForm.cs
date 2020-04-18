using Microsoft.Win32;
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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hotkeySelectionButton_Area.UpdateHotkey(Program.youploadSettings.AreaHotkey);
            hotkeySelectionButton_ClipBoard.UpdateHotkey(Program.youploadSettings.ClipboardHotkey);
            txt_SaveFolder.Text = Program.youploadSettings.PersonnalFolder;
        }

        private void btn_SelectSaveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txt_SaveFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btn_runStartup_Click(object sender, EventArgs e)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (rkApp.GetValue("Youpload") != null)
            {
                rkApp.DeleteValue("Youpload", false);
                MessageBox.Show("Youpload run at startup disabled.");
            }
            else
            {
                rkApp.SetValue("Youpload", System.Reflection.Assembly.GetExecutingAssembly().Location);
                MessageBox.Show("Youpload run at startup enabled.");
            }
        }
    }
}
