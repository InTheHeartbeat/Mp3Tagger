using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mp3Tagger.Kernel.Settings
{
    public class SettingsRepository
    {
        public readonly string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Mp3Tagger/Settings.xml";

        public SettingsKit LoadSettings()
        {
            if (!Directory.Exists(Path.GetDirectoryName(SettingsPath)) || !File.Exists(SettingsPath))
                throw new IOException("Settings not found");
            

            SettingsKit result = null;

            XmlSerializer deserializer = new XmlSerializer(typeof(SettingsKit));

            using (FileStream fs = new FileStream(SettingsPath, FileMode.Open))
            {
                result = (SettingsKit) deserializer.Deserialize(fs);                
            }

            return result;
        }

        public void SaveSettings(SettingsKit toSave)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsKit));

            if (!Directory.Exists(Path.GetDirectoryName(SettingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

            using (FileStream fs = new FileStream(SettingsPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs,toSave);
            }
        }
    }
}
