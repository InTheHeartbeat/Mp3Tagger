using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Processing
{
    public class ProcessingFeatureRunner
    {
        public event Action<IProcessingFeature, ProcessingState> ProcessingStarted;
        public event Action<IProcessingFeature, ProcessingState> ProcessingCompleted;
        public event Action<IProcessingFeature, ProcessingState> ProcessingStateUpdated;

        public ProcessingState CurrentState { get; private set; }

        private Stopwatch _elapsed;

        public ProcessingFeatureRunner()
        {}

        public async Task PerformProcessorByList(ObservableCollection<Composition> list, IProcessingFeature feature)
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

        protected virtual void OnProcessingStarted(IProcessingFeature processor, ProcessingState state)
        {
            _elapsed = Stopwatch.StartNew();
            state.Elapsed = _elapsed.Elapsed;
            CurrentState = state;            
            ProcessingStarted?.Invoke(processor, state);            
        }

        protected virtual void OnProcessingCompleted(IProcessingFeature processor, ProcessingState state)
        {
            state.Elapsed = _elapsed.Elapsed;
            state.IsBusy = false;
            CurrentState = state;
            _elapsed.Stop();
            ProcessingCompleted?.Invoke(processor,state);                    
        }

        protected virtual void OnProcessingStateUpdated(IProcessingFeature processor, ProcessingState state)
        {
            state.Elapsed = _elapsed.Elapsed;
            CurrentState = state;
            ProcessingStateUpdated?.Invoke(processor, state);            
        }
    }
}
