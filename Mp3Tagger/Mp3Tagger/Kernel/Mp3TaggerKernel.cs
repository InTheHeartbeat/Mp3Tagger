using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features;
using Mp3Tagger.Kernel.Features.IO;
using Mp3Tagger.Kernel.History;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;
using Mp3Tagger.Kernel.Settings;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel
{
    public class Mp3TaggerKernel
    {
        public ObservableCollection<Composition> Compositions;
        
        public HistoryKeeper HistoryKeeper { get; private set; }
       
        public FeaturesKit Features { get; private set; }

        public ProcessingFeatureRunner ProcessingFeatureRunner { get; private set; }
        public IOFeatureRunner IoFeatureRunner { get; private set; }        

        public ProcessingState CurrentState { get; private set; }

        public event Action<ProcessingState> ProcessingStateChanged; 

        public Mp3TaggerKernel()
        {
           Initialize();
        }

        private void Initialize()
        {            
            HistoryKeeper = new HistoryKeeper();
            Features = new FeaturesKit();
            ProcessingFeatureRunner = new ProcessingFeatureRunner();
            IoFeatureRunner = new IOFeatureRunner();

            ProcessingFeatureRunner.ProcessingCompleted +=
                (state) =>
                {
                    HistoryKeeper.History.Add(
                        new HistoryEntry()
                        {
                            Elapsed = state.Elapsed,
                            UsedFeature = Features.GetFeatureEntryByFeature(state.CurrentFeature)
                        });
                    OnProcessingStateChanged(ProcessingFeatureRunner.CurrentState);
                };
            IoFeatureRunner.ProcessingCompleted +=
                (state) =>
                {
                    HistoryKeeper.History.Add(
                        new HistoryEntry()
                        {
                            Elapsed = state.Elapsed,
                            UsedFeature = Features.GetFeatureEntryByFeature(state.CurrentFeature)
                        });
                    OnProcessingStateChanged(IoFeatureRunner.CurrentState);
                };

            ProcessingFeatureRunner.ProcessingStateUpdated +=
                report => OnProcessingStateChanged(ProcessingFeatureRunner.CurrentState);
            IoFeatureRunner.ProcessingStateUpdated += report => OnProcessingStateChanged(IoFeatureRunner.CurrentState);
        }

        public async Task InitilizeCompositions(string path)
        {
            Compositions = new ObservableCollection<Composition>();

            List<FileInfo> searchedFiles = new List<FileInfo>();
            
            Features.SetFeatureSettings(FeatureName.FileSystemWalker, new FileSystemWalkerSettings() {Root = path});

            await IoFeatureRunner.PerformProcessorByList(searchedFiles,
                (IIOFeature) Features.GetFeatureEntryByName(FeatureName.FileSystemWalker).Feature);
                                                
            CompositionsLoader compositionsLoader = (CompositionsLoader)Features.GetFeatureEntryByName(FeatureName.CompositionLoader).Feature;
            compositionsLoader.Initialize(new CompositionsLoaderSettings(),searchedFiles);
            await ProcessingFeatureRunner.PerformProcessorByList(Compositions, compositionsLoader);                                    
        } 

        protected virtual void OnProcessingStateChanged(ProcessingState obj)
        {
            ProcessingStateChanged?.Invoke(obj);
        }
    }
}
