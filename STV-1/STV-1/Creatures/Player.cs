using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Player : Creature
{
    int MaxHP;
    int atk;
    List<Item> inventory;

    public Player(int hitPoints, int attack, Node loc)
        :base(hitPoints, attack, loc)
    {
        MaxHP = hitPoints;
        atk = attack;
    }

    public override void Move(Node destination)
    {
        throw new NotImplementedException();
    }

    public override void Attack(Creature creature)
    {
        creature.HP -= atk;
    }

    public override void Die()
    {
        throw new NotImplementedException();
    }

    public void GrabItems(Item item)
    {
        inventory.Add(item);
    }

    public override int HP
    {
        get { return base.HP; }
        set
        {
            if (value > MaxHP)
                base.HP = MaxHP;
            else
                base.HP = value;
        }
    }
}
