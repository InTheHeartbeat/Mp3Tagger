using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Interfaces;
using Mp3Tagger.Models;

namespace Mp3Tagger.Features
{
    public class EncodingFixer : IFeature
    {
        public string Name { get; set; }

        public EncodingFixer()
        {
            Name = "Encoding fixing";
        }

        public async void ApplyToList(List<Composition> list, Action<IFeature,int,int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback)
        {
            await Task.Run(() =>
            {
                for (var index = 0; index < list.Count; index++)
                {
                    ApplyToComposition(list[index]);
                    progressUpdatedCallback(this, index, list.Count);
                }
            });
            progressCompletedCallback(this);
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
            for (var i = 0; i < composition.AlbumArtists.Length; i++)
            {
                composition.AlbumArtists[i] = ToUtf8(composition.AlbumArtists[i]);
            }
            for (var i = 0; i < composition.Composers.Length; i++)
            {
                composition.Composers[i] = ToUtf8(composition.Composers[i]);
            }
            for (var i = 0; i < composition.Genres.Length; i++)
            {
                composition.Genres[i] = ToUtf8(composition.Genres[i]);
            }            
        }

        private string ToUtf8(string unknown)
        {
            return new string(unknown.ToCharArray()
                .Select(x => ((x + 848) >= 'А' && (x + 848) <= 'ё') ? (char)(x + 848) : x)
                .ToArray());
        }
    }
}
