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

        [TestMethod]
        public void TestHistory()
        {
            var agent = new Agent().Face<IMoveableAgent>(true);
;
            agent.HistoryCount = 5;

            double force = 1.1;

            var count = agent.HistoryCount;
            for (var i=0;i<count-1;i++)
            {
                agent.ForceSum = new Vector3D(force, 0, 0);

                agent.Move();
                force *= force;
            }

            Assert.AreEqual(agent["Position"].Count, 5);

            Bin<Vector3D> history = agent.PositionHistory();
            var h = history[0];

            Assert.AreEqual(agent.PositionHistory(4).x, 0.0);
            Assert.AreEqual(agent.PositionHistory(3).x, 1.1);
            Assert.AreEqual(agent.PositionHistory(2).x, 3.41, 0.1);
            Assert.AreEqual(agent.PositionHistory(1).x, 7.1841, 0.1);
            Assert.AreEqual(agent.Position.x, 13.10178881, 0.1);


            agent.Move();
            Assert.AreEqual(agent.PositionHistory(4).x, 1.1);

            
        }
    }
}
