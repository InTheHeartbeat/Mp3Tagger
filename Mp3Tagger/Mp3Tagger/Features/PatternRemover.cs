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

        public void ApplyToList(List<Composition> list, Action<IFeature, int, int> progressCallback, Action<IFeature> progressCompletedCallback)
        {
            foreach (Composition composition in list)
            {
                ApplyToComposition(composition);
            }
        }        

        public void ApplyToComposition(Composition composition)
        {
            foreach (PropertyInfo propertyInfo in composition.GetType().GetProperties())
            {
                PatternRemoverApplyTo removerApplyTo =
                    PatternRemoverSettings.ApplyToSettings.FirstOrDefault(s => s.FieldName == propertyInfo.Name);
                if (removerApplyTo != null && removerApplyTo.IsApply)
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


            /*
            if (patternRemoverSettings.ApplyToSettings.FirstOrDefault(s=>s.FieldName==""))
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Album); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Album); }
            }
            if (patternRemoverSettings.ApplyToComment)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Comment); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Comment); }
            }
            if (patternRemoverSettings.ApplyToConductor)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Conductor); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Conductor); }
            }
            if (patternRemoverSettings.ApplyToCopyright)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Copyright); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Copyright); }
            }
            if (patternRemoverSettings.ApplyToLyrics)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Lyrics); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Lyrics); }
            }
            if (patternRemoverSettings.ApplyToGrouping)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Grouping); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Grouping); }
            }
            if (patternRemoverSettings.ApplyToTitle)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Title); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Title); }
            }
            if (patternRemoverSettings.ApplyToPerformer)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { CleanStringByPatternList(composition.Performer); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { CleanStringByBrackets(composition.Performer); }
            }

            if (patternRemoverSettings.ApplyToAlbumArtists)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { composition.AlbumArtists.ToList().ForEach(CleanStringByPatternList); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { composition.AlbumArtists.ToList().ForEach(CleanStringByBrackets); }
            }
            if (patternRemoverSettings.ApplyToComposers)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { composition.Composers.ToList().ForEach(CleanStringByPatternList); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { composition.Composers.ToList().ForEach(CleanStringByBrackets); }
            }
            if (patternRemoverSettings.ApplyToGenres)
            {
                if (patternRemoverSettings.RemoveByPatternList)
                { composition.Genres.ToList().ForEach(CleanStringByPatternList); }
                if (patternRemoverSettings.RemoveByBracketsList)
                { composition.Genres.ToList().ForEach(CleanStringByBrackets); }
            }*/
        }

        private string CleanStringByPatternList(string data)
        {
            foreach (string pattern in PatternRemoverSettings.PatternList)
            {
                if (data.Contains(pattern))
                { data = data.Replace(pattern, ""); }
            }
            return data;
        }

        private string CleanStringByBrackets(string data)
        {
            foreach (string pattern in PatternRemoverSettings.BracketsList)
            {
                Regex regex = new Regex("\\" + pattern.First() + @"\w*\D*?" + "\\" + pattern.Last());
                //replaced += regex.Matches(data).Count;
                data = regex.Replace(data, "");
            }
            return data;
        }
    }
}
