using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Youpload.Forms;

namespace Youpload
{
    static class Program
    {
        public static YouploadSettings youploadSettings;
        private static Mutex singleInstanceMutex;

        private static void SetupPersonalFolder()
        {
            if (!Directory.Exists(youploadSettings.PersonnalFolder))
            {
                try
                {
                    Directory.CreateDirectory(youploadSettings.PersonnalFolder);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to create custom folder" + string.Format(" \"{0}\"\r\n\r\n{1}", youploadSettings.PersonnalFolder, e),
                        "Youplad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void Run()
        {
            youploadSettings = new YouploadSettings();
            youploadSettings.LoadSettings();

            SetupPersonalFolder();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Chargement du gestionnaire d'erreur standart
            string application_name = Path.GetFileName(Application.ExecutablePath);

            try
            {
                singleInstanceMutex = Mutex.OpenExisting(application_name);
                MessageBox.Show(String.Format("This application is already running.", application_name), "Youpload", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                singleInstanceMutex = new Mutex(true, application_name);
                Run();
            }
        }
    }
}
