using ASD.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AZ;
using System.Collections.Generic;

namespace AZ_Tests
{
    [TestClass]
    public class GraphHelperTests
    {

        [TestMethod]
        public void LineGraphTest()
        {
            Graph g = FileHelper.LoadFile("Resources\\test.txt");
            List<Edge> lst;
            Graph k = GraphHelper.LineGraph(g, out lst);
            Assert.AreEqual(14, k.EdgesCount);
            Assert.AreEqual(7, k.VerticesCount);
        }

        [TestMethod]
        public void ComplementGraphTest()
        {
            Graph g = FileHelper.LoadFile("Resources\\test.txt");
            Graph k = GraphHelper.ComplementGraph(g);

            List<Edge> lst = new List<Edge>();
            for (int i = 0; i < 5; i++)
                foreach (Edge e in k.OutEdges(i))
                    lst.Add(e);
            for (int i = 0; i < 5; i++)
                foreach (Edge e in g.OutEdges(i))
                    Assert.IsFalse(lst.Contains(e));
            Assert.AreEqual(3, k.EdgesCount);
        }

        [TestMethod]
        public void FindMaximumMatchingTest()
        {
            Graph g = FileHelper.LoadFile("Resources\\test.txt");
            List<Edge> M, M1 = new List<Edge>();
            Graph k = GraphHelper.LineGraph(g, out M);
            k = GraphHelper.ComplementGraph(k);
            M = GraphHelper.FindMaximumMatching(k, M1);
            Assert.AreEqual(M.Count, (int)(k.VerticesCount/2));
        }
    }
}
