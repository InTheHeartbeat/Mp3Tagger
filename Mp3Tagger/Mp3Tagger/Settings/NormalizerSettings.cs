using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Enums;

namespace Mp3Tagger.Settings
{
    public class NormalizerSettings
    {
        public bool Trimming { get; set; }
        public bool ChangeCase { get; set; }
        public bool RemoveChars { get; set; }
        public List<string> CharsToRemove { get; set; }
        public CaseChangeMode CaseChangeMode { get; set; }

        public NormalizerSettings()
        {
            CharsToRemove = new List<string>();
            Trimming = true;
            CaseChangeMode = CaseChangeMode.AllWordsUpper;
        }
    }
}
