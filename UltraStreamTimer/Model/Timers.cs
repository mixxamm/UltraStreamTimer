using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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
            /*TimerList.CollectionChanged += TimerList_CollectionChanged;*/
        }

        public void AddTimer()
        {
            TimerList.Add(new TimerObject());
        }
        
/*        void TimerList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (TimerObject item in e.NewItems)
                    item.PropertyChanged += TimerObject_PropertyChanged;

            if (e.OldItems != null)
                foreach (TimerObject item in e.OldItems)
                    item.PropertyChanged -= TimerObject_PropertyChanged;
        }

        void TimerObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Seconds")
                Debug.WriteLine("Seconds property is veranderd");
        }*/
    }
}
