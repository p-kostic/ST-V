using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestNode
    {
        [TestMethod]
        public void TestNodeAddPlayer()
        {
            Node a = new Node(1,1);
            Player player = new Player(10,10,a, null);
            a.AddPlayer(player);
            Assert.AreEqual(a, player.Location);
        }
    }
}
