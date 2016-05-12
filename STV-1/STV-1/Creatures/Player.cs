using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    /* 
     * This class will handle all the player specific actions.
     * Since the player is a creature, it will inherit from the
     * abstract creature class.
     */
    public class Player : Creature
    {
        private int maxHP;
        private int atk;
        public List<Item> inventory; // To keep track of the items in the player's inventory.

        // We call the base method in the abstract creature class, and we will set the
        // maxHP to the hp value.
        public Player(int hp, int atk, Node loc, Dungeon dungeon)
            : base(hp, atk, loc, dungeon)
        {
            this.maxHP = hp;
            this.atk = atk;
            this.Location = loc;
            loc.AddPlayer(this);
        }

        // We will move the player to the given destination.
        public override void Move(Node destination)
        {
            this.Location = destination;
            destination.CheckInCombat();
        }

        // The attacked creature is dealt damage equal to the player's attack power.
        public override void Attack(Creature creature)
        {
            creature.HP -= this.atk;
        }

        // If the health is below zero, the player will die.
        public override void Die()
        {
            if (this.IsDead)
                throw new NotImplementedException();
        }

        // This will add items to the player's inventory.
        public void GrabItems(Item item)
        {
            inventory.Add(item);
        }

        // We override the virtual method of the creature class here, and add a
        // testcase to make sure the hp value can't exceed the maximum value.
        public override int HP
        {
            get { return base.HP; }
            set
            {
                if (value > maxHP)
                    base.HP = maxHP;
                else
                    base.HP = value;
            }
        }

        public int MaxHP { get { return maxHP; } }

        public void GetCommand()
        {
            // TODO
        }
    }
}
