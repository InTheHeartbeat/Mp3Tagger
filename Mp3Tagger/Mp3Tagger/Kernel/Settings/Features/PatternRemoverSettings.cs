using System;
using System.Collections.Generic;
using System.Linq;
using Mp3Tagger.Kernel.Base.Extensions;
using Mp3Tagger.Kernel.Features.Helpers;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Settings.Features
{
    [Serializable]
    public class PatternRemoverSettings : ISettings
    {
        public List<string> PatternList { get; set; }
        public List<string> BracketsList { get; set; }
        public List<FeatureApplyToField> ApplyToSettings { get; set; }
        public bool RemoveByPatternList { get; set; }
        public bool RemoveByBracketsList { get; set; }

        public PatternRemoverSettings()
        {                                                
        }

        public void InitializeByDefault()
        {
            InitializeApplyToByDefault();
            InitalizePatternListByDefauld();
            InitializeBracketsListByDefault();
            InitializeRemoveBoolsByDefault();
        }

        public void InitializeApplyToByDefault()
        {
            ApplyToSettings = new List<FeatureApplyToField>();
            ApplyToSettings.AddRange(typeof(Composition).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string) && prop.Name != "Bitrate" && prop.Name != "Path")
                .Select(pp => new FeatureApplyToField() { FieldName = pp.Name.Replace("Joined", "").AddSpaceBetwenUpperChars(), IsApply = true }));
        }

        public void InitalizePatternListByDefauld()
        {
            PatternList = new List<string> { "[pleer.net]", "ProstoPleer.com", "Zaycev.Net" };
        }

        public void InitializeBracketsListByDefault()
        {
            BracketsList = new List<string> { "[]", "()", "{}" };
        }

        public void InitializeRemoveBoolsByDefault()
        {
            RemoveByPatternList = true;
            RemoveByBracketsList = true;
        }
    }
}
