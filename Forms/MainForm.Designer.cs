namespace Youpload.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.captureAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.captureDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendClipboardToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Text = "Youpload";
            this.notifyIcon.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureAreaToolStripMenuItem,
            this.captureDesktopToolStripMenuItem,
            this.sendClipboardToolStripMenuItem1,
            this.uploadFileToolStripMenuItem1,
            this.toolStripMenuHistory,
            this.settingsToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(168, 158);
            // 
            // captureAreaToolStripMenuItem
            // 
            this.captureAreaToolStripMenuItem.Image = global::Youpload.Properties.Resources.area;
            this.captureAreaToolStripMenuItem.Name = "captureAreaToolStripMenuItem";
            this.captureAreaToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.captureAreaToolStripMenuItem.Text = "Capture Area";
            this.captureAreaToolStripMenuItem.Click += new System.EventHandler(this.captureAreaToolStripMenuItem_Click);
            // 
            // captureDesktopToolStripMenuItem
            // 
            this.captureDesktopToolStripMenuItem.Image = global::Youpload.Properties.Resources.fullscreen;
            this.captureDesktopToolStripMenuItem.Name = "captureDesktopToolStripMenuItem";
            this.captureDesktopToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.captureDesktopToolStripMenuItem.Text = "Capture Desktop";
            this.captureDesktopToolStripMenuItem.Click += new System.EventHandler(this.captureDesktopToolStripMenuItem_Click);
            // 
            // sendClipboardToolStripMenuItem1
            // 
            this.sendClipboardToolStripMenuItem1.Image = global::Youpload.Properties.Resources.clipboard;
            this.sendClipboardToolStripMenuItem1.Name = "sendClipboardToolStripMenuItem1";
            this.sendClipboardToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.sendClipboardToolStripMenuItem1.Text = "Upload Clipboard";
            this.sendClipboardToolStripMenuItem1.Click += new System.EventHandler(this.sendClipboardToolStripMenuItem1_Click);
            // 
            // uploadFileToolStripMenuItem1
            // 
            this.uploadFileToolStripMenuItem1.Image = global::Youpload.Properties.Resources.file;
            this.uploadFileToolStripMenuItem1.Name = "uploadFileToolStripMenuItem1";
            this.uploadFileToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.uploadFileToolStripMenuItem1.Text = "Upload File";
            this.uploadFileToolStripMenuItem1.Click += new System.EventHandler(this.uploadFileToolStripMenuItem1_Click);
            // 
            // toolStripMenuHistory
            // 
            this.toolStripMenuHistory.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuHistory.Image")));
            this.toolStripMenuHistory.Name = "toolStripMenuHistory";
            this.toolStripMenuHistory.Size = new System.Drawing.Size(167, 22);
            this.toolStripMenuHistory.Text = "History";
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 247);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem captureAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem captureDesktopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendClipboardToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuHistory;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

