﻿using System;
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
        Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback);
        void ApplyToComposition(Composition composition);        
    }
}
