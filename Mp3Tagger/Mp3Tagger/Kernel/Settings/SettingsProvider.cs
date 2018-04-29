using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Settings.Features;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings
{
    public class SettingsProvider
    {
        public SettingsKit CurrentSettings { get; set; }

        private readonly SettingsRepository _repository;

        public SettingsProvider()
        {
            CurrentSettings = new SettingsKit();
            _repository = new SettingsRepository();

            Initialize();
        }
      
        private void Initialize()
        {
            try
            {
                CurrentSettings = _repository.LoadSettings();
            }
            catch (IOException)
            {
                CurrentSettings.InitializeByDefault();
                _repository.SaveSettings(CurrentSettings);
            }
        }

        public void SaveSettings()
        {
            _repository.SaveSettings(CurrentSettings);
        }
    }
}
