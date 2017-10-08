using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Models
{
    public class ProcessState : INotifyPropertyChanged
    {
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ObservableCollection<string> History
        {
            get { return history; }
            set
            {
                history = value; 
                OnPropertyChanged("History");
            }
        }

        public int OperationsCount
        {
            get { return operationsCount; }
            set
            {
                operationsCount = value;
                OnPropertyChanged("OperationsCount");
                OnPropertyChanged("StateText");
            }
        }

        public int OperationsPerformed
        {
            get { return operationsPerformed; }
            set
            {
                operationsPerformed = value;
                OnPropertyChanged("OperationsPerformed");
                OnPropertyChanged("StateText");
            }
        }

        public string StateText
        {
            get { return $"Processed: {OperationsPerformed}/{OperationsCount}"; }
            set
            {
                stateText = value;
                OnPropertyChanged("StateText");
            }
        }

        public string CurrentFeatureName
        {
            get { return currentFeatureName; }
            set
            {
                currentFeatureName = value;
                OnPropertyChanged("CurrentFeatureName");
            }
        }

        private bool isBusy;
        private int operationsCount;
        private int operationsPerformed;
        private string stateText;
        private string currentFeatureName;
        private int totalCompositions;
        private ObservableCollection<string> history;

        public ProcessState()
        {
            CurrentFeatureName = "Ready";
            History = new ObservableCollection<string>();
            History.CollectionChanged += (sender, args) => OnPropertyChanged("History");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
