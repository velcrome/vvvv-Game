using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;

namespace VVVV.Pack.Game.Core.Tests
{
    [TestClass]
    public class ILiveCycleAgentTest
    {
        [TestMethod]
        public void TestLifeCycle()
        {
            var agent = new Agent().Face<ILifeCycleAgent>();

            agent.SetStatus(LifeStatus.Fledging);
            Assert.AreEqual(LifeStatus.Fledging, agent.GetStatus());

            agent.SetStatus(LifeStatus.Alive);
            Assert.AreEqual(LifeStatus.Alive, agent.GetStatus());

            agent.SetStatus(LifeStatus.Dying);
            Assert.AreEqual(LifeStatus.Dying, agent.GetStatus());
        }

        [TestMethod]
        public void TestLifeTime()
        {
            var agent = new Agent().Face<ILifeCycleAgent>();
            Assert.AreEqual(0, agent.TotalLifeTime().Minutes);
            Assert.AreEqual(0, agent.RestLifeTime().Minutes);
            Assert.AreEqual(1, agent.LifeTimeProgress(), 0.01);

            agent.TimeOfDeath = DateTime.Now + TimeSpan.FromMinutes(3.1);
            Assert.AreEqual(3, agent.TotalLifeTime().Minutes);
            Assert.AreEqual(3, agent.RestLifeTime().Minutes);
            Assert.AreEqual(0, agent.LifeTimeProgress(), 0.01);
        }
    }
}
