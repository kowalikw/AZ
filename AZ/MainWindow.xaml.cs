using ASD.Graphs;
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
                    bool[,] addedPairs = new bool[mainGraph.VerticesCount, mainGraph.VerticesCount];

                    listOfPairs.Text = "";
                    for (int v = 0; v < mainGraph.VerticesCount; v++)
                    {
                        foreach(Edge edge in mainGraph.OutEdges(v))
                        {
                            if (!addedPairs[edge.From, edge.To])
                            {
                                listOfPairs.Text += edge.From.ToString() + ", " + edge.To.ToString() + "\n";
                                addedPairs[edge.From, edge.To] = true;
                                addedPairs[edge.To, edge.From] = true;
                            }
                        }
                    }

                    labelCoursesCount.Content = 0;
                    labelPairsCount.Content = mainGraph.EdgesCount.ToString();
                }
                catch(Exception)
                {
                    MessageBox.Show("Nieprawidłowy format pliku! Spróbuj wczytać inny plik.", "Wystąpił błąd!");
                }
            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileHelper.SaveFile(resultSchedule.Text, saveFileDialog.FileName);
            }
        }

        private void btnFindSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (mainGraph == null)
            {
                MessageBox.Show("Aby wyznaczyć rozkład jazdy należy wczytać plik z danymi.", "Wystąpił błąd!");
                return;
            }

            var schedule = ScheduleAlgorithm.FindSchedule(mainGraph);

            resultSchedule.Text = "";
            foreach(var item in schedule)
            {
                if(item.Item2.From != -1 && item.Item2.To != -1)
                    resultSchedule.Text += item.Item1.From + "," + item.Item1.To +  " " + item.Item2.From + "," + item.Item2.To + "\n";
                else
                    resultSchedule.Text += item.Item1.From + "," + item.Item1.To + "\n";
            }

            labelCoursesCount.Content = schedule.Count.ToString();
        }
    }
}
