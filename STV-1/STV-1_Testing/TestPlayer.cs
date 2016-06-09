using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        public void TestPlayerConstructor()
        {
            int hp = 10;
            int atk = 5;
            Node a = new Node(1, 1);
            Player player = new Player(hp, atk, a, null);

            Assert.AreEqual(a, player.Location);
            Assert.AreEqual(player.HP, hp);
            Assert.AreEqual(player.ATK, atk);
        }

        [TestMethod]
        public void TestPlayerDead()
        {
            Player player = new Player(0, 0, new Node(1, 1), null);
            player.HP = 0;
            Player player2 = new Player(1, 0, new Node(1, 1), null);
            player.HP = 3;
            Assert.IsTrue(player.IsDead);
            Assert.IsFalse(player2.IsDead);
        }

        [TestMethod]
        public void TestPlayerMove()
        {
            Node a = new Node(1, 1);
            Node b = new Node(1, 1);
            Player player = new Player(10, 1, a, null);
            player.Move(b);
            Assert.AreEqual(b, player.Location);
            Assert.AreNotEqual(a, player.Location);
        }

        [TestMethod]
        public void TestPlayerAttack()
        {
            // Test if a player receives dmg from a monster
            Node a = new Node(1, 1);
            Monster monster = new Monster(10, 5, a);
            Player player = new Player(10, 2, a, null);

            int starthp = player.HP;
            int expectedhp = starthp - monster.ATK;

            monster.Attack(player);

            Assert.AreEqual(expectedhp, player.HP);

            // Test if a monster can receive dmg from a player.
            Node a2 = new Node(1, 1);
            Monster monster2 = new Monster(10, 5, a2);
            Player player2 = new Player(10, 2, a2, null);

            int starthp2 = monster2.HP;
            int expectedhp2 = starthp2 - player2.ATK;
           
            player2.Attack(monster2);

            Assert.AreEqual(expectedhp2, monster2.HP);

            // Test if kp are increased if a player kills a monster
            Node a3 = new Node(1, 1);
            Monster monster3 = new Monster(10, 5, a3);
            Player player3 = new Player(10, 10, a3, null);

            player3.Attack(monster3);
            Assert.IsTrue(monster3.IsDead);
            Assert.IsTrue(player3.kp > 0);

        }

        [TestMethod]
        public void TestPlayerSetAttack()
        {
            Player player = new Player(10, 2, new Node(1,1),new Dungeon(1));
            player.ATK = 5;
            Assert.AreNotEqual(2, player.ATK);
        }

        [TestMethod]
        public void TestPlayerUseCrystal()
        {
            // Test if the proper functionality of a time crystal on a bridge works
            Dungeon d = new Dungeon(3);
            Node bridge = new Node(1,1);
            d.GenerateDungeon(3);
            int size = d.nodes.Count;
            foreach (Node node in d.nodes)
            {
                if (node.type == "bridge")
                    bridge = node;
            }
            Player player = new Player(50, 5, bridge, d);
            player.inventory.Add(new TimeCrystal());
            

            Assert.IsTrue(player.UseCrystal());

            // Test if the crystal can't be used if it's not in the player's inventory and if the crystal has been removed
            Assert.IsFalse(player.UseCrystal());
        }

        [TestMethod]
        public void TestPlayerUsePotion()
        {
            Dungeon d = new Dungeon(3);
            Node a = new Node(1,1);
            Player player = new Player(50,5,a,d);
            player.inventory.Add(new HealingPotion());
            Assert.IsTrue(player.UsePotion());
            Assert.IsFalse(player.UsePotion()); // Checks both if the potion was removed during usage and if no potion can be used without one in the inventory

        }

        [TestMethod]
        public void TestAddItem()
        {
            // Make a new player
            Dungeon d = new Dungeon(3);
            Node n = new Node(1, 1);
            Player player = new Player(5, 5, n, d);

            // The bag shouldn't currently hold a healing potion.
            Assert.IsTrue(player.inventory.Count == 0);
            // Add a healing potion.
            player.AddItem(true);
            // The player should now have a healing potion.
            Assert.IsTrue(player.inventory.Exists(x => x.ItemType() == "HealingPotion"));
            // Do the same for the timecrystal.
            Assert.IsFalse(player.inventory.Exists(x => x.ItemType() == "TimeCrystal"));
            player.AddItem(false);
            Assert.IsTrue(player.inventory.Exists(x => x.ItemType() == "TimeCrystal"));
        }

        [TestMethod]
        public void TestPlayerGrabItems()
        {
            Dungeon d = new Dungeon(2);
            Node a = new Node(1,1);
            Node a2 = new Node(1,1);
            a.connections.Add(a2);
            a2.connections.Add(a);
            a2.items.Add(new HealingPotion());
            Player player = new Player(50, 5, a, d);
            int inventorySize = player.inventory.Count;
            player.Move(a2);
            Assert.AreNotEqual(inventorySize, player.inventory.Count);
        }

        [TestMethod]
        public void TestVisitedList()
        {
            // Make a new player.
            Dungeon x = new Dungeon(1);
            Node n = new Node(1, 1);
            Player player = new Player(5, 5, n, x);

            // Level isn't equal to curLevel, so clear the list.
            Assert.AreNotEqual(player.levelChecker, 5);
            player.levelChecker = 0;
            player.VisitedList(5);
            // Now they should be equal.
            Assert.AreEqual(player.levelChecker, 5);

            // Add some nodes to the visitedList.
            player.visitedList.Add(1);
            player.visitedList.Add(2);
            player.visitedList.Add(3);
            player.visitedList.Add(4);

            // Run the method, and check if it returns the correct string.
            player.levelChecker = 1;
            string[] testString = player.VisitedList(1).Split(',');
            string[] withoutStart = new string[4];
            for (int i = 1; i < testString.Length; i++)
                withoutStart[i - 1] = testString[i];
            Assert.AreEqual("1,2,3,4", string.Join(",", withoutStart));
        }
    }
}
