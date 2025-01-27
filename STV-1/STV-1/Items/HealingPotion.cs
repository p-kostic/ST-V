﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    /*
     * This class will take care of the healingpotion item.
     * It will inherit from the item class.
     */
    public class HealingPotion : Item
    {
        int healValue;
        private string type = "HealingPotion";

        // To make the item a bit more interesting, we are going to randomize the
        // healing value a potion can have. The value will be between 1 and 100.
        public HealingPotion()
        {
            Random random = new Random();
            healValue = random.Next(1, 100);
        }

        // If the player uses the potion, add the healvalue to the player's hp,
        // and after that destroy the item.
        public override void UseItem(Player player)
        {
            player.HP += healValue;
        }

        public override string ItemType()
        {
            return this.type;
        }
    }
}
