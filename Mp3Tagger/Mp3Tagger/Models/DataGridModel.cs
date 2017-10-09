using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Models
{
    public class DataGridModel : INotifyPropertyChanged
    {
        private ObservableCollection<Composition> compositions;

        public ObservableCollection<Composition> Compositions
        {
            get { return compositions; }
            set
            {
                compositions = value;
                OnPropertyChanged("Compositions");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DataGridModel()
        {
            if(Compositions != null)
            Compositions.CollectionChanged += (sender, args) => OnPropertyChanged("Compositions");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
