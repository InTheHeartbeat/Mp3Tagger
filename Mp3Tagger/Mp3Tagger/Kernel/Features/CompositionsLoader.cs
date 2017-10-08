﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Models;
using TagLib.Mpeg;

namespace Mp3Tagger.Kernel.Features
{
    public class CompositionsLoader : IFeature
    {
        public string Name { get; set; }        
        public List<KeyValuePair<FileInfo, Exception>> BadFiles { get; set; }

        public int FilesCount
        {
            get
            {
                if (files == null)
                {
                    throw new ArgumentException("First you need to initialize path!");
                }
                return files.Count;
            }
        }

        private List<string> files;
        public CompositionsLoader()
        {
            Name = "Compostions loading";
            BadFiles = new List<KeyValuePair<FileInfo, Exception>>();
        }

        public void Initialize(List<string> files)
        {
            this.files = files;
        }        

        public async Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback)
        {
            if(files == null)
            {throw new ArgumentException("First you need to initialize path!");}
            
            await Task.Run(() =>
            {
                for (var index = 0; index < files.Count; index++)
                {
                    string compositionPath = files[index];
                    try
                    {
                        Composition composition = new Composition(new AudioFile(compositionPath));
                        list.Add(composition);
                        progressUpdatedCallback(this,index, files.Count);
                    }
                    catch (Exception e)
                    {
                        BadFiles.Add(new KeyValuePair<FileInfo, Exception>(new FileInfo(compositionPath), e));
                    }
                }
            });
            files.Clear();            
            progressCompletedCallback(this);
        }

        public void ApplyToComposition(Composition composition)
        {return;            
        }
    }
}