using System;
using Mp3Tagger.Kernel.Base.Attributes;
using TagLib;
using TagLib.Mpeg;

namespace Mp3Tagger.Kernel.Models
{
    public class Composition
    {
        public string Title { get; set; }
        public string Performer { get; set; }
        public string Album { get; set; }
        public int Bitrate { get; set; }
        public TimeSpan Duration { get; set; }
        public int Year { get; set; }
        public string Path { get; set; }
        public string[] AlbumArtists { get; set; }

        public string JoinedAlbumArtists
        {
            get { return string.Join(", ", AlbumArtists); }
            set { AlbumArtists = value.Split(','); }
        }

        public string AmazonId { get; set; }
        [CustomFieldHeightRequired(150)]
        public string Comment { get; set; }
        public string[] Composers { get; set; }

        public string JoinedComposers
        {
            get { return string.Join(", ", Composers); }
            set { Composers = value.Split(','); }
        }
        public string Conductor { get; set; }
        [CustomFieldHeightRequired(150)]
        public string Copyright { get; set; }        
        public int Disc { get; set; }        
        public int DiscCount { get; set; }
        public string[] Genres { get; set; }

        public string JoinedGenres
        {
            get { return string.Join(", ", Genres); }
            set { Genres = value.Split(','); }
        }
        public string Grouping { get; set; }
        [CustomFieldHeightRequired(250)]
        public string Lyrics { get; set; }
        public int Track { get; set; }
        public int TrackCount { get; set; }
        public IPicture[] Pictures { get; set; }


        public Composition(AudioFile audioFile)
        {
            Path = audioFile.Name;
            Title = audioFile.Tag.Title ?? String.Empty;
            Album = audioFile.Tag.Album ?? String.Empty;
            AlbumArtists = audioFile.Tag.AlbumArtists;
            AmazonId = audioFile.Tag.AmazonId ?? String.Empty;
            Comment = audioFile.Tag.Comment ?? String.Empty;
            Composers = audioFile.Tag.Composers;
            Conductor = audioFile.Tag.Conductor ?? String.Empty;
            Copyright = audioFile.Tag.Copyright ?? String.Empty;
            Disc = (int)audioFile.Tag.Disc;
            DiscCount = (int)audioFile.Tag.DiscCount;
            Genres = audioFile.Tag.Genres;
            Grouping = audioFile.Tag.Grouping ?? String.Empty;
            Lyrics = audioFile.Tag.Lyrics ?? String.Empty;
            Performer = audioFile.Tag.JoinedPerformers ?? String.Empty;
            Pictures = audioFile.Tag.Pictures;
            Track = (int)audioFile.Tag.Track;
            TrackCount = (int)audioFile.Tag.TrackCount;
            Year = (int)audioFile.Tag.Year;
            Bitrate = audioFile.Properties.AudioBitrate;
            Duration = audioFile.Properties.Duration;
        }
    }
}
