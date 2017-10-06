using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Base.Attributes
{
    public class CustomFieldHeightRequired : Attribute
    {
        public int Height { get; set; }

        public CustomFieldHeightRequired(int height)
        {
            Height = height;
        }
    }
}
