using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel
{
    public class FeatureKitEntry
    {
        public FeatureName Name { get; set; }
        public IFeature Feature { get; set; }

        public FeatureKitEntry(FeatureName name, IFeature feature)
        {
            Name = name;
            Feature = feature;
        }
    }
}
