using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestHealingPotion
    {
        [TestMethod]
        public void TestHealingPotionExeedingHP()
        {
            HealingPotion healingPotion = new HealingPotion();
            Player player = new Player(10,1,new Node(1,1), new Dungeon(2));

            healingPotion.UseItem(player);

            // Test if it didn't exceed max hp
            Assert.AreEqual(10, player.HP);
        }

        [TestMethod]
        public void TestHealingPotionHealing()
        {
            HealingPotion healingPotion = new HealingPotion();
            Player player = new Player(10, 2, new Node(1, 1), new Dungeon(2));

            // Damage the player a bit.
            player.HP -= 5;

            healingPotion.UseItem(player);

            // We now test if the player has been healed.
            Assert.IsTrue(player.HP > 5);
        }

        [TestMethod]
        public void TestHealingPotionItemType()
        {
            HealingPotion potion = new HealingPotion();
            var type = potion.ItemType();
            Assert.AreEqual(type, "HealingPotion");
        }
    }
}