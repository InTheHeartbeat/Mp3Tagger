using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        private IView _view;

        private CompositionsLoader compositionsLoader;
        private EncodingFixer encodingFixer;
        private PatternRemover patternRemover;



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
        }

        public async void OpenCompositions(string path)
        {
            Compositions = new List<Composition>();            
            await compositionsLoader.Initialize(path);
            OnFeatureWorkStarted(compositionsLoader,compositionsLoader.FilesCount);
            compositionsLoader.ApplyToList(Compositions,OnFeatureProgressUpdated,OnFeatureWorkCompleted);
        }               

        public async void FixAllEncoding()
        {
            OnFeatureWorkStarted(encodingFixer,Compositions.Count);
            encodingFixer.ApplyToList(Compositions, OnFeatureProgressUpdated, OnFeatureWorkCompleted);
        }

        public async void FixSelectedEncoding(List<Composition> selected)
        {
            OnFeatureWorkStarted(encodingFixer,selected.Count);
            encodingFixer.ApplyToList(selected,OnFeatureProgressUpdated,OnFeatureWorkCompleted);
        }

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
    }
}
