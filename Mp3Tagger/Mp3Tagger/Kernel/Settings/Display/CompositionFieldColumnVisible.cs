using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;

namespace Mp3Tagger.Kernel.Settings.Display
{
    [Serializable]
    public class CompositionFieldColumnVisible
    {
        private bool v;

        public CompositionField Field { get; set; }
        public bool Visible { get; set; }

        public CompositionFieldColumnVisible()
        {
            
        }

        public CompositionFieldColumnVisible(CompositionField field, bool v)
        {
            Field = field;
            this.v = v;
        }
    }
}
