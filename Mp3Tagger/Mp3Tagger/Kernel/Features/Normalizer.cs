using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Base.Extensions;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Features.Helpers;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Settings.Features;


namespace Mp3Tagger.Kernel.Features
{
    public class Normalizer : IProcessingFeature
    {
        public string Name { get; set; }

        public ISettings Settings { get; set; }

        private NormalizerSettings settings => (NormalizerSettings) Settings;

        public Normalizer()
        {
            Name = "Normalizing";
            Initialize(new NormalizerSettings());
        }

        public void Initialize(ISettings settings)
        {
            Settings = settings;
        }

        public void Initialize(FeaturesSettings currentSettingsFeatures)
        {
            Initialize(currentSettingsFeatures.Normalizer);
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
                    processReport.PerformedOperations = i + 1;
                    progressUpdatedCallback(processReport);
                }
            });            
        }

        public void ApplyToComposition(Composition composition)
        {
            if (settings == null) throw new ArgumentException("Initialize required!");

            if (composition == null) return;

            if (settings.Trimming)
            {
                composition.Title = composition.Title.Trim();
                composition.Album = composition.Album.Trim();
                composition.Artist = composition.Artist.Trim();
            }
            if (settings.ChangeCase)
            {
                foreach (PropertyInfo propertyInfo in composition.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(string)))
                {
                    FeatureApplyToField removerApplyToField =
                        settings.CaseChangeApplyTo.FirstOrDefault(
                            s => s.FieldName.Trim().ToLower() == propertyInfo.Name.Trim().ToLower());
                    if (removerApplyToField != null && removerApplyToField.IsApply)
                    {
                        if (settings.CaseChangeMode == CaseChangeMode.AllWordsUpper)
                        {
                            PropertyInfo property = composition.GetType()
                                .GetProperty(propertyInfo.Name);
                            if (property != null)
                                property
                                    .SetValue(composition,
                                        ((string) property
                                            .GetValue(composition)).AllWordsUpper());
                        }
                        if (settings.CaseChangeMode == CaseChangeMode.FirstWordUpper)
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
            if (settings.RemoveChars)
            {
                settings.CharsToRemove.ForEach(ch =>
                {
                    foreach (PropertyInfo propertyInfo in composition.GetType()
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(string)))
                    {
                        FeatureApplyToField removerApplyToField =
                            settings.CaseChangeApplyTo.FirstOrDefault(
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
