using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestPack
    {
        [TestMethod]
        public void TestPackConstructor()
        {
            Node a = new Node(2, 5);
            Pack pack = new Pack(5, a);

            // Check if the correct amount of monsters are created in the node.
            Assert.AreEqual(pack.Monsters.Count, 5);

            Node b = new Node(2, 1);
            Pack pack2 = new Pack(20, b);

            // Check if the constraint on node capacity works
            Assert.AreNotEqual(pack2.Monsters.Count, 20);
            // Check if the amount is indeed equal to the Node's max monsters
            Assert.AreEqual(pack2.Monsters.Count, b.MaxMonsters);


            // Try to make a new pack at a wrong startlocation
            Pack pack3 = new Pack(10, new Node(1, 1, "start"));
            Assert.IsTrue(pack3.Monsters.Count == 0); // No monsters should be added
        }

        [TestMethod]
        public void TestMovePack()
        {
            Node a = new Node(1, 1);
            Node b = new Node(1, 1);

            Pack pack = new Pack(5, a);
            pack.MovePack(b);
            // Test if movement doesn't work across unconnected nodes
            Assert.AreNotEqual(pack.PackLocation, b);

            a.connections.Add(b);
            b.connections.Add(a);
            pack.MovePack(b);

            // Test if it can now indeed move after they are connected
            Assert.AreEqual(pack.PackLocation, b);

            // Test if a pack is able to move to a start or exit node
            Node x = new Node(1, 1, "start");
            Node y = new Node(1, 1, "exit");
            Node xy = new Node(1, 1);

            x.connections.Add(xy);
            xy.connections.Add(x);
            y.connections.Add(xy);
            xy.connections.Add(y);

            Pack pack2 = new Pack(5, xy);
            pack2.MovePack(x);
            Assert.AreNotEqual(pack2.PackLocation, x);
            pack2.MovePack(y);
            Assert.AreNotEqual(pack2.PackLocation, y);

            // Test if a pack can move to a node with full capacity, this should not be possible
            Node c = new Node(1,1);
            Node c2 = new Node(1,1);
            c.connections.Add(c2);
            c2.connections.Add(c);
            Pack pack3 = new Pack(2, c);
            Pack pack4 = new Pack(2, c2);
            Node var = pack3.PackLocation;
            pack3.MovePack(c2);
            Assert.AreEqual(var, pack3.PackLocation);

        }

        [TestMethod]
        public void TestPackMoves()
        {
            Pack pack = new Pack(5, new Node(1, 1));

            pack.MovePack(new Node(2, 1));

            // Check if every monster has moved to the pack location
            foreach (Monster monster in pack.Monsters)
            {
                Assert.AreEqual(pack.PackLocation, monster.Location);
            }
        }

        [TestMethod]
        public void TestPackAttack()
        {
            // Test if a pack can attack a player
            Node location = new Node(4, 5);
            Pack pack = new Pack(5, location);
            Player player = new Player(20, 5, location, new Dungeon(1));

            pack.PackAttack(player);
            Assert.AreEqual(player.HP, 10);

            // Test the same if the Node level is not high enough
            Node location2 = new Node(1, 1);
            Pack pack2 = new Pack(5, location2);
            Player player2 = new Player(20, 5, location, new Dungeon(1));

            pack2.PackAttack(player2);
            Assert.AreNotEqual(player2.HP, 10);

            // Test if it's possible to attack a player on a different node
            pack2.PackAttack(player);
            Assert.AreEqual(player.HP, 10);

            // Test if it's possible to attack a monster (instead of a player)
            Node location3 = new Node(4, 5);
            Pack pack3 = new Pack(5, location3);
            Monster monster = new Monster(20, 2, location3);
            pack3.PackAttack(monster);
            Assert.AreEqual(monster.HP, 10);
        }

        [TestMethod]
        public void CheckPackHP()
        {
            Pack pack = new Pack(10, new Node(4, 5));

            // First test: Check if the total pack HP is correct (should be 100)
            Assert.AreEqual(100, pack.PackHP);
        }

        [TestMethod]
        public void CheckPackDamage()
        {
            Pack pack = new Pack(10, new Node(4, 5));

            // Damage the pack for 50 hitpoints
            foreach (Monster monster in pack.Monsters)
                monster.HP -= 5;

            // Check if the packHP is still correct after the damage
            Assert.AreEqual(50, pack.PackHP);
        }

        [TestMethod]
        public void TestPackUpdatePack()
        {
            // Test to see if deaths are handled correctly 
            Pack pack = new Pack(10, new Node(4, 5));
            foreach (Monster monster in pack.Monsters)
                monster.HP -= 5;
            pack.UpdatePack();

            Assert.AreEqual(pack.Monsters.Count, 10); // None should be removed

            // Now we test if it is updated accordingly to the deaths
            Pack pack2 = new Pack(20, new Node(4, 5));
            for (int i = 0; i < pack2.Monsters.Count - 1; i++)
                pack2.Monsters[i].HP = -1;
            pack2.UpdatePack();

            Assert.AreEqual(pack2.Monsters.Count, 1); // Er moeten geen monsters meer in de lijst zitten
        }

        [TestMethod]
        public void TestPackLocation()
        {
            // Test to see if an empty pack is correctly deleted
            Pack pack = new Pack(0, new Node(1,1));
            Assert.AreEqual(pack.PackLocation, null);
        }

        [TestMethod]
        public void TestUpdatePack2()
        {
            Pack p = new Pack(1, new Node(1, 1));

            p.monsters[0].HP -= 10;
            // Check if the pack gets deleted.
            p.UpdatePack();
            Assert.IsNotNull(p.PackLocation);
        }

        [TestMethod]
        public void TestHandlePackAI()
        {
            Dungeon d = new Dungeon(1);
            d.GenerateDungeon(1, false);
            Player p = new Player(10, 2, d.nodes[0], d);
            Pack packie = new Pack(5, d.nodes[2]);

            packie.HandlePackAI(p, d);

            // The pack shouldn't move to the player.
            Assert.IsFalse(packie.PackLocation != d.nodes[2]);
        }
    }
}
