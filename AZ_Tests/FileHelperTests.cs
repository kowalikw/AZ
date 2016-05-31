using ASD.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AZ;
using System.Collections.Generic;

namespace AZ_Tests
{
    [TestClass]
    public class FileHelperTests
    {
        [TestMethod]
        public void FileReaderTest()
        {
            Graph g = FileHelper.LoadFile("Resources\\test.txt");

            Assert.AreEqual(g.VerticesCount, 5);
            Assert.AreEqual(g.EdgesCount, 7);
            List<Edge> lst = new List<Edge>();
            for (int i = 0; i < 5; i++)
                foreach (Edge e in g.OutEdges(i))
                    lst.Add(e);

            Assert.IsTrue(lst.Contains(new Edge(0, 1)));
            Assert.IsTrue(lst.Contains(new Edge(0, 2)));
            Assert.IsTrue(lst.Contains(new Edge(0, 3)));
            Assert.IsTrue(lst.Contains(new Edge(0, 4)));
            Assert.IsTrue(lst.Contains(new Edge(3, 1)));
            Assert.IsTrue(lst.Contains(new Edge(4, 1)));
            Assert.IsTrue(lst.Contains(new Edge(2, 3)));
        }

        [TestMethod]
        public void SaveFileTest()
        {
            FileHelper.SaveFile("Some random content", "tst.txt");
            if (System.IO.File.Exists("tst.txt"))
                System.IO.File.Delete("tst.txt");
        }
    }
}
