using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.IO
{
    public class FilesProvider
    {
        public static async Task<List<string>> GetFiles(string path, bool subDirs)
        {
            return await Task.Run(() => Directory.GetFiles(path, "*.mp3", subDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .ToList());                      
        }
    }
}
