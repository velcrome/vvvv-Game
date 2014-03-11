using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;

namespace VVVV.Pack.Game.Core.Tests
{
    [TestClass]
    public class INamedAgentTest
    {
        [TestMethod]
        public void TestExtensionMethod()
        {
            var agent = new Agent();


            var named = agent.Face<INamedAgent>(true);
            Assert.AreEqual("vvvv", named.Name);

            var name = named.SetRandomName();
            Assert.AreEqual(name, named.Name);
        }
    }
}
