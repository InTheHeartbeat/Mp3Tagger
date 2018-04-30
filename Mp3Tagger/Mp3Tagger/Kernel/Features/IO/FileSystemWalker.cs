using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Kernel.Features.IO
{
    public class FileSystemWalker : IIOFeature
    {                
        private readonly FileSystemWalkerSettings fileSystemWalkerSettings;

        public string Name { get; set; }
        public ISettings Settings { get; private set; }

        private FileSystemWalkerSettings settings => (FileSystemWalkerSettings) Settings;        

        public FileSystemWalker(FileSystemWalkerSettings settings)
        {            
            fileSystemWalkerSettings = settings;
            Name = "File system walker";
        }

        public void Initialize(ISettings settings)
        {
            Settings = settings;
        }

        public void Initialize(FeaturesSettings currentSettingsFeatures)
        {
            Initialize(currentSettingsFeatures.FileSystemWalker);
        }

        public async Task ApplyToList(List<FileInfo> list, Action<FeatureProcessReport> progressUpdatedCallback)
        {
            FeatureProcessReport processReport = new FeatureProcessReport();
            await Task.Run(() => settings.Roots.ToList().ForEach(root=>WalkDirectoryTree(new DirectoryInfo(root), list.Add, progressUpdatedCallback, processReport)));            
        }

        private void WalkDirectoryTree(DirectoryInfo root, Action<FileInfo> searched, Action<FeatureProcessReport> progressUpdatedCallback, FeatureProcessReport processReport)
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
                processReport.TotalOperations += subDirs.Length;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    processReport.PerformedOperations++;
                    progressUpdatedCallback(processReport);
                    WalkDirectoryTree(dirInfo, searched, progressUpdatedCallback,processReport);
                }
            }
        }
    }
}
