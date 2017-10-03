using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Models;

namespace Mp3Tagger.Interfaces
{
    public interface IFeature
    {
        string Name { get; set; }
        void ApplyToList(List<Composition> list, Action<IFeature,int, int> progressCallback, Action<IFeature> progressCompletedCallback);
        void ApplyToComposition(Composition composition);        
    }
}
