using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using System.Configuration;

namespace TimeStampGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool flag = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag = true;
                StartButton.Visibility = Visibility.Collapsed;
                CreatingLabel.Visibility = Visibility.Visible;
                FileNameLabel.Visibility = Visibility.Visible;
                if (StopButton.Visibility == Visibility.Collapsed)
                {
                    StopButton.Visibility = Visibility.Visible;
                }
                string directoryPath = string.Empty;
                VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
                dlg.SelectedPath = ConfigurationManager.AppSettings["directoryPath"].ToString();
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == true)
                {
                    directoryPath = dlg.SelectedPath;
                }
                var dir = new DirectoryInfo(directoryPath);
                FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
                StringBuilder sb = new StringBuilder();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        sb.Append(file.Name).Append("\t").Append(file.FullName).Append("\n");

                    }
                    var result = sb.ToString();
                    string fileName = string.Empty;
                    int counter = 0;
                    while (flag && counter < 5)
                    {
                        fileName = DateTime.Now.ToString("yyyy-MM-ddTHH.mm.ss.fff");
                        CreateTimestampFileAndFillItWithFilesStructure(result, directoryPath, String.Concat(fileName, ".txt"));
                        FileNameLabel.Content = fileName;
                        await Task.Delay(2000);
                        counter++;
                    }
                    StopButton.Visibility = Visibility.Collapsed;
                    if (StartButton.Visibility == Visibility.Collapsed)
                    {
                        CreatingLabel.Visibility = Visibility.Collapsed;
                        StartButton.Visibility = Visibility.Visible;
                        FileNameLabel.Visibility = Visibility.Collapsed;
                    }

                }
            }
            catch (Exception ex) {
                MessageBox.Show("Exception just occurred: " + ex.Message, "Exception Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            

        }

        private void CreateTimestampFileAndFillItWithFilesStructure(string result, string directoryPath, string fileName)
        {
            try {
                File.WriteAllText(System.IO.Path.Join(directoryPath, fileName), result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception just occurred: " + ex.Message, "Exception Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            flag = false;
            StopButton.Visibility = Visibility.Collapsed;
            if (StartButton.Visibility == Visibility.Collapsed)
            {
                StartButton.Visibility = Visibility.Visible;
            }
        }
    }
}
