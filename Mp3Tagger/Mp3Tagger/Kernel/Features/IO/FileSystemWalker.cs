using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Features.IO
{
    public class FileSystemWalker : IIOFeature
    {
        public string Name { get; set; }
        public DirectoryInfo Root { get; set; }

        private static int _totalCount;
        private static int _totalPerformed;

        public FileSystemWalker()
        {
            Name = "Files searching";
        }

        public void Initialize(string root)
        {
            _totalCount = 0;
            _totalPerformed = 0;
            Root = new DirectoryInfo(root);
        }

        public Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback)
        {return null;}

        public void ApplyToComposition(Composition composition)
        {return;}

        public async Task ApplyToList(List<string> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback)
        {
            await Task.Run(() => WalkDirectoryTree(Root, list.Add,progressUpdatedCallback));
        }

        private void WalkDirectoryTree(DirectoryInfo root, Action<string> searched, Action<IFeature, int, int> progressUpdatedCallback)
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
                    searched(file.FullName);
                }
                var subDirs = root.GetDirectories();
                _totalCount += subDirs.Length;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    progressUpdatedCallback(this, ++_totalPerformed, _totalCount);
                    WalkDirectoryTree(dirInfo, searched, progressUpdatedCallback);
                }
            }
        }
    }
}
