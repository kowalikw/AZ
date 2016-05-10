using ASD.Graphs;
using System.IO;

namespace AZ
{
    public static class FileHelper
    {
        public static Graph LoadFile(string path)
        {
            Graph graph;

            using (StreamReader file = new StreamReader(path))
            {
                string line;
                int verticesCount = int.Parse(file.ReadLine());

                graph = new AdjacencyMatrixGraph(false, verticesCount);
                while ((line = file.ReadLine()) != null)
                {
                    var persons = line.Split(',');
                    graph.AddEdge(int.Parse(persons[0]), int.Parse(persons[1]), 1);
                }
            }

            return graph;
        }

        public static void SaveFile()
        {

        }
    }
}
