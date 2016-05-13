using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestDungeon
    {
        [TestMethod]
        public void TestDungeonConstructor()
        {
            Dungeon d = new Dungeon(2);


            // Test if start != exit node
            Assert.AreNotEqual(d.nodes[0], d.nodes[d.nodes.Count -1]);


            // Test if no nodes have more than 4 connections
            bool fail = false;
            foreach (Node curNode in d.nodes)
            {
                if (curNode.connections.Count > 4)
                    fail = true;
            }
            Assert.IsFalse(fail);


            // Test if avarage connectivity is ~3
            int totalConnections = 0;
            foreach (Node curNode in d.nodes)
            {
                totalConnections += curNode.connections.Count;
               
            }
            float avarageConnections = totalConnections/2/d.nodes.Count;

            Assert.IsTrue(avarageConnections < 3.0f);
            // Als we zouden willen testen op reachability, kan je gwn kijken of er een path is met shortest path

        }
        [TestMethod]
        public void TestDungeonDestroy()
        {
            // DEEP COPY NODIG VOOR DE TEST 
            Dungeon d = new Dungeon(2);
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
        }

        [TestMethod]
        public void TestShortestPath()
        {
            Dungeon d = new Dungeon(2);
            Assert.IsTrue(d.FindShortestPath(d.nodes[0], d.nodes[d.nodes.Count - 1]) != null);
        }
    }
}
