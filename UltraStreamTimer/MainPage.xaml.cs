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
using Windows.Storage;
using Windows.Storage.AccessCache;
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
        
        Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Windows.Storage.StorageFile timerFile;
        public MainPage()
        {
            this.InitializeComponent();
            Timers = new Timers();
            dispatcherTimer.Tick += SubtractTime;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            InitializeFile();
        }

        public async void InitializeFile()
        {
            timerFile =
    await storageFolder.CreateFileAsync("timer.txt",
        Windows.Storage.CreationCollisionOption.ReplaceExisting);
        }
        public Timers Timers { get; set; }
        int index = 0;
        IStorageFile storageFile;



        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            /*int index = myList.Items.IndexOf((e.OriginalSource as FrameworkElement).DataContext);
            if (index == myList.Items.Count - 1)
                myList.Items.Insert(index, new TimerObject());*/
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            Debug.WriteLine(index);
            Timers.TimerList.ElementAt(index).Seconds -= 1;
            dispatcherTimer.Start();
        }

        private async void SubtractTime(object sender, object e)
        {
            Timers.TimerList.ElementAt(index).Seconds -= 1;
            await Windows.Storage.FileIO.WriteTextAsync(storageFile, $"{Timers.TimerList.ElementAt(index).Seconds}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timers.AddTimer(Name.Text);
            /*listView.ItemsSource = null;
            listView.ItemsSource = Timers.TimerList;*/
        }

        private async void ChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("PickedFolderToken");
                storageFile = await storageFolder.CreateFileAsync("timer.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
        }
    }
}
