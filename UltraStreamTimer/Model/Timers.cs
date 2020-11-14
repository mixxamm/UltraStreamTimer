using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraStreamTimer.Model
{
    public class Timers
    {
        public ObservableCollection<TimerObject> TimerList { get; }

        public Timers()
        {
            TimerList = new ObservableCollection<TimerObject>();
        }

        public void AddTimer()
        {
            TimerList.Add(new TimerObject());
        }
    }
}
