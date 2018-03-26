using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel.Settings
{
    [Serializable]
    public class SettingsKit
    {
        public FeaturesSettingsList FeaturesSettings { get; set; }        

        public SettingsKit()
        {
            
        }

        public void InitializeByDefault()
        {
            FeaturesSettings = new FeaturesSettingsList
            {
                new NormalizerSettings(),
                new CompositionsLoaderSettings(),
                new FileSystemWalkerSettings(),
                new PatternRemoverSettings()
            };
            FeaturesSettings.ForEach(st=>st.InitializeByDefault());
        }

        public IFeatureSettings GetSettings<T>()
        {
            return FeaturesSettings.FirstOrDefault(s => s.GetType() == typeof(T));
        }
    }
}
