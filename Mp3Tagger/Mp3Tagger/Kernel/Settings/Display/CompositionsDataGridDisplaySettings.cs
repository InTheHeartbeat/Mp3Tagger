using System;
using System.Collections.Generic;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings.Display
{
    [Serializable]
    public class CompositionsDataGridDisplaySettings : ISettings
    {
        public Dictionary<CompositionField, bool> CompositionsGridViewColumnVisibility { get; set; }

        public CompositionsDataGridDisplaySettings()
        {}

        public CompositionsDataGridDisplaySettings(Dictionary<CompositionField, bool> compositionsGridViewColumnVisibility)
        {
            CompositionsGridViewColumnVisibility = compositionsGridViewColumnVisibility;
        }

        public void InitializeByDefault()
        {
            CompositionsGridViewColumnVisibility = new Dictionary<CompositionField, bool>();
            foreach (CompositionField field in Enum.GetValues(typeof(CompositionField)))
            {
                CompositionsGridViewColumnVisibility.Add(field, false);
            }

            CompositionsGridViewColumnVisibility[CompositionField.Title] = true;
            CompositionsGridViewColumnVisibility[CompositionField.Artist] = true;
            CompositionsGridViewColumnVisibility[CompositionField.Album] = true;
            CompositionsGridViewColumnVisibility[CompositionField.Genres] = true;
            CompositionsGridViewColumnVisibility[CompositionField.Bitrate] = true;            
        }
    }
}
