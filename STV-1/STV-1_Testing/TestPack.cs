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
            Node a = new Node(2,5);
            Pack pack = new Pack(5, a);

            // Check if the correct amount of monsters are created in the node.
            Assert.AreEqual(pack.Monsters.Count, 5);

            Node b = new Node(2,1);
            Pack pack2 = new Pack(20, b);

            // Check if the constraint on node capacity works
            Assert.AreNotEqual(pack2.Monsters.Count, 20);
            // Check if the amount is indeed equal to the Node's max monsters
            Assert.AreEqual(pack2.Monsters.Count, b.MaxMonsters);
        }

        [TestMethod]
        public void TestMovePack()
        {
            Node a = new Node(1,1);
            Node b = new Node(1,1);

            Pack pack = new Pack(5,a);
            pack.MovePack(b);
            // Test if movement doesn't work across unconnected nodes
            Assert.AreNotEqual(pack.PackLocation, b);

            a.connections.Add(b);
            b.connections.Add(a);
            pack.MovePack(b);

            // Test if it can now indeed move after they are connected
            Assert.AreEqual(pack.PackLocation,b);



            // Test if a pack is able to move to a start or exit node
            Node x = new Node(1, 1, "start");
            Node y = new Node(1, 1, "exit");
            Node xy = new Node(1,1);

            x.connections.Add(xy);
            xy.connections.Add(x);
            y.connections.Add(xy);
            xy.connections.Add(y);

            Pack pack2 = new Pack(5, xy);
            pack2.MovePack(x);
            Assert.AreNotEqual(pack2.PackLocation, x);
            pack2.MovePack(y);
            Assert.AreNotEqual(pack2.PackLocation, y);



            // TODO: Test if a pack can move to a node with full capacity, this should not be possible
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
            Node location = new Node(4,5);
            Pack pack = new Pack(5,location);
            Player player = new Player(20, 5, location, new Dungeon(1));

            pack.PackAttack(player);
            Assert.AreEqual(player.HP, 10);

            // Test the same if the Node level is not high enough
            Node location2 = new Node(1,1);
            Pack pack2 = new Pack(5, location2);
            Player player2 = new Player(20, 5, location, new Dungeon(1));

            pack2.PackAttack(player2);
            Assert.AreNotEqual(player2.HP, 10);

            // Test if it's possible to attack a player on a different node
            pack2.PackAttack(player);
            Assert.AreEqual(player.HP, 10);

            // Test if it's possible to attack a monster (instead of a player)
            Node location3 = new Node(4,5);
            Pack pack3 = new Pack(5,location3);
            Monster monster = new Monster(20,2,location3);
            pack3.PackAttack(monster);
            Assert.AreEqual(monster.HP, 10);
        }

        [TestMethod]
        public void CheckPackHP()
        {
            Pack pack = new Pack(10, new Node(4,5));

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
    }
}
