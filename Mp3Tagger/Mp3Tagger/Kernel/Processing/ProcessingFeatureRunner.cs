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
        public event Action<ProcessingState> ProcessingStarted;
        public event Action<ProcessingState> ProcessingCompleted;

        public event Action<FeatureProcessReport> ProcessingStateUpdated;

        public ProcessingState CurrentState { get; private set; }

        private Stopwatch _elapsed;

        public ProcessingFeatureRunner()
        {}

        public async Task PerformProcessorByList(ObservableCollection<Composition> list, IProcessingFeature feature)
        {            
            OnProcessingStarted(CurrentState = new ProcessingState()
            {
                CurrentFeature = feature,
                IsBusy = true,
                OperationsCount = list.Count,
                PerformedOperations = 0
            });

            await feature.ApplyToList(list, OnProcessingStateUpdated);

            OnProcessingCompleted(CurrentState);
        }

        protected virtual void OnProcessingStarted(ProcessingState state)
        {
            _elapsed = Stopwatch.StartNew();
            state.Elapsed = _elapsed.Elapsed;
            CurrentState = state;            
            ProcessingStarted?.Invoke(state);            
        }

        protected virtual void OnProcessingCompleted(ProcessingState state)
        {
            state.Elapsed = _elapsed.Elapsed;
            state.IsBusy = false;
            CurrentState = state;
            _elapsed.Stop();
            ProcessingCompleted?.Invoke(state);                    
        }

        protected virtual void OnProcessingStateUpdated(FeatureProcessReport processReport)
        {
            CurrentState.Elapsed = _elapsed.Elapsed;
            CurrentState.PerformedOperations = processReport.PerformedOperations;
            CurrentState.OperationsCount = processReport.TotalOperations;
            ProcessingStateUpdated?.Invoke(processReport);            
        }
    }
}
