using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;

namespace STV_1_Testing
{
    // This class is used to test for certain actions a player could make.
    [TestClass]
    public class BotPlayer
    {
        public Node node;
        public bool doMove;
        public bool doRetreat;
        public bool useItem;
        public bool useTC;
        public bool useHP;

        // Make the stubs that we can use for testing.
        [TestMethod]
        public void TestMethod1(bool doMove, bool doRetreat, bool useTC, bool useHP, Node node = null)
        {
            useItem = useTC || useHP;
            this.useTC = useTC;
            this.useHP = useHP;
            this.doMove = doMove;
            this.doRetreat = doRetreat;
            this.node = node;
        }
    }
}
