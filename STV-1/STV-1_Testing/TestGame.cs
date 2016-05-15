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
            Game game = new Game();
        }
    }
}
