using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Kernel.Settings
{
    public class SettingsController
    {
        private readonly SettingsRepository repository;
        public SettingsProvider Provider { get; private set; }
        public SettingsController()
        {
            repository = new SettingsRepository();
            Provider = new SettingsProvider();

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                Provider.CurrentSettings = repository.LoadSettings();
            }
            catch (IOException)
            {
                Provider.CurrentSettings.InitializeByDefault();
                repository.SaveSettings(Provider.CurrentSettings);
            }
        }

        public void SaveSettings()
        {
            repository.SaveSettings(Provider.CurrentSettings);
        }
    }
}
