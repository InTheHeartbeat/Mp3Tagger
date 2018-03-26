using System;
using System.Collections.Generic;
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

        public SettingsProvider()
        {
            CurrentSettings = new SettingsKit();
        }

        public T GetSettings<T>()
        {
            return (T) CurrentSettings.GetSettings<T>();            
        }

        public IFeatureSettings GetSettingsByFeatureName(FeatureName name)
        {
            switch (name)
            {
                case FeatureName.EncodingFixer:
                    return CurrentSettings.GetSettings<EncodingFixerSettings>();                   
                case FeatureName.PatternRemover:
                    return CurrentSettings.GetSettings<PatternRemoverSettings>();                    
                case FeatureName.Normalizer:
                    return CurrentSettings.GetSettings<NormalizerSettings>();                    
                case FeatureName.FileSystemWalker:
                    return CurrentSettings.GetSettings<FileSystemWalkerSettings>();                    
                case FeatureName.CompositionLoader:
                    return CurrentSettings.GetSettings<CompositionsLoaderSettings>();                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(name), name, null);
            }
        }
    }
}
