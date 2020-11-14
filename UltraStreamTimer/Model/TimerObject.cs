using System;
using System.ComponentModel;

namespace UltraStreamTimer.Model
{
    public class TimerObject : INotifyPropertyChanged, IComparable
    {
        private string name;
        private int seconds;

        public string Name { get => name; set { name = value; RaisePropertyChanged("Name"); } }
        public int Seconds { get => seconds; set
            {
                seconds = value;
                RaisePropertyChanged("Seconds");
            }
        }

        public TimerObject(string name)
        {
            Name = name;
            Seconds = 60;
        }

        public override string ToString()
        {
            return Name ?? $"Naam {Seconds}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is TimerObject)
                return ((TimerObject)obj).Seconds.CompareTo(seconds);
            else
                return 0;
        }
    }
}
