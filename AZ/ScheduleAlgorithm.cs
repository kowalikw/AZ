using ASD.Graphs;
using System;
using System.Collections.Generic;

namespace AZ
{
    /// <summary>
    /// Algorithm for calculating optimal kayaking schedule.
    /// </summary>
    public static class ScheduleAlgorithm
    {
        /// <summary>
        /// Calculates optimal kayaking schedule for people pairs.
        /// </summary>
        /// <param name="graph">Source graph of people pairs.</param>
        /// <returns>Returns optimal schedule.</returns>
        public static List<Tuple<Edge, Edge>> FindSchedule(Graph graph)
        {
            List<Edge> association;
            List<Edge> M = new List<Edge>();
            List<Tuple<Edge, Edge>> schedule = new List<Tuple<Edge, Edge>>();

            var lineGraph = graph.LineGraph(out association);
            var complementGraph = lineGraph.ComplementGraph();

            //GraphExport ge = new GraphExport();
            //ge.Export(complementGraph);

            var maxMatching = complementGraph.FindMaximumMatching(M);

            bool[] extractedEdges = new bool[complementGraph.VerticesCount];
            foreach(var item in maxMatching)
            {
                if(!(association[item.From].From == association[item.To].From || association[item.From].From == association[item.To].To || association[item.From].To == association[item.To].From || association[item.From].To == association[item.To].To))
                {
                    extractedEdges[item.From] = true;
                    extractedEdges[item.To] = true;

                    schedule.Add(new Tuple<Edge, Edge>(association[item.From], association[item.To]));
                }
            }

            for(int i = 0; i < extractedEdges.Length; i++)
                if(!extractedEdges[i])
                    schedule.Add(new Tuple<Edge, Edge>(association[i], new Edge(-1, -1)));

            return schedule;
        }
    }
}
