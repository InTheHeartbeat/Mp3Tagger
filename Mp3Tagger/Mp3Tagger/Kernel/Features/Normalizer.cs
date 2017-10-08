using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Base.Extensions;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features.Helpers;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Settings;
using Mp3Tagger.Models;

namespace Mp3Tagger.Kernel.Features
{
    public class Normalizer : IFeature
    {
        public string Name { get; set; }

        public NormalizerSettings Settings { get; set; }

        public Normalizer()
        {
            Name = "Normalizing";
        }

        public void Initialize(NormalizerSettings settings)
        {
            Settings = settings;
        }

        public async Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback,
            Action<IFeature> progressCompletedCallback)
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
            if (Settings == null) throw new ArgumentException("Initialize required!");

            if (composition == null) return;

            if (Settings.Trimming)
            {
                composition.Title = composition.Title.Trim();
                composition.Album = composition.Album.Trim();
                composition.Performer = composition.Performer.Trim();
            }
            if (Settings.ChangeCase)
            {
                foreach (PropertyInfo propertyInfo in composition.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(string)))
                {
                    FeatureApplyToField removerApplyToField =
                        Settings.CaseChangeApplyTo.FirstOrDefault(
                            s => s.FieldName.Trim().ToLower() == propertyInfo.Name.Trim().ToLower());
                    if (removerApplyToField != null && removerApplyToField.IsApply)
                    {
                        if (Settings.CaseChangeMode == CaseChangeMode.AllWordsUpper)
                        {
                            PropertyInfo property = composition.GetType()
                                .GetProperty(propertyInfo.Name);
                            if (property != null)
                                property
                                    .SetValue(composition,
                                        ((string) property
                                            .GetValue(composition)).AllWordsUpper());
                        }
                        if (Settings.CaseChangeMode == CaseChangeMode.FirstWordUpper)
                        {
                            PropertyInfo property = composition.GetType()
                                .GetProperty(propertyInfo.Name);
                            if (property != null)
                                property
                                    .SetValue(composition,
                                        ((string) property
                                            .GetValue(composition)).FirstWordUpper());
                        }
                    }
                }
            }
            if (Settings.RemoveChars)
            {
                Settings.CharsToRemove.ForEach(ch =>
                {
                    foreach (PropertyInfo propertyInfo in composition.GetType()
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(string)))
                    {
                        FeatureApplyToField removerApplyToField =
                            Settings.CaseChangeApplyTo.FirstOrDefault(
                                s => s.FieldName.Trim().ToLower() == propertyInfo.Name.Trim().ToLower());
                        if (removerApplyToField != null && removerApplyToField.IsApply)
                        {
                            PropertyInfo property = composition.GetType()
                                .GetProperty(propertyInfo.Name);
                            if (property != null)
                                property
                                    .SetValue(composition,
                                        ((string) property
                                            .GetValue(composition)).Replace(ch.ToString(), string.Empty));
                        }
                    }
                });
            }
        }
    }
}
