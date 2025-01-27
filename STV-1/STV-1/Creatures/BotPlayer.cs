﻿/*
 * This stub class is used to simulate the behaviour of a player.
 * We will use these booleans in a queue, and with that queue we can
 * control the actions of a player. In this way we can conduct various
 * tests on combat and other things.
 */

namespace STV1
{
    public class BotPlayer
    {
        public bool itemUsed, usedHP, usedTC;
        public bool moved, retreated;

        public BotPlayer(bool HP, bool TC, bool move, bool retreat)
        {
            usedHP = HP;
            usedTC = TC;
            moved = move;
            retreated = retreat;

            itemUsed = HP || TC; // An item is used when an HP or TC item is used.
        }
    }
}
