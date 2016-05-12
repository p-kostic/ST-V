using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestCreature
    {
        [TestMethod]
        public void TestCreatureConstructor()
        {
            Dungeon d = new Dungeon(2);

            Assert.IsTrue(d.FindShortestPath(d.nodes[0], d.nodes[d.nodes.Count - 1]) != null);
        }
    }
}
