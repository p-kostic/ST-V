using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class Creature
{
    int hp;
    int atk;
    bool isDead = false;
    Node location;

    public Creature(int hitPoints, int attack, Node loc)
    {
        hp = hitPoints;
        atk = attack;
        location = loc;
    }

    public abstract void Move(Node destination);
    public abstract void Attack(Creature creature);
    public abstract void Die();

    public Node Location
    {
        get { return location; }
        set { location = value; }
    }

    public virtual int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
                Die();
        }
    }
}