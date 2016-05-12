using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestMonster
    {
        [TestMethod]
        public void TestConstructor()
        {
            int hp = 10;
            int atk = 5;
            Node a = new Node(1, 1);
            Monster monster = new Monster(hp, atk, a, null);

            Assert.AreEqual(a, monster.Location);
            Assert.AreEqual(monster.HP, hp);
            Assert.AreEqual(monster.ATK, atk);
        }

        [TestMethod]
        public void TestDead()
        {
            Monster monster = new Monster(0, 0, new Node(1, 1), null);
            monster.HP = 0;
            Monster monster2 = new Monster(1, 0, new Node(1, 1), null);
            monster2.HP = 3;
            Assert.IsTrue(monster.IsDead);
            Assert.IsFalse(monster2.IsDead);
        }

        [TestMethod]
        public void TestMove()
        {
            Node a = new Node(1, 1);
            Node b = new Node(1, 1);
            Monster monster = new Monster(10, 1, a, null);
            monster.Move(b);
            Assert.AreEqual(b, monster.Location);
            Assert.AreNotEqual(a, monster.Location);
        }

        [TestMethod]
        public void TestAttack()
        {
            Node a = new Node(1, 1);
            Monster monster = new Monster(10, 5, a, null);
            Monster monster2 = new Monster(10, 2, a, null);

            int starthp = monster2.HP;
            int expectedhp = starthp - monster.ATK;

            monster.Attack(monster2);

            Assert.AreEqual(expectedhp, monster2.HP);
        }
    }
}
