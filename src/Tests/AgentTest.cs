using System;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Faces;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Core.Tests
{
    [TestClass]
    public class AgentTest
    {
        [TestMethod]
        public void TestAgentClass()
        {

            var agent = new Agent();

            agent.Add("Test", "HelloWorld");
            Assert.AreEqual("HelloWorld", agent["Test"][0], "HelloWorld");



        }



        [TestMethod]
        public void TestAgentDynamic()
        {

            var agent = new Agent();
            Assert.IsInstanceOfType(agent, typeof(DynamicObject), "not inherited");

            agent.Add("Test", "HelloWorld");
            Assert.AreEqual("HelloWorld", agent["Test"][0], "HelloWorld");



        }

        [TestMethod]
        public void TestAgentFace()
        {

            var agent = new Agent();
            var cast = agent.Face<ITestAgent>();

            cast.Test = "HelloWorld";
            Assert.AreEqual("HelloWorld", cast.Test, "Implicit Cast to int");

            cast.Name = new Core.Bin<string>("Marko", "Ritter");
            cast.Name = (Core.Bin<string>)new string[] { "Marko", "Ritter" };
            Assert.AreEqual("Marko", cast.Name[0]);

            cast.Health = 77;
            cast.Health++;
            Assert.AreEqual(78, cast.Health, "Direct Cast");

            
            cast.Palette = new Core.Bin<RGBAColor>(new RGBAColor(1, 0, 0, 1), new RGBAColor(0, 1, 0, 1));
            cast.Position = new Vector3D();

            
            Assert.Fail("booboo");
 
        }
    }
}
