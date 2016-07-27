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
        public void FindScheduleTest1()
        {
            Graph g = FileHelper.LoadFile("Resources\\test1.txt");
            List<Tuple<Edge, Edge>> lst = ScheduleAlgorithm.FindSchedule(g);
            Assert.AreEqual(lst.Count, 4);
        }

        [TestMethod]
        public void FindScheduleTest2()
        {
            Graph g = FileHelper.LoadFile("Resources\\test2.txt");
            List<Tuple<Edge, Edge>> lst = ScheduleAlgorithm.FindSchedule(g);
            Assert.AreEqual(lst.Count, 5);
        }

        [TestMethod]
        public void FindScheduleTest3()
        {
            Graph g = FileHelper.LoadFile("Resources\\test3.txt");
            List<Tuple<Edge, Edge>> lst = ScheduleAlgorithm.FindSchedule(g);
            Assert.AreEqual(lst.Count, 8);
        }
    }
}
