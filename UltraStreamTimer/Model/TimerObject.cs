using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraStreamTimer.Model
{
    public class TimerObject : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Seconds { get; set; }

        public TimerObject()
        {
            Seconds = 60;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name?? $"Naam {Seconds}";
        }
    }
}
