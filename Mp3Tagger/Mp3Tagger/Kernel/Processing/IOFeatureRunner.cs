using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Processing
{
    public class IOFeatureRunner
    {
        public event Action<IIOFeature, ProcessingState> ProcessingStarted;
        public event Action<IIOFeature, ProcessingState> ProcessingCompleted;
        public event Action<IIOFeature, ProcessingState> ProcessingStateUpdated;

        public ProcessingState CurrentState { get; private set; }

        private Stopwatch _elapsed;

        public IOFeatureRunner()
        { }

        public async Task PerformProcessorByList(List<FileInfo> list, IIOFeature feature)
        {
            OnProcessingStarted(feature, CurrentState = new ProcessingState()
            {
                CurrentFeature = feature,
                IsBusy = true,
                OperationsCount = list.Count,
                OperationsPerformed = 0
            });

            await feature.ApplyToList(list, OnProcessingStateUpdated);

            OnProcessingCompleted(feature, CurrentState);
        }

        protected virtual void OnProcessingStarted(IIOFeature processor, ProcessingState state)
        {
            _elapsed = Stopwatch.StartNew();
            state.Elapsed = _elapsed.Elapsed;
            CurrentState = state;            
            ProcessingStarted?.Invoke(processor, state);            
        }

        protected virtual void OnProcessingCompleted(IIOFeature processor, ProcessingState state)
        {
            state.Elapsed = _elapsed.Elapsed;
            state.IsBusy = false;
            CurrentState = state;
            _elapsed.Stop();
            ProcessingCompleted?.Invoke(processor, state);                       
        }

        protected virtual void OnProcessingStateUpdated(IIOFeature processor, ProcessingState state)
        {
            state.Elapsed = _elapsed.Elapsed;
            CurrentState = state;
            ProcessingStateUpdated?.Invoke(processor, state);            
        }
    }
}
