using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Kernel.Interfaces
{
    interface IIOFeature : IFeature
    {
        new Task ApplyToList(List<string> list, Action<IFeature, int, int> progressUpdatedCallback,
            Action<IFeature> progressCompletedCallback);
    }
}
