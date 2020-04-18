using System;
using System.IO;
using Newtonsoft.Json;
using ShareX.HelpersLib;
using System.Windows.Forms;

namespace Youpload
{
    public class YouploadSettings
    {
        private const string AppName = "Youpload";
        
        private static HotkeyInfo DefaultAreaHotkey = new HotkeyInfo(Keys.D3 | Keys.Control | Keys.Shift);
        private static HotkeyInfo DefaultClipboardHotkey = new HotkeyInfo(Keys.D4 | Keys.Control | Keys.Shift);
        private static string DefaultPersonnalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName);
        private static string SettingsFile = Path.Combine(DefaultPersonnalFolder, "Settings.json");

        //This is the url used for upload, the server used for youpload send back a link to the upload.
        [JsonIgnore]
        public readonly string UploadUrl = "";

        public string PersonnalFolder;
        public HotkeyInfo AreaHotkey;
        public HotkeyInfo ClipboardHotkey;
     
        public void LoadSettings()
        {         
            bool foundSettings = File.Exists(SettingsFile);
            if( foundSettings )
            {
                try
                {
                    // deserialize JSON directly from a file
                    using (StreamReader file = File.OpenText(SettingsFile))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        YouploadSettings settings = (YouploadSettings)serializer.Deserialize(file, typeof(YouploadSettings));
                        this.PersonnalFolder = settings.PersonnalFolder;
                        this.AreaHotkey = settings.AreaHotkey;
                        this.ClipboardHotkey = settings.ClipboardHotkey;
                        file.Dispose();
                    }
                }
                catch
                {
                    LoadDefaults();
                }
        
            }
            else
            {
                LoadDefaults();
            }
        }

        public void SaveSettings()
        {
            JsonSerializer serializer = new JsonSerializer();        
            try
            {
                using (StreamWriter sw = new StreamWriter(SettingsFile))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                    sw.Dispose();                    
                }
            }    
            catch
            {

            }      
        }

        private void LoadDefaults()
        {
            PersonnalFolder = DefaultPersonnalFolder;
            AreaHotkey = DefaultAreaHotkey;
            ClipboardHotkey = DefaultClipboardHotkey;
        }
    }

}
