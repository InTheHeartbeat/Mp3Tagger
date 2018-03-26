using Mp3Tagger.Kernel.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Settings.Features;

namespace Mp3Tagger.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {        
        public CompositionsLoaderSettings CompositionsLoaderSettings { get; set; }

        public double MeasureGapMinDurationForLoad
        {
            get { return CompositionsLoaderSettings.MinDurationForLoad.TotalSeconds; }
            set { CompositionsLoaderSettings.MinDurationForLoad = TimeSpan.FromSeconds(value); OnPropertyChanged("MeasureGapMinDurationForLoad"); }
        }

        public SettingsWindow(SettingsProvider controller)
        {        
            CompositionsLoaderSettings = (CompositionsLoaderSettings) controller.GetSettingsByFeatureName(FeatureName.CompositionLoader);
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
