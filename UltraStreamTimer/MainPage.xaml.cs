using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using UltraStreamTimer.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UltraStreamTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Timers = new Timers();
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(SubtractTime);
        }

        public Timers Timers { get; set; }
        int index = 0;
        Timer timer;




        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            /*int index = myList.Items.IndexOf((e.OriginalSource as FrameworkElement).DataContext);
            if (index == myList.Items.Count - 1)
                myList.Items.Insert(index, new TimerObject());*/
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            Debug.WriteLine(index);
            Timers.TimerList.ElementAt(index).Seconds -= 1;
            /*timer.Start();*/
        }

        private void SubtractTime(object sender, ElapsedEventArgs e)
        {
            Timers.TimerList.ElementAt(index).Seconds -= 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timers.AddTimer();
            /*listView.ItemsSource = null;
            listView.ItemsSource = Timers.TimerList;*/
        }
    }
}
