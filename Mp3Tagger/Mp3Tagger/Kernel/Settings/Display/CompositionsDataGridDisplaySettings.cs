using System;
using System.Collections.Generic;
using System.Linq;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings.Display
{
    [Serializable]
    public class CompositionsDataGridDisplaySettings : ISettings
    {
        public List<CompositionFieldColumnVisible> CompositionsGridColumnVisibility { get; set; }

        public CompositionsDataGridDisplaySettings()
        {}

        public CompositionsDataGridDisplaySettings(List<CompositionFieldColumnVisible> compositionsGridColumnVisibility)
        {
            CompositionsGridColumnVisibility = compositionsGridColumnVisibility;
        }

        public void InitializeByDefault()
        {
            CompositionsGridColumnVisibility = new List<CompositionFieldColumnVisible>();
            foreach (CompositionField field in Enum.GetValues(typeof(CompositionField)))
            {
                CompositionsGridColumnVisibility.Add(new CompositionFieldColumnVisible(field, false));
            }

            CompositionsGridColumnVisibility.FirstOrDefault(enty=>enty.Field == CompositionField.Title).Visible = true;
            CompositionsGridColumnVisibility.FirstOrDefault(enty => enty.Field == CompositionField.Artist).Visible = true;
            CompositionsGridColumnVisibility.FirstOrDefault(enty => enty.Field == CompositionField.Album).Visible = true;
            CompositionsGridColumnVisibility.FirstOrDefault(enty => enty.Field == CompositionField.Genres).Visible = true;
            CompositionsGridColumnVisibility.FirstOrDefault(enty => enty.Field == CompositionField.Bitrate).Visible = true;            
        }
    }
}
