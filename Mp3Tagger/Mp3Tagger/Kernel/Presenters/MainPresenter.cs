using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Settings;

namespace Mp3Tagger.Kernel.Presenters
{
    public sealed class MainPresenter : IPresenter
    {
        public List<Composition> Compositions;
                
        public ProcessState CurrentState { get; set; }

        public event Action<IFeature, int> FeatureWorkStarted;
        public event Action<IFeature> FeatureWorkCompleted;
        public event Action<IFeature, int, int> FeatureProgressUpdated;

        public PatternRemoverSettings PatternRemoverSettings => patternRemover.PatternRemoverSettings;
        public NormalizerSettings NormalizerSettings => normalizer.Settings;

        private IView _view;

        private FileSystemWalker fileSystemWalker;

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
            fileSystemWalker = new FileSystemWalker();
            encodingFixer = new EncodingFixer();
            compositionsLoader = new CompositionsLoader();
            patternRemover = new PatternRemover();
            patternRemover.Initialize(new PatternRemoverSettings());
            normalizer = new Normalizer();
            normalizer.Initialize(new NormalizerSettings());
            Compositions = new List<Composition>();
            CurrentState = new ProcessState();
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
            fileSystemWalker.Initialize(path);
            List<string> searchedFiles = new List<string>();
            OnFeatureWorkStarted(fileSystemWalker,0);
            await fileSystemWalker.ApplyToList(searchedFiles, OnFeatureProgressUpdated, OnFeatureWorkCompleted);

            Compositions = new List<Composition>();
            compositionsLoader.Initialize(searchedFiles);
            OnFeatureWorkStarted(compositionsLoader, searchedFiles.Count);
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
            CurrentState.IsBusy = true;
            CurrentState.CurrentFeatureName = feature.Name;
            CurrentState.History.Add(feature.Name);
            FeatureWorkStarted?.Invoke(feature,operationCount);
        }
        private void OnFeatureWorkCompleted(IFeature feature)
        {
            CurrentState.IsBusy = false;
            CurrentState.CurrentFeatureName = "Ready";
            FeatureWorkCompleted?.Invoke(feature);
        }
        private void OnFeatureProgressUpdated(IFeature feature, int performed, int of)
        {
            CurrentState.OperationsCount = of;            
            CurrentState.OperationsPerformed = performed;
            FeatureProgressUpdated?.Invoke(feature, performed, of);
        }
        #endregion
    }
}
