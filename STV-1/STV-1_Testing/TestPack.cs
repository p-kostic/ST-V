using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestPack
    {
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
        public void CheckPackHP()
        {
            Pack pack = new Pack(10, new Node(1, 1));

            // First test: Check if the total pack HP is correct (should be 100)
            Assert.AreEqual(100, pack.PackHP);
        }

        [TestMethod]
        public void CheckPackDamage()
        {
            Pack pack = new Pack(10, new Node(1, 1));

            // Damage the pack for 50 hitpoints
            foreach (Monster monster in pack.Monsters)
                monster.HP -= 5;

            // Check if the packHP is still correct after the damage
            Assert.AreEqual(50, pack.PackHP);
        }
    }
}
