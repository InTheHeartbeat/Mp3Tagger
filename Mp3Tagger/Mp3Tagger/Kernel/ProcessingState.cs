using System;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel
{
    public class ProcessingState
    {
        public bool IsBusy { get; set; }
        public int OperationsCount { get; set; }
        public int PerformedOperations { get; set; }
        public IFeature CurrentFeature { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
