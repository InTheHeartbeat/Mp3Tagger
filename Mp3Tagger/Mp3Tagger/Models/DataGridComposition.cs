﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;
using TagLib;
using TagLib.Mpeg;

namespace Mp3Tagger.Models
{
    public class DataGridComposition : INotifyPropertyChanged
    {
        private string title;
        private string performer;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title");}
        }

        public string Performer
        {
            get { return performer; }
            set { performer = value; OnPropertyChanged("Performer"); }
        }

        public string Album { get; set; }
        public int Bitrate { get; set; }
        public string Duration { get; set; }
        public int Year { get; set; }
        public string Path { get; set; }
        public string AlbumArtists { get; set; }
        public string AmazonId { get; set; }
        public string Comment { get; set; }
        public string Composers { get; set; }
        public string Conductor { get; set; }
        public string Copyright { get; set; }        
        public int Disc { get; set; }
        public int DiscCount { get; set; }
        public string Genres { get; set; }
        public string Grouping { get; set; }        
        public int Track { get; set; }
        public int TrackCount { get; set; }                

        public DataGridComposition(Composition baseComposition)
        {
            Path = baseComposition.Path;
            Title = baseComposition.Title ?? String.Empty;
            Album = baseComposition.Album ?? String.Empty;
            AlbumArtists = string.Join(", ",baseComposition.AlbumArtists);
            AmazonId = baseComposition.AmazonId ?? String.Empty;
            Comment = baseComposition.Comment ?? String.Empty;
            Composers = string.Join(", ",baseComposition.Composers);
            Conductor = baseComposition.Conductor ?? String.Empty;
            Copyright = baseComposition.Copyright ?? String.Empty;
            Disc = (int)baseComposition.Disc;
            DiscCount = (int)baseComposition.DiscCount;
            Genres = string.Join(", ", baseComposition.Genres);
            Grouping = baseComposition.Grouping ?? String.Empty;            
            Performer = string.Join(", ",baseComposition.Performer ?? String.Empty);                     
            Track = (int)baseComposition.Track;
            TrackCount = (int)baseComposition.TrackCount;
            Year = (int)baseComposition.Year;
            Bitrate = baseComposition.Bitrate;
            Duration = baseComposition.Duration.Minutes + ":" + baseComposition.Duration.Seconds;
        }

        public DataGridComposition()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
