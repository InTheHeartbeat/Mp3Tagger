using System.Collections.Generic;
using System.Linq;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel
{
    public class FeaturesKit
    {
        private List<FeatureKitEntry> _features;

        public FeaturesKit()
        {            
            InitializeFeatures();
        }

        private void InitializeFeatures()
        {
            _features = new List<FeatureKitEntry>
            {
                new FeatureKitEntry(FeatureName.FileSystemWalker,new FileSystemWalker(new FileSystemWalkerSettings() {MinFileBytes = -1})),
                new FeatureKitEntry(FeatureName.EncodingFixer,new EncodingFixer()),
                new FeatureKitEntry(FeatureName.CompositionLoader,new CompositionsLoader()),
                new FeatureKitEntry(FeatureName.PatternRemover,new PatternRemover()),
                new FeatureKitEntry(FeatureName.Normalizer,new Normalizer())
            };
        }

        public void SetFeatureSettings(FeatureName featureName, ISettings settings)
        {
            _features.FirstOrDefault(f=>f.Name == featureName)?.Feature.Initialize(settings);
        }

        public FeatureKitEntry GetFeatureEntryByName(FeatureName featureName)
        {
            return _features.FirstOrDefault(f => f.Name == featureName);
        }

        public FeatureKitEntry GetFeatureEntryByFeature(IFeature feature)
        {
            return _features.FirstOrDefault(f => f.Feature == feature);
        }

        public void SetFeaturesSettings(FeaturesSettings currentSettingsFeatures)
        {
            _features.ForEach(feature=>feature.Feature.Initialize(currentSettingsFeatures));
        }
    }
}
