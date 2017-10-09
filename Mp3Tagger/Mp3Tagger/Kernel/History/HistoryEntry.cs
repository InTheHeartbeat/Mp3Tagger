using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.History
{
    public class HistoryEntry
    {
        public FeatureKitEntry UsedFeature { get; set; }
        public TimeSpan Elapsed { get; set; }

        public override string ToString()
        {
            return UsedFeature.Name + " of " + Elapsed.ToString(@"ss\.ff\s");
        }
    }
}
