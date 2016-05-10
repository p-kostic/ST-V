using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Pack
{
    List<Creature> monsters;
    Node curPos;

    Pack(int number) {
        for (int i = 0; i < number; i++) {
            monsters.Add(new Creature());
        }
    }

    void Move() {
        // TODO
    }

    void Attack() {
        // TODO
    }
}
