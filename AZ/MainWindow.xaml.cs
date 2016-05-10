using Microsoft.Win32;
using System;
using System.Windows;

namespace AZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileHelper.LoadFile(openFileDialog.FileName);
                }
                catch(Exception)
                {
                    MessageBox.Show("");
                }
            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
