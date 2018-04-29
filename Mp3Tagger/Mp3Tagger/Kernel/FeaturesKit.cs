using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Settings;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel
{
    public class FeaturesKit
    {
        private List<FeatureKitEntry> features;

        public FeaturesKit()
        {            
            InitializeFeatures();
        }

        private void InitializeFeatures()
        {
            features = new List<FeatureKitEntry>
            {
                new FeatureKitEntry(FeatureName.FileSystemWalker,new FileSystemWalker(new FileSystemWalkerSettings() {MinFileBytes = -1})),
                new FeatureKitEntry(FeatureName.EncodingFixer,new EncodingFixer()),
                new FeatureKitEntry(FeatureName.CompositionLoader,new CompositionsLoader()),
                new FeatureKitEntry(FeatureName.PatternRemover,new PatternRemover()),
                new FeatureKitEntry(FeatureName.Normalizer,new Normalizer())
            };
        }

        public void SetFeatureSettings(FeatureName featureName, IFeatureSettings settings)
        {
            features.FirstOrDefault(f=>f.Name == featureName)?.Feature.Initialize(settings);
        }

        public FeatureKitEntry GetFeatureEntryByName(FeatureName featureName)
        {
            return features.FirstOrDefault(f => f.Name == featureName);
        }

        public FeatureKitEntry GetFeatureEntryByFeature(IFeature feature)
        {
            return features.FirstOrDefault(f => f.Feature == feature);
        }
    }
}
