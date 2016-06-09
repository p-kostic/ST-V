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
        public void PackInNodeTest()
        {
            // Test to see if it will it returns true if a pack is in a node
            Node a = new Node(4,5);
            Pack pack = new Pack(5,a);
            Assert.IsTrue(a.PackInNode());
            
            // Test to see if it will return false if a pack is not created in that node.
            Node b = new Node(4, 5);
            Assert.IsFalse(b.PackInNode());
        }

        [TestMethod]
        public void PlayerInNodeTest()
        {
            // Check if it returns true if a player is in the node
            Node a = new Node(4,5);
            Player player = new Player(20, 2, a, new Dungeon(1));
            Assert.IsTrue(a.PlayerInNode());

            // Check if it returns false if a player is in the node
            Node b = new Node(4,5);
            Assert.IsFalse(b.PlayerInNode());
        }

        [TestMethod]
        public void NodeUseItemTest()
        {
            HealingPotion healingPotion = new HealingPotion();
            Node a = new Node(4,5);
            Player player = new Player(100, 2, a, new Dungeon(1));
            a.AddPlayer(player);
            // Simulate that the player received damage
            player.HP -= 90;
            a.UseItem(healingPotion);
            // After using the item, we check if there was any healing
            Assert.AreNotEqual(player.HP, 10);
        }

        [TestMethod]
        public void TestNodeConnections()
        {
            Node node1 = new Node(0, 0);
            Node node2 = new Node(0, 0);

            // The nodes shouldn't be connected
            Assert.IsFalse(node1.ConnectedTo(node2));
        }

        [TestMethod]
        public void TestCombatRound()
        {
            // Make a new node, player and pack for a combat situation.
            Node node = new Node(10, 3);
            Player player = new Player(100, 10, node, null);
            Pack pack = new Pack(5, node);

            // Do the actual combat
            node.DoCombatRound(pack, player, false);

            // We check if a monster died in the pack,
            // since the attack power of the player is equal to the monster his health.
            Assert.AreEqual(4, pack.PackSize);

            // All the monsters should attack the player, and we should account for the monster
            // that died, so 4 * 2 = 8, 100 - 8 should be 92.
            Assert.AreEqual(92, player.HP);
        }

        [TestMethod]
        public void PackInNode()
        {
            Node node = new Node(2, 2);
            Pack p = new Pack(5, node);

            node.AddPack(p);

            // Check if the pack gets placed into the node.
            Assert.AreEqual(1, node.nodePacks.Count);
        }

        [TestMethod]
        public void CheckForCombatSituation()
        {
            // Make a node, a player, and a pack.
            Node node = new Node(2, 2);
            Player player = new Player(10, 10, node, null);
            Pack pack = new Pack(4, node);

            // There should be a combat situation.
            Assert.IsTrue(node.CheckInCombat());
        }

        [TestMethod]
        public void TestPackFleeing()
        {
            // Make two new nodes and connect them.
            Node node1 = new Node(2, 2);
            Node node2 = new Node(2, 2);
            node1.connections.Add(node2);
            node2.connections.Add(node1);

            // Make a player and a pack.
            Player player = new Player(100, 2, node1, null);
            Pack pack = new Pack(5, node1);

            // Do the actual combat.
            node1.DoCombatRound(pack, player, false);

            // The pack should have fled to the other node.
            Assert.IsTrue(pack.PackLocation == node2);
        }

        [TestMethod]
        public void TestHowManyMonsters()
        {
            // Create the packs and a starting point.
            Node s = new Node(1, 1);
            Pack p1 = new Pack(5, s);
            Pack p2 = new Pack(5, s);

            // Place the packs in a new node.
            Node n = new Node(1, 1);
            n.AddPack(p1);
            n.AddPack(p2);

            // The method should return 10 (5 * 2)
            Assert.AreEqual(2, n.HowManyPacks());
        }

        [TestMethod]
        public void CheckInCombat2()
        {
            Dungeon d = new Dungeon(2);
            Node n = new Node(1, 1);
            Player p = new Player(10, 5, n, d);

            // Combat should be false.
            Assert.IsFalse(n.CheckInCombat());
        }
    }
}
