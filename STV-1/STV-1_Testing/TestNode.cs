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

        [TestMethod]
        public void TestNodeAddPack()
        {
            // Test if it's possible to add a pack to a starting node
            Node a = new Node(1,1,"start");
            Pack pack = new Pack(1,new Node(1,1));
            a.AddPack(pack);
            Assert.IsTrue(pack.PackLocation != a);
            Assert.IsFalse(a.nodePacks.Contains(pack));

            // Test if it's possible to add a pack to a exit node
            Node b = new Node(1, 1, "exit");
            Pack pack2 = new Pack(1, new Node(1, 1));
            b.AddPack(pack2);
            Assert.IsTrue(pack2.PackLocation != b);
            Assert.IsFalse(b.nodePacks.Contains(pack2));

            // Test if it's possible to add a pack to a normal node
            Node c = new Node(1, 1);
            Pack pack3 = new Pack(1, new Node(1, 1));
            c.AddPack(pack3);
            Assert.AreEqual(pack3.PackLocation, c);
            Assert.IsTrue(c.nodePacks.Contains(pack3));


            // Test if it's possible to add a pack to a bridge node
            Node d = new Node(1, 1, "bridge");
            Pack pack4 = new Pack(1, new Node(1, 1));
            d.AddPack(pack4);
            Assert.AreEqual(pack4.PackLocation, d);
            Assert.IsTrue(d.nodePacks.Contains(pack4));
        }

        [TestMethod]
        public void TestNodeRemovePack()
        {
            // Test if it's possible to remove a pack from a Node
            Node a = new Node(1,1);
            Pack pack = new Pack(1,a);
            a.RemovePack(pack);
            Assert.IsFalse(a.nodePacks.Contains(pack));

            // Test if it's possible to remove a pack from a Node with a type 
        }

        [TestMethod]
        public void PackFitsInNodeTest()
        {
            
        }
    }
}
