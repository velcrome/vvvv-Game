using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;

namespace VVVV.Pack.Game
{
    [TestClass]
    public class IAgentTest
    {
        [TestMethod]
        public void TestKill()
        {
            var agent = new Agent();

            Assert.AreEqual(false, agent.Killed);
            agent.Killed = true;

            Assert.AreEqual(true, agent.Killed);
        }
    }
}
