using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Enums;
using Mp3Tagger.Extensions;
using Mp3Tagger.Interfaces;
using Mp3Tagger.Models;
using Mp3Tagger.Settings;

namespace Mp3Tagger.Features
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
                if (Settings.CaseChangeMode == CaseChangeMode.AllWordsUpper)
                {
                    composition.Title = composition.Title.AllWordsUpper();
                    composition.Album = composition.Album.AllWordsUpper();
                    composition.Performer = composition.Performer.AllWordsUpper();
                }
                if (Settings.CaseChangeMode == CaseChangeMode.FirstWordUpper)
                {
                    composition.Title = composition.Title.FirstWordUpper();
                    composition.Album = composition.Album.FirstWordUpper();
                    composition.Performer = composition.Performer.FirstWordUpper();
                }
            }
            if (Settings.RemoveChars)
            {
                Settings.CharsToRemove.ForEach(ch =>
                {
                    composition.Title = composition.Title.Replace(ch.ToString(), string.Empty);
                    composition.Album = composition.Album.Replace(ch.ToString(), string.Empty);
                    composition.Performer = composition.Performer.Replace(ch.ToString(), string.Empty);
                });
            }
        }
    }
}
