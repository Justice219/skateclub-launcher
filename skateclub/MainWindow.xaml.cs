using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using skateclub.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET;

namespace skateclub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isdownloading = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LaunchGame(string args)
        {
            Process firstProc = new Process();
            firstProc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"\Skate.crack.exe";
            firstProc.StartInfo.Arguments = args;
            firstProc.Start();
        }

        private void ConnectToServer(string IP)
        {
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -DelMarOnline.Enable false -Online.ClientIsPresenceEnabled false -Client.ServerIp " + IP;

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            if (Settings.Default.ShowFPS)
                launchargs += " -DebugRender true";

            if (Settings.Default.HideWatermark)
                launchargs += " -DelMarUI.EnableWatermark false";

            //Thanks skate.online
            if (Settings.Default.EnableCosmetic)
                launchargs += " -ItemManager.ForceOwnAll true -Architect.ShowAllCategories true";

            LaunchGame(launchargs);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.AppTitle = "skateclub";
            AutoUpdater.Start("https://skateclub.xyz/api/updater/update.xml");

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Skate.crack.exe"))
            {
                MainGrid.Visibility = Visibility.Visible;
                InstallGrid.Visibility = Visibility.Hidden;
                ServersGrid.Visibility = Visibility.Hidden;
                PlayerNameGrid.Visibility = Visibility.Hidden;
                SettingsGrid.Visibility = Visibility.Hidden;

                SettingButton.Visibility = Visibility.Visible;
            }
            else
            {
                MainGrid.Visibility = Visibility.Hidden;
                InstallGrid.Visibility = Visibility.Visible;
                ServersGrid.Visibility = Visibility.Hidden;
                PlayerNameGrid.Visibility = Visibility.Hidden;
                SettingsGrid.Visibility = Visibility.Hidden;

                SettingButton.Visibility = Visibility.Hidden;
            }
        }
        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(!isdownloading)
                this.Close();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            ServersGrid.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Visible;
            ServersGrid.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -Online.ClientIsPresenceEnabled false -DelMarOnline.Enable false";

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            if (Settings.Default.ShowFPS)
                launchargs += " -DebugRender true";

            if (Settings.Default.HideWatermark)
                launchargs += " -DelMarUI.EnableWatermark false";

            //Thanks skate.online
            if (Settings.Default.EnableCosmetic)
                launchargs += " -ItemManager.ForceOwnAll true -Architect.ShowAllCategories true";

            //Solo Game
            LaunchGame(launchargs);
        }

        private void ServerBtn1_Click(object sender, RoutedEventArgs e)
        {
            //NA Server 1
            ConnectToServer("45.76.235.243");
        }

        private void ServerBtn2_Click(object sender, RoutedEventArgs e)
        {
            //NA Server 2
            ConnectToServer("144.202.75.13");
        }

        private void ServerBtn4_Click(object sender, RoutedEventArgs e)
        {
            //NA Server 3
            ConnectToServer("54.39.130.229");
        }

        private void ServerBtn3_Click(object sender, RoutedEventArgs e)
        {
            //EU Server 1
            ConnectToServer("skate.axxonte.xyz");
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            isdownloading = true;

            InstallBtn.Content = "Downloading...";
            InstallBtn.IsEnabled = false;

            PBar.Visibility = Visibility.Visible;
            PLabel.Visibility = Visibility.Visible;

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/deps/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/deps/");

            Task.Factory.StartNew(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri("https://cdn4.bunkr.is/Skate.-Crack-+Fix-&-MP-4GHwtYkU.rar"), AppDomain.CurrentDomain.BaseDirectory + "/skateclub.rar");
            });
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                PLabel.Content = "Downloading: " + ConvertBytesToMegabytes(e.BytesReceived).ToString("0.00") + "MB of " + ConvertBytesToMegabytes(e.TotalBytesToReceive).ToString("0.00") + "MB";
                PBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InstallBtn.Content = "Downloading...";
            });

            Task.Factory.StartNew(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client86_DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri("https://aka.ms/vs/17/release/vc_redist.x86.exe"), AppDomain.CurrentDomain.BaseDirectory + "/deps/vc_redist.x86.exe");
            });
        }

        void client86_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InstallBtn.Content = "Downloading...";
            });

            Task.Factory.StartNew(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client64_DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri("https://aka.ms/vs/17/release/vc_redist.x64.exe"), AppDomain.CurrentDomain.BaseDirectory + "/deps/vc_redist.x64.exe");
            });
        }

        void client64_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InstallBtn.Content = "Extracting...";
            });

            ExtractGameFiles();
        }

        void ExtractGameFiles()
        {
            Task.Factory.StartNew(() =>
            {
                //Extract RAR
                using (var archive = RarArchive.Open(@AppDomain.CurrentDomain.BaseDirectory + "/skateclub.rar"))
                {
                    Dispatcher.Invoke(() =>
                    {
                        PBar.Maximum = archive.Entries.Count();
                    });

                    int i = 0;

                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToDirectory(@AppDomain.CurrentDomain.BaseDirectory, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });

                        Dispatcher.Invoke(() =>
                        {
                            PLabel.Content = "Extracting: " + entry.ToString();
                            PBar.Value = i;
                        });

                        i++;
                    }
                }

                //Move files from skate 4 folder to base directory
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/"))
                {
                    foreach (var file in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/").GetFiles())
                    {
                        file.MoveTo($@"{AppDomain.CurrentDomain.BaseDirectory}\{file.Name}");
                    }

                    foreach (var dir in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/").GetDirectories())
                    {
                        dir.MoveTo($@"{AppDomain.CurrentDomain.BaseDirectory}\{dir.Name}");
                    }
                }

                /*Dispatcher.Invoke(() =>
                {
                    InstallBtn.Content = "Waiting...";
                    PLabel.Content = "Please install the Microsoft Visual C++ Redistributables";
                });

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\deps\vc_redist.x86.exe"))
                {
                    Process firstProc = new Process();
                    firstProc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"\deps\vc_redist.x86.exe";
                    firstProc.Start();
                    firstProc.WaitForExit();
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\deps\vc_redist.x64.exe"))
                {
                    Process SecondProc = new Process();
                    SecondProc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"\deps\vc_redist.x64.exe";
                    SecondProc.Start();
                    SecondProc.WaitForExit();
                }*/

                StartSetName();

                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/"))
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/");

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/skateclub.rar"))
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/skateclub.rar");
            });
        }

        void StartSetName()
        {
            Dispatcher.Invoke(() =>
            {
                PlayerNameText.Text = Settings.Default.Playername;

                MainGrid.Visibility = Visibility.Hidden;
                InstallGrid.Visibility = Visibility.Hidden;
                ServersGrid.Visibility = Visibility.Hidden;
                PlayerNameGrid.Visibility = Visibility.Visible;
                SettingsGrid.Visibility = Visibility.Hidden;

                isdownloading = false;
            });
        }

        private void SaveName_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Playername = PlayerNameText.Text;
            Settings.Default.Save();

            MainGrid.Visibility = Visibility.Visible;
            InstallGrid.Visibility = Visibility.Hidden;
            ServersGrid.Visibility = Visibility.Hidden;
            PlayerNameGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
            SettingButton.Visibility = Visibility.Visible;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer(IPText.Text);
        }

        private void HostServerButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchGame(@"-Server");
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Playername = PlayerNameSettingsText.Text;
            Settings.Default.DX11 = ShadowFix.IsChecked.Value;
            Settings.Default.ShowFPS = ShowFPS.IsChecked.Value;
            Settings.Default.HideWatermark = HideWatermark.IsChecked.Value;
            Settings.Default.EnableCosmetic = EnableCosmetic.IsChecked.Value;
            Settings.Default.Save();

            MainGrid.Visibility = Visibility.Visible;
            InstallGrid.Visibility = Visibility.Hidden;
            ServersGrid.Visibility = Visibility.Hidden;
            PlayerNameGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerNameSettingsText.Text = Settings.Default.Playername;
            ShadowFix.IsChecked = Settings.Default.DX11;
            ShowFPS.IsChecked = Settings.Default.ShowFPS;
            HideWatermark.IsChecked = Settings.Default.HideWatermark;
            EnableCosmetic.IsChecked = Settings.Default.EnableCosmetic;

            MainGrid.Visibility = Visibility.Hidden;
            InstallGrid.Visibility = Visibility.Hidden;
            ServersGrid.Visibility = Visibility.Hidden;
            PlayerNameGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Visible;
        }

        private void Discord_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://discord.gg/skateclub");
        }
    }
}
