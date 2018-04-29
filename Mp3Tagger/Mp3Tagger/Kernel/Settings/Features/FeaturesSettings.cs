using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Kernel.Settings.Features
{
    public class FeaturesSettings
    {
        public NormalizerSettings Normalizer { get; set; }
        public CompositionsLoaderSettings CompositionsLoader { get; set; }
        public FileSystemWalkerSettings FileSystemWalker { get; set; }
        public PatternRemoverSettings PatternRemover { get; set; }

        public FeaturesSettings()
        {
            
        }

        public void InitializeByDefault()
        {
            Normalizer = new NormalizerSettings();
            CompositionsLoader = new CompositionsLoaderSettings();
            FileSystemWalker = new FileSystemWalkerSettings();
            PatternRemover = new PatternRemoverSettings();

            Normalizer.InitializeByDefault();
            CompositionsLoader.InitializeByDefault();
            FileSystemWalker.InitializeByDefault();
            PatternRemover.InitializeByDefault();
        }
    }
}
