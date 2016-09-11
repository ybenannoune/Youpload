using System;
using System.IO;
using Newtonsoft.Json;
using Youpload.Hotkeys;

namespace Youpload
{
    public class GlobalSettings
    {
        private const string AppName = "Youpload";

        [JsonIgnore]
        static HotkeyInfo DefaultAreaHotkey = new HotkeyInfo(System.Windows.Forms.Keys.D3, true, true, false, false);
        [JsonIgnore]
        static HotkeyInfo DefaultClipboardHotkey = new HotkeyInfo(System.Windows.Forms.Keys.D4, true, true, false, false);
        private static string DefaultPersonnalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName);
        private static string SettingsFile = Path.Combine(DefaultPersonnalFolder, "Settings.json");
                
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
                        GlobalSettings settings = (GlobalSettings)serializer.Deserialize(file, typeof(GlobalSettings));
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
