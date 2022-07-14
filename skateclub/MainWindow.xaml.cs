using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using skateclub.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Skate.crack.exe"))
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
            LaunchGame(@"-Online.ClientIsPresenceEnabled false -DelMarUI.EnableWatermark false -DelMarOnline.Enable false");

            LogsText.AppendText("Start solo skate session.");
        }

        private void ServerBtn1_Click(object sender, RoutedEventArgs e)
        {
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -DelMarUI.EnableWatermark false -DelMarOnline.Enable false -Online.ClientIsPresenceEnabled false -Client.ServerIp " + "45.76.235.243";

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            LaunchGame(launchargs);

            LogsText.AppendText("Joining skateclub NA1.");
        }

        private void ServerBtn2_Click(object sender, RoutedEventArgs e)
        {
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -DelMarUI.EnableWatermark false -DelMarOnline.Enable false -Online.ClientIsPresenceEnabled false -Client.ServerIp " + "144.202.75.13";

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            LaunchGame(launchargs);

            LogsText.AppendText("Joining skateclub NA2.");
        }

        private void ServerBtn3_Click(object sender, RoutedEventArgs e)
        {
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -DelMarUI.EnableWatermark false -DelMarOnline.Enable false -Online.ClientIsPresenceEnabled false -Client.ServerIp " + "skate.axxonte.xyz";

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            LaunchGame(launchargs);

            LogsText.AppendText("Joining skateclub EU1.");
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
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client86_DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client86_DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri("https://aka.ms/vs/17/release/vc_redist.x86.exe"), AppDomain.CurrentDomain.BaseDirectory + "/deps/vc_redist.x86.exe");
            });
        }

        void client86_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
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

        void client86_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InstallBtn.Content = "Downloading...";
            });

            Task.Factory.StartNew(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client64_DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client64_DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri("https://aka.ms/vs/17/release/vc_redist.x64.exe"), AppDomain.CurrentDomain.BaseDirectory + "/deps/vc_redist.x64.exe");
            });
        }

        void client64_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
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

                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "/skate 4/");
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/skateclub.rar");

                Dispatcher.Invoke(() =>
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
                }

                StartSetName();
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
            string launchargs = @"-DelMar.LocalPlayerDebugName " + Settings.Default.Playername + " -DelMarUI.EnableWatermark false -DelMarOnline.Enable false -Online.ClientIsPresenceEnabled false -Client.ServerIp " + IPText.Text;

            if (Settings.Default.DX11)
                launchargs += " -Render.ForceDx11 true";

            LaunchGame(launchargs);

            LogsText.AppendText("Connecting to server.");
        }

        private void HostServerButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchGame(@"-Server");

            LogsText.AppendText("Starting Server.");
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Playername = PlayerNameSettingsText.Text;
            Settings.Default.DX11 = ShadowFix.IsChecked.Value;
            Settings.Default.Save();

            MainGrid.Visibility = Visibility.Visible;
            InstallGrid.Visibility = Visibility.Hidden;
            ServersGrid.Visibility = Visibility.Hidden;
            PlayerNameGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;

            LogsText.AppendText("Saving settings.");
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerNameSettingsText.Text = Settings.Default.Playername;
            ShadowFix.IsChecked = Settings.Default.DX11;

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
