/*
 * This abstract class is used to define all the creatures in our game.
 * It consists of some basic methods and variables which are needed for
 * the creatures to function properly. It also handles a few test cases,
 * like dying for example.
 */

namespace STV1
{
    public abstract class Creature
    {
        int hp;
        int atk;
        bool isDead = false;
        Node location;
        protected Dungeon dungeon;

        // The constructor. This will be overridden by the inheriting class.
        public Creature(int hitPoints, int attack, Node loc, Dungeon dungeon)
        {
            hp = hitPoints;
            atk = attack;
            location = loc;
            this.dungeon = dungeon;
        }

        // Some abstract methods are defined here to make sure that we add them to the 
        // sub classes that will inherit this class.
        public abstract void Move(Node destination);
        public abstract void Attack(Creature creature);
        public abstract void Die();

        // A few getters/setters that will make sure we can get to our variables. In the
        // hp getter/setter we will also define when a creature has died.
        public virtual int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0)
                    isDead = true;
            }
        }

        public Node Location
        {
            get { return location; }
            set { location = value; }
        }

        public bool IsDead { get { return isDead; } }

        public virtual int ATK
        {
            get { return atk; }
            set { atk = value; }
        }

        public Dungeon Dungeon { get { return dungeon; } }
    }
}