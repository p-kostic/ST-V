using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HealingPotion : Item
{
    int healValue;

    public HealingPotion()
    {
        Random random = new Random();
        healValue = random.Next(1, 20);
    }

    public override void UseItem(Player player)
    {
        // If used, heal the player.
    }

    public void destroy()
    {
    }
}
