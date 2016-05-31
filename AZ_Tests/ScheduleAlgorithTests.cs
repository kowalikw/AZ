using ASD.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AZ;
using System.Collections.Generic;
using System;

namespace AZ_Tests
{
    [TestClass]
    public class ScheduleAlgorithTests
    {
        [TestMethod]
        public void FindScheduleTest()
        {
            Graph g = FileHelper.LoadFile("Resources\\test.txt");
            List<Tuple<Edge, Edge>> lst = ScheduleAlgorithm.FindSchedule(g);
            Assert.AreEqual(lst.Count, 4);
        }
    }
}
