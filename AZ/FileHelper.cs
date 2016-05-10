using ASD.Graphs;
using System.IO;

namespace AZ
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
                    graph.AddEdge(int.Parse(persons[0]), int.Parse(persons[1]));
                }
            }

            return graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="path"></param>
        public static void SaveFile(Graph graph, string path)
        {

        }
    }
}
