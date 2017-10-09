using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Processing;

namespace Mp3Tagger.Kernel.Interfaces
{
    public interface IIOFeature : IFeature
    {
        Task ApplyToList(List<FileInfo> list, Action<IIOFeature, ProcessingState> progressUpdatedCallback);
    }
}
