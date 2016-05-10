using ASD.Graphs;
using System.Collections.Generic;

namespace AZ
{
    /// <summary>
    /// Extends Graph library.
    /// </summary>
    public static class GraphHelper
    {
        /// <summary>
        /// Constructs line graph to the source graph.
        /// </summary>
        /// <param name="graph">Source graph.</param>
        /// <param name="associations">Out list of vertices and new edged associations.</param>
        /// <returns>Line graph.</returns>
        public static Graph LineGraph(this Graph graph, out List<Edge> associations)
        {
            associations = new List<Edge>();
            Graph lineGraph = new AdjacencyMatrixGraph(false, graph.EdgesCount);

            // generates associations between old graph edges and new graph vertices
            for(int v = 0; v < graph.VerticesCount; v++)
            {
                foreach(Edge e in graph.OutEdges(v))
                {
                    bool canAdd = true;

                    for (int i = 0; i < associations.Count; i++)
                        if (associations[i].From == e.To && associations[i].To == e.From)
                            canAdd = false;

                    if(canAdd)
                        associations.Add(new Edge(e.From, e.To));
                }
            }

            // adds appropriate edges to new graph
            for(int v = 0; v < graph.VerticesCount; v++)
            {
                foreach(Edge e in graph.OutEdges(v))
                {
                    foreach(Edge eNext in graph.OutEdges(e.To))
                    {
                        int v1 = 0, v2 = 0;

                        for (int i = 0; i < associations.Count; i++)
                        {
                            if (associations[i].From == e.From && associations[i].To == e.To ||
                                associations[i].From == e.To && associations[i].To == e.From)
                                v1 = i;
                            if (associations[i].From == eNext.From && associations[i].To == eNext.To ||
                                associations[i].From == eNext.To && associations[i].To == eNext.From)
                                v2 = i;
                        }

                        if(v1 != v2)
                            lineGraph.AddEdge(v1, v2);
                    }
                }
            }

            return lineGraph;
        }

        /// <summary>
        /// Constructs complement graph to the source graph.
        /// </summary>
        /// <param name="graph">Source graph.</param>
        /// <returns>Complement graph.</returns>
        public static Graph ComplementGraph(this Graph graph)
        {
            Graph complementGraph = new AdjacencyMatrixGraph(false, graph.VerticesCount);

            for(int i = 0; i < graph.VerticesCount; i++)
            {
                for(int j = 0; j < graph.VerticesCount; j++)
                {
                    if (i == j) continue;
                    if(graph.GetEdgeWeight(i, j) == null)
                    {
                        complementGraph.AddEdge(i, j);
                    }
                }
            }

            return complementGraph;
        }

        // Mock-up algorytmu.
        /// <summary>
        /// Finds macimum matching in source graph.
        /// </summary>
        /// <param name="graph">Source graph.</param>
        /// <returns>Maximum graph matching.</returns>
        public static List<Edge> MaxMatching(this Graph graph)
        {
            List<Edge> matching = new List<Edge>();

            for(int v = 0; v < graph.VerticesCount; v++)
            {
                foreach(Edge e in graph.OutEdges(v))
                {
                    matching.Add(e);
                }
            }

            return matching;
        }
    }
}
