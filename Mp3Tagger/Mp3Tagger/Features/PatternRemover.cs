using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mp3Tagger.Features.Helpers;
using Mp3Tagger.Interfaces;
using Mp3Tagger.Models;
using Mp3Tagger.Settings;

namespace Mp3Tagger.Features
{
    public class PatternRemover : IFeature
    {
        public string Name { get; set; }

        public PatternRemoverSettings PatternRemoverSettings { get; set; }      

        public PatternRemover()
        {
            Name = "Pattern removing";
        }

        public void Initialize(PatternRemoverSettings settings)
        {
            PatternRemoverSettings = settings;
        }

        public async Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback)
        {
            await Task.Run(() =>
            {
                for (var i = 0; i < list.Count; i++)
                {
                    ApplyToComposition(list[i]);
                    progressUpdatedCallback(this, i, list.Count);
                }
            });
            progressCompletedCallback(this);
        }        

        public void ApplyToComposition(Composition composition)
        {            
            foreach (PropertyInfo propertyInfo in composition.GetType().GetProperties().Where(p=>p.PropertyType == typeof(string)))
            {
                FeatureApplyToField removerApplyToField =
                    PatternRemoverSettings.ApplyToSettings.FirstOrDefault(s => s.FieldName.Trim().ToLower() == propertyInfo.Name.Trim().ToLower());
                if (removerApplyToField != null && removerApplyToField.IsApply)
                {
                    if (PatternRemoverSettings.RemoveByPatternList)
                    {
                        PropertyInfo property = composition.GetType()
                            .GetProperty(propertyInfo.Name);
                        if (property != null)
                            property
                                .SetValue(composition,
                                    CleanStringByPatternList((string) property
                                        .GetValue(composition)));
                    }
                    if (PatternRemoverSettings.RemoveByBracketsList)
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
                foreach (string pattern in PatternRemoverSettings.PatternList)
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
                foreach (string pattern in PatternRemoverSettings.BracketsList)
                {
                    Regex regex = new Regex("\\" + pattern.First() + @"(.*?)" + "\\" + pattern.Last());
                    data = regex.Replace(data, "");
                }
            }
            return data;
        }
    }
}
