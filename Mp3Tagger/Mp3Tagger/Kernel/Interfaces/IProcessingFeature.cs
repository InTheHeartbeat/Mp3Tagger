using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;

namespace Mp3Tagger.Kernel.Interfaces
{
    public interface IProcessingFeature : IFeature
    {
        Task ApplyToList(ObservableCollection<Composition> list, Action<IProcessingFeature, ProcessingState> progressUpdatedCallback);
        void ApplyToComposition(Composition composition);
    }
}
