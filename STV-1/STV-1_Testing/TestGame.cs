using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    [TestClass]
    public class TestGame
    {
        [TestMethod]
        public void TestGameConstructor()
        {
            // When we create a new game, the level should be 1
            Game game = new Game();
            Assert.IsTrue(game.d.level == 1);

            // When we move to the next dungeon, level should be 2
            game.NextDungeon();
            Assert.IsTrue(game.d.level == 2);
        }

        [TestMethod]
        public void TestBotPlayer()
        {
            BotPlayer bot = new BotPlayer(true,true,true,true);
            Assert.IsTrue(bot.usedTC);
            // Secret code coverage test method
        }
    }
}
