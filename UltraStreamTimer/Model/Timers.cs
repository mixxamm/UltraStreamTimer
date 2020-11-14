using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraStreamTimer.Model
{
    public class Timers
    {
        public List<TimerObject> TimerList { get; }

        public Timers()
        {
            TimerList = new List<TimerObject>();
        }

        public void AddTimer()
        {
            TimerList.Add(new TimerObject());
        }
    }
}
