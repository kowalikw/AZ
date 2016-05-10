using ASD.Graphs;
using System;
using System.IO;

namespace AZ
{
    /// <summary>
    /// FileHelper class.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Reads input file.
        /// </summary>
        /// <param name="path">Path of file.</param>
        /// <returns>Graph of people pairs.</returns>
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
                    int edgeFrom = int.Parse(persons[0]);
                    int edgeTo = int.Parse(persons[1]);

                    if (edgeFrom > verticesCount - 1 || edgeTo > verticesCount - 1)
                        throw new ArgumentException();
                    if (edgeFrom < 0 || edgeTo < 0)
                        throw new ArgumentException();

                    graph.AddEdge(edgeFrom, edgeTo);
                }
            }

            return graph;
        }

        /// <summary>
        /// Saves output file.
        /// </summary>
        /// <param name="content">Content of file.</param>
        /// <param name="path">Path of file.</param>
        public static void SaveFile(string content, string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.Write(content);
            }
        }
    }
}
