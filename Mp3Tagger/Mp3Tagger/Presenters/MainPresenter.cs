using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mp3Tagger.Enums;
using Mp3Tagger.Features;
using Mp3Tagger.Interfaces;
using Mp3Tagger.IO;
using Mp3Tagger.Models;
using Mp3Tagger.Settings;
using TagLib.Mpeg;

namespace Mp3Tagger.Presenters
{
    public sealed class MainPresenter : IPresenter
    {
        public List<Composition> Compositions;
                
        public event Action<IFeature, int> FeatureWorkStarted;
        public event Action<IFeature> FeatureWorkCompleted;
        public event Action<IFeature, int, int> FeatureProgressUpdated;

        public PatternRemoverSettings PatternRemoverSettings => patternRemover.PatternRemoverSettings;
        public NormalizerSettings NormalizerSettings => normalizer.Settings;

        private IView _view;

        private CompositionsLoader compositionsLoader;
        private EncodingFixer encodingFixer;
        private PatternRemover patternRemover;
        private Normalizer normalizer;


        public MainPresenter(IView view)
        {
            _view = view;
            Initialize();
        }

        private void Initialize()
        {
            encodingFixer = new EncodingFixer();
            compositionsLoader = new CompositionsLoader();
            patternRemover = new PatternRemover();
            patternRemover.Initialize(new PatternRemoverSettings());
            normalizer = new Normalizer();
            normalizer.Initialize(new NormalizerSettings());
            Compositions = new List<Composition>();
        }

        public async void ApplyFeatureForAll(Feature feature)
        {
            switch (feature)
            {
                case Feature.EncodingFixer:
                    await ApplyFeatureConcreteFeatureForAll(encodingFixer);
                    break;
                case Feature.PatternRemover:
                    await ApplyFeatureConcreteFeatureForAll(patternRemover);
                    break;
                case Feature.Normalizer:
                    await ApplyFeatureConcreteFeatureForAll(normalizer);
                    break;
            }
        }

        public async void ApplyFeatureForSelected(Feature feature, List<Composition> selected)
        {
            switch (feature)
            {
                    case Feature.EncodingFixer:
                        await ApplyFeatureConcreteFeatureForSelected(encodingFixer,selected);
                    break;
            }
        }

        public async void OpenCompositions(string path)
        {
            Compositions = new List<Composition>();            
            await compositionsLoader.Initialize(path);
            OnFeatureWorkStarted(compositionsLoader,compositionsLoader.FilesCount);
            await compositionsLoader.ApplyToList(Compositions,OnFeatureProgressUpdated,OnFeatureWorkCompleted);
        }

        private async Task ApplyFeatureConcreteFeatureForAll(IFeature featureInstace)
        {
            OnFeatureWorkStarted(featureInstace,Compositions.Count);
            await featureInstace.ApplyToList(Compositions, OnFeatureProgressUpdated, OnFeatureWorkCompleted);
        }
        private async Task ApplyFeatureConcreteFeatureForSelected(IFeature featureInstace, List<Composition> selected)
        {
            OnFeatureWorkStarted(featureInstace, Compositions.Count);
            await featureInstace.ApplyToList(selected, OnFeatureProgressUpdated, OnFeatureWorkCompleted);
        }

        #region Event invokators
        private void OnFeatureWorkStarted(IFeature feature,int operationCount)
        {
            FeatureWorkStarted?.Invoke(feature,operationCount);
        }
        private void OnFeatureWorkCompleted(IFeature feature)
        {
            FeatureWorkCompleted?.Invoke(feature);
        }
        private void OnFeatureProgressUpdated(IFeature feature, int performed, int of)
        {
            FeatureProgressUpdated?.Invoke(feature, performed, of);
        }
        #endregion
    }
}
