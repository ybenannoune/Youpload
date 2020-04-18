namespace Youpload.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.btn_runStartup = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SelectSaveFolder = new System.Windows.Forms.Button();
            this.txt_SaveFolder = new System.Windows.Forms.TextBox();
            this.hotkeySelectionButton_Area = new ShareX.HelpersLib.HotkeySelectionButton();
            this.hotkeySelectionButton_ClipBoard = new ShareX.HelpersLib.HotkeySelectionButton();
            this.SuspendLayout();
            // 
            // btn_runStartup
            // 
            this.btn_runStartup.Location = new System.Drawing.Point(136, 94);
            this.btn_runStartup.Name = "btn_runStartup";
            this.btn_runStartup.Size = new System.Drawing.Size(210, 23);
            this.btn_runStartup.TabIndex = 13;
            this.btn_runStartup.Text = "Enable/Disable run at startup";
            this.btn_runStartup.UseVisualStyleBackColor = true;
            this.btn_runStartup.Click += new System.EventHandler(this.btn_runStartup_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Upoad Clipboard Hotkey";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Capture Area Hotkey";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Local Folder";
            // 
            // btn_SelectSaveFolder
            // 
            this.btn_SelectSaveFolder.Location = new System.Drawing.Point(352, 8);
            this.btn_SelectSaveFolder.Name = "btn_SelectSaveFolder";
            this.btn_SelectSaveFolder.Size = new System.Drawing.Size(33, 23);
            this.btn_SelectSaveFolder.TabIndex = 9;
            this.btn_SelectSaveFolder.Text = "...";
            this.btn_SelectSaveFolder.UseVisualStyleBackColor = true;
            this.btn_SelectSaveFolder.Click += new System.EventHandler(this.btn_SelectSaveFolder_Click);
            // 
            // txt_SaveFolder
            // 
            this.txt_SaveFolder.Location = new System.Drawing.Point(136, 10);
            this.txt_SaveFolder.Name = "txt_SaveFolder";
            this.txt_SaveFolder.Size = new System.Drawing.Size(210, 20);
            this.txt_SaveFolder.TabIndex = 8;
            // 
            // hotkeySelectionButton_Area
            // 
            this.hotkeySelectionButton_Area.Location = new System.Drawing.Point(136, 36);
            this.hotkeySelectionButton_Area.Name = "hotkeySelectionButton_Area";
            this.hotkeySelectionButton_Area.Size = new System.Drawing.Size(210, 23);
            this.hotkeySelectionButton_Area.TabIndex = 14;
            this.hotkeySelectionButton_Area.UseVisualStyleBackColor = true;
            // 
            // hotkeySelectionButton_ClipBoard
            // 
            this.hotkeySelectionButton_ClipBoard.Location = new System.Drawing.Point(136, 65);
            this.hotkeySelectionButton_ClipBoard.Name = "hotkeySelectionButton_ClipBoard";
            this.hotkeySelectionButton_ClipBoard.Size = new System.Drawing.Size(210, 23);
            this.hotkeySelectionButton_ClipBoard.TabIndex = 15;
            this.hotkeySelectionButton_ClipBoard.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 125);
            this.Controls.Add(this.hotkeySelectionButton_ClipBoard);
            this.Controls.Add(this.hotkeySelectionButton_Area);
            this.Controls.Add(this.btn_runStartup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_SelectSaveFolder);
            this.Controls.Add(this.txt_SaveFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_runStartup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SelectSaveFolder;
        private System.Windows.Forms.TextBox txt_SaveFolder;
        private ShareX.HelpersLib.HotkeySelectionButton hotkeySelectionButton_Area;
        private ShareX.HelpersLib.HotkeySelectionButton hotkeySelectionButton_ClipBoard;
    }
}