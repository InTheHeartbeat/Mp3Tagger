using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.History;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Settings;

namespace Mp3Tagger.Kernel.Presenters
{
    public sealed class MainPresenter : IPresenter
    {
        public ObservableCollection<Composition> CurrentCompositions;

        public event Action<IFeature, ProcessingState> FeatureStarted;
        public event Action<IFeature, ProcessingState> FeatureCompleted;
        public event Action<IFeature, ProcessingState> FeatureStateUpdated;

        public Mp3TaggerKernel Kernel { get; set; }

        public MainPresenter(IView view)
        {            
            Initialize();
        }

        private void Initialize()
        {            
            Kernel = new Mp3TaggerKernel();            
            CurrentCompositions = new ObservableCollection<Composition>();

            Kernel.ProcessingFeatureRunner.ProcessingStarted += OnFeatureStarted;
            Kernel.ProcessingFeatureRunner.ProcessingStateUpdated += OnFeatureStateUpdated;
            Kernel.ProcessingFeatureRunner.ProcessingCompleted += OnFeatureCompleted;
            Kernel.IoFeatureRunner.ProcessingStarted += OnFeatureStarted;
            Kernel.IoFeatureRunner.ProcessingStateUpdated += OnFeatureStateUpdated;
            Kernel.IoFeatureRunner.ProcessingCompleted += OnFeatureCompleted;
        }

        public async void ApplyFeatureForAll(FeatureName featureName)
        {            
            await Kernel.ProcessingFeatureRunner.PerformProcessorByList(CurrentCompositions,
                (IProcessingFeature) Kernel.Features.GetFeatureEntryByName(featureName).Feature);                        
        }
       

        public async void OpenCompositions(string path)
        {
            await Kernel.InitilizeCompositions(path);            
        }

        private void OnFeatureStarted(IFeature arg1, ProcessingState arg2)
        {
            FeatureStarted?.Invoke(arg1, arg2);
        }

        private void OnFeatureCompleted(IFeature arg1, ProcessingState arg2)
        {
            CurrentCompositions = Kernel.Compositions;
            FeatureCompleted?.Invoke(arg1, arg2);
        }

        private void OnFeatureStateUpdated(IFeature arg1, ProcessingState arg2)
        {
            FeatureStateUpdated?.Invoke(arg1, arg2);
        }
    }
}
