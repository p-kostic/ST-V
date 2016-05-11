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

            Assert.IsTrue(d.FindShortestPath(d.nodes[0], d.nodes[d.nodes.Count -1]) != null);
        }
    }
}
