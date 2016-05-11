using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HealingPotion : Item
{
    int healValue = 0;
    bool used;
    

    public HealingPotion()
    {
        Random random = new Random();
        healValue = random.Next(1, 20);
    }

    public void destroy()
    {
        if (used)
        {
            // Destroy potion
        }
    }

    // Getters and setters for the variables.
    public int HealValue { get { return healValue; } }
    public bool Used { set { used = value; } }
}
