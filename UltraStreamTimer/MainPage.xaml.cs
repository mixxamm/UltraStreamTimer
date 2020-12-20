using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraStreamTimer.Model;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UltraStreamTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainPage()
        {
            InitializeComponent();
            Timers = new Timers();
            dispatcherTimer.Tick += SubtractTime;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }
        public Timers Timers { get; set; }

        private int index = 0;
        private IStorageFile storageFile, allTimersFile, currentTimerName;



        private async void ListButton_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            bool indexChanged = index != newIndex;

            index = newIndex;
            Debug.WriteLine(index);
            if (dispatcherTimer.IsEnabled && !indexChanged)
                StopTimer();
            else
            {
                await SaveTimerToFile();
                dispatcherTimer.Start();
                if (storageFile == null)
                {
                    dispatcherTimer.Stop();
                    PickFolder();
                }
                else
                {
                    await FileIO.WriteTextAsync(currentTimerName, $"{Timers.TimerList.ElementAt(index).Name}");
                }
            }
                
        }

        private async void SubtractTime(object sender, object e)
        {
            if (Timers.TimerList.ElementAt(index).Seconds > 0)
            {
                Timers.TimerList.ElementAt(index).Seconds -= 1;
                await SaveTimerToFile();
            }
            else
            {
                StopTimer();
            }

        }

        private async Task SaveTimerToFile()
        {
            if (storageFile == null)
            {
                dispatcherTimer.Stop();
                PickFolder();
            }
            else
            {
                try
                {
                    await FileIO.WriteTextAsync(storageFile, $"{Timers.TimerList.ElementAt(index).Seconds}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timers.AddTimer(Name.Text);
            Name.Text = "";
            /*listView.ItemsSource = null;
            listView.ItemsSource = Timers.TimerList;*/
        }

        private void ChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            PickFolder();
        }

        private async void PickFolder()
        {
            Windows.Storage.Pickers.FolderPicker folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                StorageFolder storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("PickedFolderToken");
                storageFile = await storageFolder.CreateFileAsync("timer.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                allTimersFile = await storageFolder.CreateFileAsync("all_timers.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                currentTimerName = await storageFolder.CreateFileAsync("current_timer_name.txt", CreationCollisionOption.ReplaceExisting);
            }
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            Timers.TimerList.ElementAt(index).Seconds = 60;
            await SaveTimerToFile();
            if (storageFile == null)
            {
                PickFolder();
            }
            else
            {
                await FileIO.WriteTextAsync(currentTimerName, $"{Timers.TimerList.ElementAt(index).Name}");
                UpdateAllTimersFile();
            }
        }

        private async void AddKButton_Click(object sender, RoutedEventArgs e)
        {
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            try
            {
                Timers.TimerList.ElementAt(index).Seconds += int.Parse(KTextBox.Text) * 1000;
                await SaveTimerToFile();
                UpdateAllTimersFile();
                KTextBox.Text = (int.Parse(KTextBox.Text) * -1).ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void AddMButton_Click(object sender, RoutedEventArgs e)
        {
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            try
            {
                Timers.TimerList.ElementAt(index).Seconds += (int)(double.Parse(MTextBox.Text) * 1000000.0);
                await SaveTimerToFile();
                UpdateAllTimersFile();
                MTextBox.Text = (double.Parse(MTextBox.Text) * -1).ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StopTimer()
        {
            dispatcherTimer.Stop();
            UpdateAllTimersFile();
        }

        private void ClearK_Click(object sender, RoutedEventArgs e)
        {
            KTextBox.Text = "";
        }

        private void ClearM_Click(object sender, RoutedEventArgs e)
        {
            MTextBox.Text = "";
        }

        private async void UpdateAllTimersFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<TimerObject> list = Timers.TimerList.ToList();
            list.Sort();
            list.ForEach(timerObject =>
            {
                stringBuilder.AppendLine($"{timerObject.Name}: {timerObject.Seconds.ToString("N0")}");
            });
            if (storageFile == null)
            {
                PickFolder();
            }
            else
            {
                await FileIO.WriteTextAsync(allTimersFile, stringBuilder.ToString());
            }
        }

        private async void AddSeconds_Click(object sender, RoutedEventArgs e)
        {
            index = Timers.TimerList.IndexOf((TimerObject)(e.OriginalSource as FrameworkElement).DataContext);
            Timers.TimerList.ElementAt(index).Seconds += int.Parse((sender as Windows.UI.Xaml.Controls.Button).Content.ToString().Substring(1));
            await SaveTimerToFile();
            if (storageFile == null)
            {
                PickFolder();
            }
            else
            {
                await FileIO.WriteTextAsync(currentTimerName, $"{Timers.TimerList.ElementAt(index).Name}");
                UpdateAllTimersFile();
            }
        }
    }
}
