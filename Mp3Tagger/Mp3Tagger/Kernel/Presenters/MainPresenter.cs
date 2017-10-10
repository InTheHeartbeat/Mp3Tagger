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
        public ObservableCollection<Composition> CurrentCompositions => Kernel.Compositions;

        public event Action<ProcessingState> FeatureStarted;
        public event Action<ProcessingState> FeatureCompleted;

        public Mp3TaggerKernel Kernel { get; set; }

        public MainPresenter(IView view)
        {            
            Initialize();
        }

        private void Initialize()
        {            
            Kernel = new Mp3TaggerKernel();                        

            Kernel.ProcessingFeatureRunner.ProcessingStarted += OnFeatureStarted;            
            Kernel.ProcessingFeatureRunner.ProcessingCompleted += OnFeatureCompleted;

            Kernel.IoFeatureRunner.ProcessingStarted += OnFeatureStarted;            
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

        private void OnFeatureStarted(ProcessingState obj)
        {
            FeatureStarted?.Invoke(obj);
        }

        private void OnFeatureCompleted(ProcessingState obj)
        {
            FeatureCompleted?.Invoke(obj);
        }
    }
}
