using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Kernel.Settings.Display
{
    public class DisplaySettings
    {
        public CompositionsDataGridDisplaySettings CompositionsDataGridDisplay { get; set; }

        public DisplaySettings()
        {
            
        }

        public void InitializeByDefault()
        {
            CompositionsDataGridDisplay = new CompositionsDataGridDisplaySettings();        
            CompositionsDataGridDisplay.InitializeByDefault();
        }
    }
}
