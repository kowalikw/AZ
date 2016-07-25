using ASD.Graphs;
using System.Collections.Generic;
using System;

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

        /// <summary>
        /// Finds maximum matching in source graph.
        /// </summary>
        /// <param name="graph">Source graph.</param>
        /// <param name="M">Initial of matched edges.</param>
        /// <returns>List of matched edges.</returns>
        public static List<Edge> FindMaximumMatching(this Graph graph, List<Edge> M)
        {
            var P = FindAugmentingPath(graph, M);
            if (P != null && P.Count > 0)
            {
                AugmentMatchingAlongPath(ref M, P);

                return FindMaximumMatching(graph, M);
            }
            else
                return M;
        }

        #region Private methods.

        private static List<Edge> FindAugmentingPath(Graph graph, List<Edge> M)
        {
            Graph F = new AdjacencyMatrixGraph(false, graph.VerticesCount);
            bool[] insideF = new bool[graph.VerticesCount];
            bool[] rootsF = new bool[graph.VerticesCount];
            bool[] markedVertices = new bool[graph.VerticesCount];
            bool[,] markedEdges = new bool[graph.VerticesCount, graph.VerticesCount];

            foreach (Edge e in M)
                markedEdges[e.From, e.To] = markedEdges[e.To, e.From] = true;
            for (int i = 0; i < graph.VerticesCount; i++)
                if (IsExposedVertex(i, M))
                {
                    insideF[i] = true;
                    rootsF[i] = true;
                }

            int v = -1;
            while(IsUnmarkedVertexInForestWithRootEvenDistance(F, markedVertices, insideF, rootsF, out v))
            {
                Edge e;
                while(IsUnmarkedEdge(graph, markedEdges, v, out e))
                {
                    int w = e.To;
                    if(!insideF[w]) // w nie jest w lesie
                    {
                        int x = -1;
                        foreach (Edge matchedEdge in M)
                        {
                            if (matchedEdge.From == w)
                                x = matchedEdge.To;
                            if (matchedEdge.To == w)
                                x = matchedEdge.From;
                        }

                        F.AddEdge(v, w);
                        F.AddEdge(w, x);
                        insideF[v] = true;
                        insideF[w] = true;
                        insideF[x] = true;
                    }
                    else // jest w lesie
                    {
                        Edge[] distance;
                        F.AStar(w, RootOfVertexInTree(F, rootsF, w), out distance);

                        if(distance.Length % 2 == 1)
                        {
                            // DO NOTHING
                        }
                        else
                        {
                            if(RootOfVertexInTree(F, rootsF, v) != RootOfVertexInTree(F, rootsF, w))
                            {
                                List<Edge> augmentingPath = new List<Edge>();

                                Edge[] pathV;
                                Edge[] pathW;

                                F.AStar(RootOfVertexInTree(F, rootsF, v), v, out pathV);
                                F.AStar(w, RootOfVertexInTree(F, rootsF, w), out pathW);

                                foreach (Edge edge in pathV)
                                    augmentingPath.Add(edge);
                                augmentingPath.Add(new Edge(v, w));
                                foreach (Edge edge in pathW)
                                    augmentingPath.Add(edge);


                                return augmentingPath;
                            }
                            else
                            {
                                List<int> cycle = new List<int>();
                                Edge[] path;
                                F.AStar(v, w, out path);

                                foreach (var edge in path)
                                    cycle.Add(edge.From);
                                cycle.Add(w);

                                int cycleRoot = -1;
                                foreach (var i in cycle)
                                {
                                    if (rootsF[i])
                                    {
                                        cycleRoot = i;
                                        break;
                                    }
                                }

                                int vertexNumber = 1;
                                int[] association = new int[F.VerticesCount];
                                for(int i = 0; i < F.VerticesCount; i++)
                                {
                                    if (cycle.Contains(i))
                                        association[i] = 0;
                                    else
                                        association[i] = vertexNumber++;
                                }

                                Graph graphPrim = new AdjacencyMatrixGraph(false, F.VerticesCount - cycle.Count + 1);
                                for(int i = 0; i < association.Length; i++)
                                {
                                    foreach(var edge in graph.OutEdges(i))
                                    {
                                        int from = association[edge.From];
                                        int to = association[edge.To];

                                        if (graphPrim.GetEdgeWeight(from, to) == null && graphPrim.GetEdgeWeight(to, from) == null && from != to)
                                            graphPrim.AddEdge(from, to);
                                    }
                                }

                                List<Edge> MPrim = new List<Edge>();

                                foreach(var edge in M)
                                {
                                    if (association[edge.From] == association[edge.To])
                                        continue;

                                    int from = association[edge.From];
                                    int to = association[edge.To];

                                    Edge e1 = new Edge(from, to);
                                    Edge e2 = new Edge(to, from);

                                    if (!MPrim.Contains(e1) && !MPrim.Contains(e2))
                                        MPrim.Add(new Edge(from, to));
                                }

                                //GraphExport ge = new GraphExport();
                                //ge.Export(graphPrim);

                                // 0 = blossom (cykl zwinięty ma indeks 0)
                                List<Edge> pathPrim = FindAugmentingPath(graphPrim, MPrim);
                                List<Edge> augmentingPath = new List<Edge>();

                                // Rozciąganie ścieżki poniżej

                                if (pathPrim != null && pathPrim.Count > 0)
                                {
                                    foreach (var edge in pathPrim)
                                    {
                                        // cykl w środku
                                        if (edge.From != 0 && edge.To != 0) // TODO: jakaś krawędź skojarzona musi być na ścieżce rozciągniętej!
                                        {
                                            int from = -1;
                                            int to = -1;

                                            for (int i = 0; i < association.Length; i++)
                                            {
                                                if (association[i] == edge.From)
                                                    from = i;
                                                if (association[i] == edge.To)
                                                    to = i;  
                                            }
                                            augmentingPath.Add(new Edge(from, to));
                                        }
                                        else // cykl jest jednym z końców
                                        {

                                            int x = -1;
                                            int z = -1;

                                            if (edge.From == 0)
                                            {
                                                for (int i = 0; i < association.Length; i++)
                                                    if (association[i] == edge.To)
                                                        z = i;
                                            }
                                            else if (edge.To == 0)
                                            {
                                                for (int i = 0; i < association.Length; i++)
                                                    if (association[i] == edge.From)
                                                        z = i;
                                            }

                                            /*foreach (var vertex in cycle) // find exposed vertex from cycle
                                            {
                                                if (IsExposedVertex(vertex, M))
                                                {
                                                    x = vertex;
                                                    break;
                                                }
                                            }*/

                                            x = cycle[0]; // exposed i root cyklu

                                            // zawsze istnieje exposed vertex w cyklu

                                            if(graph.GetEdgeWeight(z, x) != null) // exposed vertex bezpośrednio sąsiaduje
                                            {
                                                augmentingPath.Add(new Edge(z, x));
                                            }
                                            else // konieczność przejścia przez krawędź z M
                                            {
                                                int i = 0;
                                                for (i = 0; i < cycle.Count; i++) // wyszukanie sąsiedniego wierzchołka do "z"
                                                    if (graph.GetEdgeWeight(z, cycle[i]) != null)
                                                        break;

                                                augmentingPath.Add(new Edge(z, cycle[i]));

                                                if(M.Contains(new Edge(cycle[i], cycle[(i+1) % cycle.Count])) || M.Contains(new Edge(cycle[(i + 1) % cycle.Count], cycle[i]))) //++
                                                {
                                                    while(cycle[i % cycle.Count] != x)
                                                    {
                                                        augmentingPath.Add(new Edge(cycle[i % cycle.Count], cycle[(i + 1) % cycle.Count]));
                                                        i++;
                                                    }
                                                }
                                                else //--
                                                {
                                                    while (cycle[(i + cycle.Count) % cycle.Count] != x)
                                                    {
                                                        augmentingPath.Add(new Edge(cycle[(i + cycle.Count) % cycle.Count], cycle[((i - 1) + cycle.Count) % cycle.Count]));
                                                        i--;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                return augmentingPath;
                            }
                        }
                    }

                    markedEdges[e.From, e.To] = true;
                    markedEdges[e.To, e.From] = true;
                }
                markedVertices[v] = true;
            }

            return null;
        }

        private static bool IsUnmarkedVertexInForestWithRootEvenDistance(Graph F, bool[] markedVertices, bool[] insideF, bool[] rootsF, out int v)
        {
            v = -1;

            for(int i = 0; i < F.VerticesCount; i++)
            {
                if(insideF[i] && !markedVertices[i])
                {
                    Edge[] path;
                    if(F.AStar(i, RootOfVertexInTree(F, rootsF, i), out path))
                    {
                        if(path.Length % 2 == 0)
                        {
                            v = i;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static int RootOfVertexInTree(Graph F, bool[] rootsF, int v)
        {
            Edge[] path;
            if (rootsF[v]) return v;

            for (int i = 0; i < rootsF.Length; i++)
                if (rootsF[i] && F.AStar(v, i, out path))
                    return i;

            return -1;
        }

        private static bool IsExposedVertex(int v, List<Edge> M)
        {
            foreach (Edge e in M)
                if (v == e.From || v == e.To)
                    return false;
            return true;
        }

        private static bool IsUnmarkedEdge(Graph graph, bool[,] markedEdges, int v, out Edge e)
        {
            e = new Edge(-1, -1);

            for(int i = 0; i < graph.VerticesCount; i++)
                if(i != v && graph.GetEdgeWeight(v, i) != null && !markedEdges[v, i])
                {
                    e = new Edge(v, i);
                    return true;
                }    

            return false;
        }

        private static void AugmentMatchingAlongPath(ref List<Edge> M, List<Edge> path)
        {
            foreach (Edge e in path)
            {
                Edge e1 = new Edge(e.From, e.To);
                Edge e2 = new Edge(e.To, e.From);

                M.Remove(e1);
                M.Remove(e2);
            }

            for (int i = 0; i < path.Count; i += 2)
                M.Add(path[i]);
        }

        #endregion
    }
}
