using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    /*
     * This class will handle the actions for monsters.
     * Since the monster is a creature, it will inherit from
     * the abstract creature class.
     */
    public class Monster : Creature
    {
        private int hp;
        private int atk;

        // We just call the base method in the abstract creature class.
        public Monster(int hp, int atk, Node loc)
            : base(hp, atk, loc)
        {
            this.atk = atk;
            this.hp = hp;
            this.Location = loc;
        }

        // Move the monster to the given destination
        public override void Move(Node destination)
        {
            this.Location = destination;
            destination.CheckInCombat();
        }

        // The attacked creature is dealt damage equal to the monster's attack power.
        public override void Attack(Creature creature)
        {
            creature.HP -= this.atk;
        }
    }
}