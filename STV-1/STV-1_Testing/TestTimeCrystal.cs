using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestTimeCrystal
    {
        Node bridge; // dafuq?
        [TestMethod]
        public void TestTimeCrystalConstructor()
        {
            Dungeon d = new Dungeon(1);
            
            d.GenerateDungeon(1);
            foreach (Node node in d.nodes)
            {
                if (node.type == "bridge")
                    bridge = node;
            }

            Player player = new Player(10, 2, bridge, d);
            TimeCrystal crystal = new TimeCrystal();
            crystal.UseItem(player);

            Assert.AreNotEqual(player.Location, bridge); // Test to see if a player moved to another node
        }

        [TestMethod]
        public void TestTimeCrystalGetType()
        {
            TimeCrystal crystal = new TimeCrystal();
            var type = crystal.ItemType();
            Assert.AreEqual(type, "TimeCrystal");
        }
    }
}
