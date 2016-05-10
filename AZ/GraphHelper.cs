using ASD.Graphs;
using System;
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
        public static Graph LineGraph(this Graph graph, out List<Tuple<int, int>> associations)
        {
            associations = new List<Tuple<int, int>>();
            Graph lineGraph = new AdjacencyMatrixGraph(false, graph.EdgesCount);

            // generates associations between old graph edges and new graph vertices
            for(int v = 0; v < graph.VerticesCount; v++)
            {
                foreach(Edge e in graph.OutEdges(v))
                {
                    bool canAdd = true;

                    for (int i = 0; i < associations.Count; i++)
                        if (associations[i].Item1 == e.To && associations[i].Item2 == e.From)
                            canAdd = false;

                    if(canAdd)
                        associations.Add(new Tuple<int, int>(e.From, e.To));
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
                            if (associations[i].Item1 == e.From && associations[i].Item2 == e.To ||
                                associations[i].Item1 == e.To && associations[i].Item2 == e.From)
                                v1 = i;
                            if (associations[i].Item1 == eNext.From && associations[i].Item2 == eNext.To ||
                                associations[i].Item1 == eNext.To && associations[i].Item2 == eNext.From)
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

        public static void Matching(this Graph graph)
        {

        }
    }
}
