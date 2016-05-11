using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class Creature
{
    public int HP;
    public int ATK;

    public abstract void Attack(Creature creature);
    public abstract void Move();
    public abstract void Die();
}