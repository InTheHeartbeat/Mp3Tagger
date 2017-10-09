using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Models
{
    public class ProcessingStateViewModel : INotifyPropertyChanged
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

        public int OperationsCount
        {
            get { return operationsCount; }
            set
            {
                operationsCount = value; 
                OnPropertyChanged("OperationsCount");
                OnPropertyChanged("ProgressText");
            }
        }

        public int OperationsPerformed
        {
            get { return operationsPerformed; }
            set
            {
                operationsPerformed = value;
                OnPropertyChanged("OperationsPerformed");
                OnPropertyChanged("ProgressText");
            }
        }

        public IFeature CurrentFeature
        {
            get { return currentFeature; }
            set
            {
                currentFeature = value; 
                OnPropertyChanged("CurrentFeature");
            }            
        }

        public string ProgressText
        {
            get { return $"Processing: {OperationsPerformed}/{OperationsCount}"; }
            set
            {
                progressText = value;
                OnPropertyChanged("ProgressText");
            }
        }

        private bool isBusy;
        private int operationsCount;
        private int operationsPerformed;
        private TimeSpan elapsed;
        private IFeature currentFeature;
        private string progressText;

        public TimeSpan Elapsed
        {
            get { return elapsed; }
            set { elapsed = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
