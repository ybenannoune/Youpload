namespace Youpload.Forms
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.txt_SaveFolder = new System.Windows.Forms.TextBox();
            this.btn_SelectSaveFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hotkeyControl_SendClipboard = new exscape.HotkeyControl();
            this.hotkeyControl_AreaScreenshot = new exscape.HotkeyControl();
            this.btn_runStartup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_SaveFolder
            // 
            this.txt_SaveFolder.Location = new System.Drawing.Point(150, 24);
            this.txt_SaveFolder.Name = "txt_SaveFolder";
            this.txt_SaveFolder.Size = new System.Drawing.Size(210, 20);
            this.txt_SaveFolder.TabIndex = 2;
            // 
            // btn_SelectSaveFolder
            // 
            this.btn_SelectSaveFolder.Location = new System.Drawing.Point(366, 22);
            this.btn_SelectSaveFolder.Name = "btn_SelectSaveFolder";
            this.btn_SelectSaveFolder.Size = new System.Drawing.Size(33, 23);
            this.btn_SelectSaveFolder.TabIndex = 3;
            this.btn_SelectSaveFolder.Text = "...";
            this.btn_SelectSaveFolder.UseVisualStyleBackColor = true;
            this.btn_SelectSaveFolder.Click += new System.EventHandler(this.btn_SelectSaveFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Local Folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Capture Area Hotkey";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Upoad Clipboard Hotkey";
            // 
            // hotkeyControl_SendClipboard
            // 
            this.hotkeyControl_SendClipboard.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControl_SendClipboard.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControl_SendClipboard.Location = new System.Drawing.Point(260, 76);
            this.hotkeyControl_SendClipboard.Name = "hotkeyControl_SendClipboard";
            this.hotkeyControl_SendClipboard.Size = new System.Drawing.Size(100, 20);
            this.hotkeyControl_SendClipboard.TabIndex = 1;
            this.hotkeyControl_SendClipboard.Text = "None";
            // 
            // hotkeyControl_AreaScreenshot
            // 
            this.hotkeyControl_AreaScreenshot.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControl_AreaScreenshot.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControl_AreaScreenshot.Location = new System.Drawing.Point(260, 50);
            this.hotkeyControl_AreaScreenshot.Name = "hotkeyControl_AreaScreenshot";
            this.hotkeyControl_AreaScreenshot.Size = new System.Drawing.Size(100, 20);
            this.hotkeyControl_AreaScreenshot.TabIndex = 0;
            this.hotkeyControl_AreaScreenshot.Text = "None";
            // 
            // btn_runStartup
            // 
            this.btn_runStartup.Location = new System.Drawing.Point(260, 102);
            this.btn_runStartup.Name = "btn_runStartup";
            this.btn_runStartup.Size = new System.Drawing.Size(100, 23);
            this.btn_runStartup.TabIndex = 7;
            this.btn_runStartup.Text = "Run at startup";
            this.btn_runStartup.UseVisualStyleBackColor = true;
            this.btn_runStartup.Click += new System.EventHandler(this.btn_runStartup_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(423, 137);
            this.Controls.Add(this.btn_runStartup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_SelectSaveFolder);
            this.Controls.Add(this.txt_SaveFolder);
            this.Controls.Add(this.hotkeyControl_SendClipboard);
            this.Controls.Add(this.hotkeyControl_AreaScreenshot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private exscape.HotkeyControl hotkeyControl_AreaScreenshot;
        private exscape.HotkeyControl hotkeyControl_SendClipboard;
        private System.Windows.Forms.TextBox txt_SaveFolder;
        private System.Windows.Forms.Button btn_SelectSaveFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_runStartup;
    }
}