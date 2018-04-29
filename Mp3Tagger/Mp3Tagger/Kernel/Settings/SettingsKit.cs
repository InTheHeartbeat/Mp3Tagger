using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Settings.Display;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel.Settings
{
    [Serializable]
    public class SettingsKit
    {
        
        public FeaturesSettings Features { get; set; }
        public DisplaySettings Display { get; set; }
        

        public SettingsKit()
        {
            
        }

        public void InitializeByDefault()
        {
            Features = new FeaturesSettings();
            Display = new DisplaySettings();

            Features.InitializeByDefault();
            Display.InitializeByDefault();            
        }  
    }
}
