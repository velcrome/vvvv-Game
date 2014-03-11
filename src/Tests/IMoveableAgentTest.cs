using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Core.Tests
{
    [TestClass]
    public class IMoveableAgentTest
    {
        [TestMethod]
        public void TestExtensionMethodWithParameter()
        {
            var agent = new Agent();

            var positioned = agent.Face<IMoveableAgent>(true);
            Assert.AreEqual(new Vector3D(), positioned.Position);

            var v3 = positioned.SetRandomPosition(10.0);
            Assert.AreEqual(v3, positioned.Position);
        }
    }
}
