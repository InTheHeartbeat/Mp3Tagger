using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;
using Mp3Tagger.Models;
using TagLib.Mpeg;

namespace Mp3Tagger.Kernel.Features
{
    public class CompositionsLoader : IProcessingFeature
    {
        public string Name { get; set; }        
        public List<KeyValuePair<FileInfo, Exception>> BadFiles { get; set; }

        public IFeatureSettings Settings { get; private set; }        

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

        private List<FileInfo> files;
        public CompositionsLoader()
        {
            Name = "Compostions loading";
            BadFiles = new List<KeyValuePair<FileInfo, Exception>>();
        }

        public void Initialize(IFeatureSettings settings)
        {
            Settings = settings;
        }

        public void Initialize(IFeatureSettings settings,List<FileInfo> files)
        {
            this.files = files;
        }        

        public async Task ApplyToList(ObservableCollection<Composition> list, Action<FeatureProcessReport> progressUpdatedCallback)
        {
            if(files == null)
            {throw new ArgumentException("First you need to initialize path!");}

            FeatureProcessReport processReport = new FeatureProcessReport
            {               
                TotalOperations = files.Count
            };
            await Task.Run(() =>
            {
                for (var index = 0; index < files.Count; index++)
                {                    
                    try
                    {
                        Composition composition = new Composition(new AudioFile(files[index].FullName));
                        list.Add(composition);                        
                        processReport.PerformedOperations = index+1;
                        progressUpdatedCallback(processReport);
                    }
                    catch (Exception e)
                    {
                        BadFiles.Add(new KeyValuePair<FileInfo, Exception>(files[index], e));
                    }
                }
            });
            files.Clear();                        
        }
        
        public void ApplyToComposition(Composition composition)
        {return;            
        }        
    }
}
