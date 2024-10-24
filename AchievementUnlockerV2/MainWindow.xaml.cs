using JRPCPlusPlus;
using JsonHelper;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using XDevkit;

namespace AchievementUnlockerV2
{
    public partial class MainWindow : Window
    {
        XMessageBox mb;
        WaitForm wf;

        IXboxConsole console;

        private bool connected = false;
        private bool useCachedAddresses = false;

        class Entry
        {
            public string TitleId { get; set; }
            public string Address { get; set; }
        }

        private bool WriteEntry(string fileName, string tid, string addr)
        {
            try
            {
                var entry = new Entry { TitleId = tid, Address = addr };
                return JSONHelper.WriteToFile($"{fileName}.json", entry);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private uint EntryExists(string filename, string titleId)
        {
            string currentTid = console.XamGetCurrentTitleId().ToString("X");
            string address = JSONHelper.GetAddressForTitleId(filename, titleId);

            if (address != null)
            {
                return uint.Parse(address, System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                return 0;
            }
        }

        private void ShowMessageBox(string message, string caption="Error")
        {
            mb = new XMessageBox(message, caption);
            mb.ShowDialog();
        }

        private void WriteAchievements()
        {
            if (connected)
            {
                string path = "XUserWriteAchievements.json";
                string titleId = console.XamGetCurrentTitleId().ToString("X");
                uint address = EntryExists(path, titleId);
                uint xamfreemem = console.XamFreeMemory17559();

                if (useCachedAddresses && address != 0)
                {
                    console.XUserWriteAchievements(address, xamfreemem, xamfreemem + 0x8, 201);
                }
                else
                {
                    uint tid = console.XamGetCurrentTitleId();
                    uint callAddress = console.LocateXUserWriteAchievements();
                    uint xamFreeMem = console.XamFreeMemory17559();

                    WriteEntry("XUserWriteAchievements", tid.ToString("X"), (callAddress - 0x24).ToString("X"));

                    if (callAddress != 0)
                    {
                        console.XUserWriteAchievements(callAddress - 0x24, xamFreeMem, xamFreeMem + 8, 201); // i set the count high because the id's are not always in sequence. better chance to get all of them
                    }
                    else
                    {
                        throw new Exception("Failed to find the function address!");
                    }
                }
            }
            else
            {
                throw new Exception("You are not connected to your console!");
            }
        }

        private void WriteAvatarAwards()
        {
            if (connected)
            {
                string path = "XUserAwardAvatarAssets.json";
                string titleId = console.XamGetCurrentTitleId().ToString("X");
                uint address = EntryExists(path, titleId);
                uint xamfreemem = console.XamFreeMemory17559() + 0x20;

                if (useCachedAddresses && address != 0)
                {
                    console.XUserWriteAchievements(address, xamfreemem, xamfreemem + 0x8, 36);
                }
                else
                {
                    uint tid = console.XamGetCurrentTitleId();
                    uint callAddress = console.LocateXUserAwardAvatarAssets();
                    uint xamFreeMem = console.XamFreeMemory17559() + 0x20;

                    WriteEntry("XUserAwardAvatarAssets", tid.ToString("X"), (callAddress - 0x24).ToString("X"));

                    if (callAddress != 0)
                    {
                        console.XUserAwardAvatarAsset(callAddress - 0x24, xamFreeMem, xamFreeMem + 8, 36);
                    }
                    else
                    {
                        throw new Exception("Failed to find the function address! Make sure the game has avatar awards.");
                    }
                }
            }
            else
            {
                throw new Exception("You are now connected to your console!");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool Connect()
        {
            if (console.Connect(out console))
            {
                connected = true;
                return true;
            }
            else
            {
                connected= false;
                return false;
            }
            
        }

        private async void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wf = new WaitForm();
                wf.Show();
                await Task.Run(() => Connect());
                wf.Close();
                if (!connected)
                {
                    ShowMessageBox("Failed to connect to default console!", "Connection Error");
                }
            }
            catch (Exception ex)
            {
                wf.Close();
                ShowMessageBox(ex.Message, ex.GetType().ToString());
            }
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private async void AchievementsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wf = new WaitForm();
                wf.Show();
                await Task.Run(() => WriteAchievements());
                wf.Close();
            }
            catch(Exception ex)
            {
                wf.Close();
                ShowMessageBox(ex.Message, ex.GetType().ToString());
            }
        }

        private async void AvatarAwardsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wf = new WaitForm();
                wf.Show();
                await Task.Run(() => WriteAvatarAwards());
                wf.Close();
            }
            catch (Exception ex)
            {
                wf.Close();
                ShowMessageBox(ex.Message, ex.GetType().ToString());
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                string url = "https://github.com/XeCrippy/AchievementUnlocker";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, ex.GetType().ToString());
            }
           
        }
    }
}