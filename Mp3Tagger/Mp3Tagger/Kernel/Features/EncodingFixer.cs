using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;
using Mp3Tagger.Models;

namespace Mp3Tagger.Kernel.Features
{
    public class EncodingFixer : IProcessingFeature
    {
        public string Name { get; set; }
        public IFeatureSettings Settings { get; private set; }
                
        public EncodingFixer()
        {
            Name = "Encoding fixing";
        }

        public void Initialize(IFeatureSettings settings)
        {
            Settings = settings;
        }

        public async Task ApplyToList(ObservableCollection<Composition> list, Action<FeatureProcessReport> progressUpdatedCallback)
        {
            FeatureProcessReport progressReport = new FeatureProcessReport
            {
                TotalOperations = list.Count
            };

            //ToDo:01-05-2018:fonblaser: Consider removing intermediate await of synchronous operation
            await Task.Run(() =>
            {
                for (var index = 0; index < list.Count; index++)
                {
                    progressReport.PerformedOperations = index + 1;
                    ApplyToComposition(list[index]);
                    progressUpdatedCallback(progressReport);
                }
            });
        }

        public void ApplyToComposition(Composition composition)
        {
            composition.Album = ToUtf8(composition.Album);
            composition.Performer = ToUtf8(composition.Performer);
            composition.Title = ToUtf8(composition.Title);
            composition.Comment = ToUtf8(composition.Comment);
            composition.Lyrics = ToUtf8(composition.Lyrics);
            composition.Conductor = ToUtf8(composition.Conductor);
            composition.Copyright = ToUtf8(composition.Copyright);
            composition.Grouping = ToUtf8(composition.Grouping);
            composition.AlbumArtists = composition.AlbumArtists.Select(ToUtf8).ToArray();
            composition.Composers = composition.Composers.Select(ToUtf8).ToArray();
            composition.Genres = composition.Genres.Select(ToUtf8).ToArray();
        }

        private string ToUtf8(string unknown)
        {
            return new string(unknown.ToCharArray()
                .Select(x => ((x + 848) >= 'А' && (x + 848) <= 'ё') ? (char)(x + 848) : x)
                .ToArray());
        }        
    }
}
