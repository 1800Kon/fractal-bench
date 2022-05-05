using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FractalBench
{
    public class Chart : INotifyPropertyChanged
    {
        private int utilization = 0;
        public int Utilization
        {
            get
            {
                return utilization;
            }
            set
            {
                this.utilization = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime Time { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
