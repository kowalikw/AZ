using ASD.Graphs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph mainGraph;

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
                    mainGraph = FileHelper.LoadFile(openFileDialog.FileName);

                    List<Tuple<int, int>> associations;

                    var line = mainGraph.LineGraph(out associations);

                    //GraphExport ge = new GraphExport();
                    //ge.Export(mainGraph);
                    //ge.Export(mainGraph.ComplementGraph());
                    //ge.Export(line);
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
