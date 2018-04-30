using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Features.Helpers;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;
using Mp3Tagger.Kernel.Settings;
using Mp3Tagger.Kernel.Settings.Features;
using Mp3Tagger.Models;

namespace Mp3Tagger.Kernel.Features
{
    public class PatternRemover : IProcessingFeature
    {
        public string Name { get; set; }
        public ISettings Settings { get; private set; }

        private PatternRemoverSettings settings => (PatternRemoverSettings) Settings;

        public PatternRemover()
        {            
            Initialize(new PatternRemoverSettings());
            Name = "Pattern remover";
        }

        public void Initialize(ISettings settings)
        {
            Settings = settings;
        }

        public void Initialize(FeaturesSettings currentSettingsFeatures)
        {
            Initialize(currentSettingsFeatures.PatternRemover);
        }

        public async Task ApplyToList(ObservableCollection<Composition> list, Action<FeatureProcessReport> progressUpdatedCallback)
        {
            FeatureProcessReport processReport = new FeatureProcessReport
            {
                TotalOperations = list.Count
            };
            await Task.Run(() =>
            {
                for (var i = 0; i < list.Count; i++)
                {
                    ApplyToComposition(list[i]);
                    progressUpdatedCallback(processReport);
                }
            });            
        }

        public void ApplyToComposition(Composition composition)
        {            
            foreach (PropertyInfo propertyInfo in composition.GetType().GetProperties().Where(p=>p.PropertyType == typeof(string)))
            {
                FeatureApplyToField removerApplyToField =
                    settings.ApplyToSettings.FirstOrDefault(s => s.FieldName.Trim().ToLower() == propertyInfo.Name.Trim().ToLower());
                if (removerApplyToField != null && removerApplyToField.IsApply)
                {
                    if (settings.RemoveByPatternList)
                    {
                        PropertyInfo property = composition.GetType()
                            .GetProperty(propertyInfo.Name);
                        if (property != null)
                            property
                                .SetValue(composition,
                                    CleanStringByPatternList((string) property
                                        .GetValue(composition)));
                    }
                    if (settings.RemoveByBracketsList)
                    {
                        PropertyInfo property = composition.GetType()
                            .GetProperty(propertyInfo.Name);
                        if (property != null)
                            property
                                .SetValue(composition,
                                    CleanStringByBrackets((string)property
                                        .GetValue(composition)));
                    }
                }
            }                      
        }

        private string CleanStringByPatternList(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                foreach (string pattern in settings.PatternList)
                {
                    if (data.Contains(pattern))
                    {
                        data = data.Replace(pattern, "");
                    }
                }
            }
            return data;
        }

        private string CleanStringByBrackets(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                foreach (string pattern in settings.BracketsList)
                {
                    Regex regex = new Regex("\\" + pattern.First() + @"(.*?)" + "\\" + pattern.Last());
                    data = regex.Replace(data, "");
                }
            }
            return data;
        }        
    }
}
