using System;
using System.Collections.Generic;
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
        public void TestInt()
        {
            var agent = new Agent();

            agent.Add("Test", 1);
            Assert.AreEqual(1, agent["Test"][0], "Vanilla Agent int initialisation");

            agent["Test"].Add(2);
            Assert.AreEqual(2, agent["Test"][1], "Vanilla Agent int append");

            var l = new List<int> { 3, 4 };
            agent["Test"].Add(l);

            Assert.AreEqual(3, agent["Test"][2], "Vanilla Agent int add IEnumeration");
            Assert.AreEqual(4, agent["Test"][3], "Vanilla Agent int add IEnumeration");


            agent["Test"].First =0;
            Assert.AreEqual(0, agent["Test"][0], "Vanilla Agent int set First");
            Assert.AreEqual(0, agent["Test"].First, "Vanilla Agent int get First");

            agent["Test"].AssignFrom(l);
            Assert.AreEqual(3, agent["Test"][0], "Vanilla Agent int assign IEnumeration");
            Assert.AreEqual(4, agent["Test"][1], "Vanilla Agent int assign IEnumeration");

            Assert.AreEqual(2, agent["Test"].Count, "Vanilla Agent int Count");


            agent.Init<int>("SecondTestInt");
            agent["SecondTestInt"].Add(1);
            Assert.AreEqual(1, agent["SecondTestInt"].First);
        }

        [TestMethod]
        public void TestDouble()
        {
            var agent = new Agent();

            agent.Add("Test", 1.0);
            Assert.AreEqual(1.0, agent["Test"][0], "Vanilla Agent double initialisation");

            agent["Test"].Add(2.0);
            Assert.AreEqual(2.0, agent["Test"][1], "Vanilla Agent double append");

            var l = new List<double> { 3.0, 4.0 };
            agent["Test"].Add(l);

            Assert.AreEqual(3.0, agent["Test"][2], "Vanilla Agent double add IEnumeration");
            Assert.AreEqual(4.0, agent["Test"][3], "Vanilla Agent double add IEnumeration");


            agent["Test"].First = 0.0;
            Assert.AreEqual(0.0, agent["Test"][0], "Vanilla Agent double set First");
            Assert.AreEqual(0.0, agent["Test"].First, "Vanilla Agent double get First");

            agent["Test"].AssignFrom(l);
            Assert.AreEqual(3.0, agent["Test"][0], "Vanilla Agent double assign IEnumeration");
            Assert.AreEqual(4.0, agent["Test"][1], "Vanilla Agent double assign IEnumeration");

            Assert.AreEqual(2, agent["Test"].Count, "Vanilla Agent double Count");
        }

        [TestMethod]
        public void TestString()
        {
            var agent = new Agent();

            agent.Add("Test", "First");
            Assert.AreEqual("First", agent["Test"][0], "Vanilla Agent string initialisation");

            agent["Test"].Add("Second");
            Assert.AreEqual("Second", agent["Test"][1], "Vanilla Agent string append");

            var l = new List<string> {"Third", "Fourth"};
            agent["Test"].Add(l);

            Assert.AreEqual("Third", agent["Test"][2], "Vanilla Agent string add IEnumeration");
            Assert.AreEqual("Fourth", agent["Test"][3], "Vanilla Agent string add IEnumeration");


            agent["Test"].First = "Zero";
            Assert.AreEqual("Zero", agent["Test"][0], "Vanilla Agent string set First");
            Assert.AreEqual("Zero", agent["Test"].First, "Vanilla Agent string get First");

            agent["Test"].AssignFrom(l);
            Assert.AreEqual("Third", agent["Test"][0], "Vanilla Agent string assign IEnumeration");
            Assert.AreEqual("Fourth", agent["Test"][1], "Vanilla Agent string assign IEnumeration");

            Assert.AreEqual(2, agent["Test"].Count, "Vanilla Agent int Count");

        }

        [TestMethod]
        public void TestVector3D()
        {
            var v0 = new Vector3D(0, 0, 0);
            var v1 = new Vector3D(1, 1, 1);
            var v2 = new Vector3D(2, 2, 2);
            var v3 = new Vector3D(3, 3, 3);
            var v4 = new Vector3D(4, 4, 4);
            
            
            var agent = new Agent();

            agent.Add("Test",v1);
            Assert.AreEqual(v1, agent["Test"][0], "Vanilla Agent Vector3D initialisation");

            agent["Test"].Add(v2);
            Assert.AreEqual(v2, agent["Test"][1], "Vanilla Agent Vector3D append");

            var l = new List<Vector3D> { v3, v4 };
            agent["Test"].Add(l);

            Assert.AreEqual(v3, agent["Test"][2], "Vanilla Agent Vector3D add IEnumeration");
            Assert.AreEqual(v4, agent["Test"][3], "Vanilla Agent Vector3D add IEnumeration");


            agent["Test"].First = v0;
            Assert.AreEqual(v0, agent["Test"][0], "Vanilla Agent Vector3D set First");
            Assert.AreEqual(v0, agent["Test"].First, "Vanilla Agent Vector3D get First");

            agent["Test"].AssignFrom(l);
            Assert.AreEqual(v3, agent["Test"][0], "Vanilla Agent Vector3D assign IEnumeration");
            Assert.AreEqual(v4, agent["Test"][1], "Vanilla Agent Vector3D assign IEnumeration");

            Assert.AreEqual(2, agent["Test"].Count, "Vanilla Agent Vector3D Count");
        }

    }
}
