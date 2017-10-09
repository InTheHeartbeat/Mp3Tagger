using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Kernel.History
{
    public class HistoryKeeper : INotifyPropertyChanged
    {
        public ObservableCollection<HistoryEntry> History { get; private set; }

        public HistoryKeeper()
        {
            History = new ObservableCollection<HistoryEntry>();
            History.CollectionChanged += (sender, args) => OnPropertyChanged("History");            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
