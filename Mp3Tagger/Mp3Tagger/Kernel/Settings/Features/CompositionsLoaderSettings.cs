using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings.Features
{
    [Serializable]
    public class CompositionsLoaderSettings:ISettings
    {

        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = "TimeSinceLastEvent")]
        public string MinDurationForLoadString
        {
            get
            {
                return XmlConvert.ToString(MinDurationForLoad);
            }
            set
            {
                MinDurationForLoad = string.IsNullOrEmpty(value) ?
                    TimeSpan.MaxValue : XmlConvert.ToTimeSpan(value);
            }
        }

        [XmlIgnore]
        public TimeSpan MinDurationForLoad { get; set; }

        public void InitializeByDefault()
        {
            MinDurationForLoad = TimeSpan.FromMinutes(59);
        }
    }
}
