using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Processing;
using Mp3Tagger.Kernel.Settings;

namespace Mp3Tagger.Kernel.Features.IO
{
    public class FileSystemWalker : IIOFeature
    {                
        private readonly FileSystemWalkerSettings fileSystemWalkerSettings;

        public IFeatureSettings Settings { get; private set; }

        private FileSystemWalkerSettings settings => (FileSystemWalkerSettings) Settings;        

        public FileSystemWalker(FileSystemWalkerSettings settings)
        {            
            fileSystemWalkerSettings = settings;
        }

        public void Initialize(IFeatureSettings settings)
        {
            Settings = settings;
        }

        public async Task ApplyToList(List<FileInfo> list, Action<IIOFeature, ProcessingState> progressUpdatedCallback)
        {
            ProcessingState state = new ProcessingState
            {
                CurrentFeature = this,
                IsBusy = true                
            };
            await Task.Run(() => WalkDirectoryTree(new DirectoryInfo(settings.Root), list.Add,progressUpdatedCallback, state));            
        }

        private void WalkDirectoryTree(DirectoryInfo root, Action<FileInfo> searched, Action<IIOFeature, ProcessingState> progressUpdatedCallback, ProcessingState state)
        {
            System.IO.FileInfo[] files = null;
            try
            {
                files = root.GetFiles("*.mp3");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    if (fileSystemWalkerSettings.MinFileBytes <= 0)
                    { searched(file); }
                    else
                    {
                        if (file.Length >= fileSystemWalkerSettings.MinFileBytes)
                        {
                            searched(file);
                        }
                    }                    
                }
                
                var subDirs = root.GetDirectories();
                state.OperationsCount += subDirs.Length;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    state.OperationsPerformed++;
                    progressUpdatedCallback(this, state);
                    WalkDirectoryTree(dirInfo, searched, progressUpdatedCallback,state);
                }
            }
        }
    }
}
