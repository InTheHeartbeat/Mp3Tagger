using System;

namespace Mp3Tagger.Kernel.Base.Attributes
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
