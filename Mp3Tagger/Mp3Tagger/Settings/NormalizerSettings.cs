using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Enums;
using Mp3Tagger.Extensions;
using Mp3Tagger.Features.Helpers;
using Mp3Tagger.Models;

namespace Mp3Tagger.Settings
{
    public class NormalizerSettings
    {
        public bool Trimming { get; set; }
        public bool ChangeCase { get; set; }
        public bool RemoveChars { get; set; }
        public List<string> CharsToRemove { get; set; }
        public CaseChangeMode CaseChangeMode { get; set; }

        public List<FeatureApplyToField> CaseChangeApplyTo { get; set; }
        public List<FeatureApplyToField> CharsToRemoveApplyTo { get; set; }

        public NormalizerSettings()
        {
            CharsToRemove = new List<string>();
            Trimming = true;
            CaseChangeMode = CaseChangeMode.AllWordsUpper;

            InitializeCaseChangeApplyToByDefault();            
            InitializeCharsToRemoveApplyToByDeafult();            
        }

        public void InitializeCaseChangeApplyToByDefault()
        {
            CaseChangeApplyTo = new List<FeatureApplyToField>();
            CaseChangeApplyTo.AddRange(GetFields());
        }

        public void InitializeCharsToRemoveApplyToByDeafult()
        {
            CharsToRemoveApplyTo = new List<FeatureApplyToField>();
            CharsToRemoveApplyTo.AddRange(GetFields());
        }

        private List<FeatureApplyToField> GetFields()
        {
            return typeof(Composition).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string) && prop.Name != "Bitrate" && prop.Name != "Path")
                .Select(pp => new FeatureApplyToField()
                {
                    FieldName = pp.Name.Replace("Joined", "").AddSpaceBetwenUpperChars(),
                    IsApply = false
                }).ToList();
        }
    }
}
