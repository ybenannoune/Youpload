﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2020 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ShareX.HelpersLib
{
    public partial class DownloaderForm : BlackStyleForm
    {
        public delegate void DownloaderInstallEventHandler(string filePath);
        public event DownloaderInstallEventHandler InstallRequested;

        public string URL { get; set; }
        public string Filename { get; set; }
        public string DownloadLocation { get; private set; }
        public IWebProxy Proxy { get; set; }
        public string AcceptHeader { get; set; }
        public bool AutoStartDownload { get; set; }
        public InstallType InstallType { get; set; }
        public bool AutoStartInstall { get; set; }
        public DownloaderFormStatus Status { get; private set; }
        public bool RunInstallerInBackground { get; set; }

        private FileDownloader fileDownloader;

        private DownloaderForm()
        {
            InitializeComponent();

            ChangeStatus(Resources.DownloaderForm_DownloaderForm_Waiting_);
            Status = DownloaderFormStatus.Waiting;
            AutoStartDownload = true;
            InstallType = InstallType.Silent;
            AutoStartInstall = true;
            RunInstallerInBackground = true;
        }

        public DownloaderForm(string url, string filename) : this()
        {
            URL = url;
            Filename = filename;
            lblFilename.Text = Helpers.SafeStringFormat(Resources.DownloaderForm_DownloaderForm_Filename___0_, Filename);
        }

        public DownloaderForm(UpdateChecker updateChecker) : this(updateChecker.DownloadURL, updateChecker.Filename)
        {
            Proxy = updateChecker.Proxy;

            if (updateChecker is GitHubUpdateChecker)
            {
                AcceptHeader = "application/octet-stream";
            }
        }

        private void DownloaderForm_Shown(object sender, EventArgs e)
        {
            if (AutoStartDownload)
            {
                StartDownload();
            }
        }

        private void btnAction_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Status == DownloaderFormStatus.Waiting)
                {
                    StartDownload();
                }
                else if (Status == DownloaderFormStatus.DownloadCompleted)
                {
                    DialogResult = DialogResult.OK;
                    Install();
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }

        public void Install()
        {
            if (Status == DownloaderFormStatus.DownloadCompleted)
            {
                Status = DownloaderFormStatus.InstallStarted;
                btnAction.Enabled = false;
                RunInstallerWithDelay();
                Close();
            }
        }

        // This function will give time for ShareX to close so installer won't tell ShareX is already running
        private void RunInstallerWithDelay(int delay = 1000)
        {
            if (RunInstallerInBackground)
            {
                Thread thread = new Thread(() =>
                {
                    Thread.Sleep(delay);
                    RunInstaller();
                });
                thread.Start();
            }
            else
            {
                Hide();
                RunInstaller();
            }
        }

        private void RunInstaller()
        {
            if (InstallType == InstallType.Event)
            {
                OnInstallRequested();
            }
            else
            {
                try
                {
                    using (Process process = new Process())
                    {
                        ProcessStartInfo psi = new ProcessStartInfo()
                        {
                            FileName = DownloadLocation,
                            Arguments = "/UPDATE",
                            UseShellExecute = true
                        };

                        if (InstallType == InstallType.Silent)
                        {
                            psi.Arguments += " /SILENT";
                        }
                        else if (InstallType == InstallType.VerySilent)
                        {
                            psi.Arguments += " /VERYSILENT";
                        }

                        if (Helpers.IsDefaultInstallDir())
                        {
                            psi.Verb = "runas";
                        }

                        process.StartInfo = psi;
                        process.Start();
                    }
                }
                catch
                {
                }
            }
        }

        protected void OnInstallRequested()
        {
            if (InstallRequested != null)
            {
                DialogResult = DialogResult.OK;
                InstallRequested(DownloadLocation);
            }
        }

        private void ChangeStatus(string status)
        {
            lblStatus.Text = Helpers.SafeStringFormat(Resources.DownloaderForm_ChangeStatus_Status___0_, status);
        }

        private void ChangeProgress()
        {
            if (fileDownloader != null)
            {
                pbProgress.Value = (int)Math.Round(fileDownloader.DownloadPercentage);
                lblProgress.Text = Helpers.SafeStringFormat(CultureInfo.CurrentCulture, Resources.DownloaderForm_ChangeProgress_Progress,
                    fileDownloader.DownloadPercentage, fileDownloader.DownloadSpeed / 1024, fileDownloader.DownloadedSize / 1024, fileDownloader.FileSize / 1024);
            }
        }

        private void StartDownload()
        {
            if (!string.IsNullOrEmpty(URL) && Status == DownloaderFormStatus.Waiting)
            {
                Status = DownloaderFormStatus.DownloadStarted;
                btnAction.Text = Resources.DownloaderForm_StartDownload_Cancel;

                string folderPath = Path.Combine(Path.GetTempPath(), "ShareX");
                Helpers.CreateDirectoryFromDirectoryPath(folderPath);
                DownloadLocation = Path.Combine(folderPath, Filename);

                fileDownloader = new FileDownloader(URL, DownloadLocation, Proxy, AcceptHeader);
                fileDownloader.FileSizeReceived += (v1, v2) => ChangeProgress();
                fileDownloader.DownloadStarted += (v1, v2) => ChangeStatus(Resources.DownloaderForm_StartDownload_Downloading_);
                fileDownloader.ProgressChanged += (v1, v2) => ChangeProgress();
                fileDownloader.DownloadCompleted += fileDownloader_DownloadCompleted;
                fileDownloader.ExceptionThrown += (v1, v2) => ChangeStatus(fileDownloader.LastException.Message);
                fileDownloader.StartDownload();

                ChangeStatus(Resources.DownloaderForm_StartDownload_Getting_file_size_);
            }
        }

        private void fileDownloader_DownloadCompleted(object sender, EventArgs e)
        {
            ChangeStatus(Resources.DownloaderForm_fileDownloader_DownloadCompleted_Download_completed_);
            Status = DownloaderFormStatus.DownloadCompleted;
            btnAction.Text = Resources.DownloaderForm_fileDownloader_DownloadCompleted_Install;

            if (AutoStartInstall)
            {
                Install();
            }
        }

        private void DownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Status == DownloaderFormStatus.DownloadStarted && fileDownloader != null)
            {
                fileDownloader.StopDownload();
            }
        }
    }
}