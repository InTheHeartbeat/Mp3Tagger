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
        public List<PatternRemoverApplyTo> ApplyToSettings { get; set; }
        public bool RemoveByPatternList { get; set; }
        public bool RemoveByBracketsList { get; set; }

        /*public bool ApplyToTitle { get; set; }
        public bool ApplyToPerformer { get; set; }
        public bool ApplyToAlbum {get;set;}
        public bool ApplyToAlbumArtists { get; set; }
        public bool ApplyToComposers { get; set; }
        public bool ApplyToConductor { get; set; }
        public bool ApplyToCopyright { get; set; }
        public bool ApplyToGenres { get; set; }
        public bool ApplyToGrouping { get; set; }
        public bool ApplyToLyrics { get; set; }
        public bool ApplyToComment { get; set; }   */

        public PatternRemoverSettings()
        {
            ApplyToSettings = new List<PatternRemoverApplyTo>();
            ApplyToSettings.AddRange(typeof(Composition).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string) && prop.Name != "Bitrate" && prop.Name != "Path")
                .Select(pp => new PatternRemoverApplyTo() {FieldName = pp.Name.Replace("Joined","").AddSpaceBetwenUpperChars(), IsApply = false}));
        }  
    }
}
