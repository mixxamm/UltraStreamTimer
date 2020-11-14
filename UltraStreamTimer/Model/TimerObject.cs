using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraStreamTimer.Model
{
    public class TimerObject
    {
        public string Name { get; set; }
        public int Seconds { get; set; }

        public TimerObject()
        {
            Seconds = 60;
        }

        public override string ToString()
        {
            return Name?? $"Naam {Seconds}";
        }
    }
}
