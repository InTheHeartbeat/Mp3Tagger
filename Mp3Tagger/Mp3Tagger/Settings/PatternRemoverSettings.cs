using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Extensions;
using Mp3Tagger.Features.Helpers;
using Mp3Tagger.Models;

namespace Mp3Tagger.Settings
{
    public class PatternRemoverSettings
    {
        public List<string> PatternList { get; set; }
        public List<string> BracketsList { get; set; }
        public List<FeatureApplyToField> ApplyToSettings { get; set; }
        public bool RemoveByPatternList { get; set; }
        public bool RemoveByBracketsList { get; set; }

        public PatternRemoverSettings()
        {
            PatternList = new List<string> {"[pleer.net]", "ProstoPleer.com", "Zaycev.Net"};
            BracketsList = new List<string> {"[]", "()", "{}"};
            RemoveByPatternList = true;
            RemoveByBracketsList = true;

            InitializeApplyToByDefault();
        }

        public void InitializeApplyToByDefault()
        {
            ApplyToSettings = new List<FeatureApplyToField>();
            ApplyToSettings.AddRange(typeof(Composition).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string) && prop.Name != "Bitrate" && prop.Name != "Path")
                .Select(pp => new FeatureApplyToField() { FieldName = pp.Name.Replace("Joined", "").AddSpaceBetwenUpperChars(), IsApply = true }));
        }
    }
}
