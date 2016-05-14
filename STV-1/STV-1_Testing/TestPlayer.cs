using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        public void TestConstructor()
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
        public void TestDead1()
        {
            Player player = new Player(0, 0, new Node(1, 1), null);
            player.HP = 0;
            Player player2 = new Player(1, 0, new Node(1, 1), null);
            player.HP = 3;
            Assert.IsTrue(player.IsDead);
            Assert.IsFalse(player2.IsDead);
        }

        [TestMethod]
        public void TestMove()
        {
            Node a = new Node(1, 1);
            Node b = new Node(1, 1);
            Player player = new Player(10, 1, a, null);
            player.Move(b);
            Assert.AreEqual(b, player.Location);
            Assert.AreNotEqual(a, player.Location);
        }

        [TestMethod]
        public void TestAttack()
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
        }

        [TestMethod]
        public void TestPlayerSetAttack()
        {
            Player player = new Player(10, 2, new Node(1,1),new Dungeon(1));
            player.ATK = 5;
            Assert.AreNotEqual(2, player.ATK);
        }

        [TestMethod]
        public void TestGrabItem()
        {
            // TODO MEME
        }
    }
}
