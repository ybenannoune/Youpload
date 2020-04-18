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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace ShareX.HelpersLib
{
    public class FileDownloader
    {
        public string URL { get; private set; }
        public string DownloadLocation { get; private set; }
        public bool IsDownloading { get; private set; }
        public bool IsCanceled { get; private set; }
        public bool IsPaused { get; private set; }
        public long FileSize { get; private set; }
        public long DownloadedSize { get; private set; }
        public double DownloadSpeed { get; private set; }

        public double DownloadPercentage
        {
            get
            {
                if (FileSize > 0)
                {
                    return (double)DownloadedSize / FileSize * 100;
                }

                return 0;
            }
        }

        public IWebProxy Proxy { get; set; }
        public string AcceptHeader { get; set; }
        public Exception LastException { get; private set; }

        public event EventHandler FileSizeReceived, DownloadStarted, ProgressChanged, DownloadCompleted, ExceptionThrown;

        private BackgroundWorker worker;
        private const int bufferSize = 32768;

        public FileDownloader(string url, string downloadLocation, IWebProxy proxy = null, string acceptHeader = null)
        {
            URL = url;
            DownloadLocation = downloadLocation;
            Proxy = proxy;
            AcceptHeader = acceptHeader;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        public void StartDownload()
        {
            if (!IsDownloading && !string.IsNullOrEmpty(URL) && !worker.IsBusy)
            {
                IsDownloading = true;
                IsCanceled = false;
                IsPaused = false;

                worker.RunWorkerAsync();
            }
        }

        public void StopDownload()
        {
            IsCanceled = true;
        }

        public void PauseDownload()
        {
            IsPaused = true;
        }

        public void ResumeDownload()
        {
            IsPaused = false;
        }

        private void ThrowEvent(EventHandler eventHandler)
        {
            worker.ReportProgress(0, eventHandler);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.UserAgent = ShareXResources.UserAgent;
                request.Proxy = Proxy;

                if (!string.IsNullOrEmpty(AcceptHeader))
                {
                    request.Accept = AcceptHeader;
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    FileSize = response.ContentLength;

                    ThrowEvent(FileSizeReceived);

                    if (FileSize > 0)
                    {
                        Stopwatch timer = new Stopwatch();
                        Stopwatch progressEventTimer = new Stopwatch();
                        long speedTest = 0;

                        byte[] buffer = new byte[(int)Math.Min(bufferSize, FileSize)];
                        int bytesRead;

                        using (FileStream fs = new FileStream(DownloadLocation, FileMode.Create, FileAccess.Write, FileShare.Read))
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            ThrowEvent(DownloadStarted);

                            while (DownloadedSize < FileSize && !worker.CancellationPending)
                            {
                                while (IsPaused && !IsCanceled)
                                {
                                    timer.Reset();
                                    Thread.Sleep(10);
                                }

                                if (IsCanceled)
                                {
                                    return;
                                }

                                if (!timer.IsRunning)
                                {
                                    timer.Start();
                                }

                                if (!progressEventTimer.IsRunning)
                                {
                                    progressEventTimer.Start();
                                }

                                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, bytesRead);
                                DownloadedSize += bytesRead;
                                speedTest += bytesRead;

                                if (timer.ElapsedMilliseconds > 500)
                                {
                                    DownloadSpeed = (double)speedTest / timer.ElapsedMilliseconds * 1000;
                                    speedTest = 0;
                                    timer.Reset();
                                }

                                if (progressEventTimer.ElapsedMilliseconds > 100)
                                {
                                    ThrowEvent(ProgressChanged);

                                    progressEventTimer.Reset();
                                }
                            }

                            ThrowEvent(ProgressChanged);
                            ThrowEvent(DownloadCompleted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!IsCanceled)
                {
                    LastException = ex;

                    ThrowEvent(ExceptionThrown);
                }
            }
            finally
            {
                if (IsCanceled)
                {
                    try
                    {
                        if (File.Exists(DownloadLocation))
                        {
                            File.Delete(DownloadLocation);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            EventHandler eventHandler = e.UserState as EventHandler;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsDownloading = false;
        }
    }
}