using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestDungeon
    {
        [TestMethod]
        public void TestDungeonGenerator()
        {
            Random rand = new Random();
            Dungeon d = new Dungeon(1, rand.Next());
            d.GenerateDungeon(1);

            // Test if start != exit node
            Assert.AreNotEqual(d.nodes[0], d.nodes[d.nodes.Count -1]);

            // Test if no nodes have more than 4 connections
            foreach (Node curNode in d.nodes)
            {
                Assert.IsFalse(curNode.connections.Count > 4);
            }
            // Test if average connectivity is ~3
            int totalConnections = 0;
            foreach (Node curNode in d.nodes)
            {
                totalConnections += curNode.connections.Count;
               
            }
            float averageConnections = totalConnections/2/d.nodes.Count;

            Assert.IsTrue(averageConnections < 3.0f);

            // Test if the amount of bridges in the dungeon is equal to the level of the dungeon
            int level = 3;
            int count = 0;
            Dungeon d2 = new Dungeon(level, rand.Next());
            d2.GenerateDungeon(level);
            foreach (Node node in d2.nodes)
            {
                if (node.type == "bridge")
                    count++;
            }
            Assert.AreEqual(count, level);

            // Test if no nodes lack any connections
            Dungeon d3 = new Dungeon(2, rand.Next());
            d3.GenerateDungeon(2);
            foreach (Node n in d.nodes)
            {
                    Assert.AreNotEqual(d3.nodes.Count, 0);
            }
        }
        [TestMethod]
        public void TestDungeonDestroy()
        {
            // DEEP COPY NODIG VOOR DE TEST 
            Random rand = new Random();
            Dungeon d = new Dungeon(2, rand.Next());
            d.GenerateDungeon(2);
            int size = d.nodes.Count;

            foreach (Node curNode in d.nodes)
            {
                if (curNode.type == "bridge")
                {
                    d.Destroy(curNode);
                    break;
                }
            }
            Assert.AreNotEqual(size, d.nodes.Count);

            // Test if it's possible to destroy something else than a bridge
            d.Destroy(d.nodes[d.nodes.Count - 1]);
            Assert.AreEqual(d.nodes[d.nodes.Count - 1].type, "exit");
        }


        [TestMethod]
        public void TestDungeonShortestPath()
        {
            Random rand = new Random();
            // Test shortest path
            for (int i = 1; i <= 5; i++)
            {
                Dungeon d = new Dungeon(i, rand.Next());
                d.GenerateDungeon(i);
                Assert.IsTrue(d.FindShortestPath(d.nodes[0], d.nodes[d.nodes.Count - 1]) != null);
            }

            // Test trivial path
            Dungeon d2 = new Dungeon(1, rand.Next());
            Node startend = new Node(1,1);
            d2.nodes.Add(startend);
            Assert.IsTrue(d2.FindShortestPath(d2.nodes[0], d2.nodes[0]).Contains(startend));

            // Test if it can find an impossible path
            Dungeon d3 = new Dungeon(2, rand.Next());
            Node start = new Node(1,1, "start");
            Node end = new Node(1,1, "exit");
            d3.nodes.Add(start);
            d3.nodes.Add(end);

            Assert.IsTrue(d3.FindShortestPath(d3.nodes[0], d3.nodes[d3.nodes.Count - 1]) == null);
        }

        [TestMethod]
        public void TestDungeonGetters()
        {
            Random rand = new Random();
            Dungeon d = new Dungeon(2, rand.Next());
            d.GenerateDungeon(2);
            Assert.IsTrue(d.GetExit.type == "exit");
            Assert.IsTrue(d.GetStart.type == "start");
        }
    }
}
