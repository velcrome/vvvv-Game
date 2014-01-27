using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game
{
    [TestClass]
    public class DynamicAgentTest
    {
        [TestMethod]
        public void TestDouble()
        {
            dynamic agent = new Agent();

            agent.TestDouble = new Bin<double>(1.0);

            Assert.AreEqual(1.0, agent.TestDouble[0], "Dynamic Agent double initialisation");

            agent.TestDouble.Add(2.0);
            Assert.AreEqual(2.0, agent.TestDouble[1], "Dynamic Agent double append");

            var l = new List<double> { 3.0, 4.0 };
            agent.TestDouble.Add(l);

            Assert.AreEqual(3.0, agent.TestDouble[2], "Dynamic Agent double add IEnumeration");
            Assert.AreEqual(4.0, agent.TestDouble[3], "Dynamic Agent double add IEnumeration");


            agent.TestDouble.First = 0.0;
            Assert.AreEqual(0.0, agent.TestDouble[0], "Dynamic Agent double set First");
            Assert.AreEqual(0.0, agent.TestDouble.First, "Dynamic Agent double get First");

            agent.TestDouble.AssignFrom(l);
            Assert.AreEqual(3.0, agent.TestDouble[0], "Dynamic Agent double assign IEnumeration");
            Assert.AreEqual(4.0, agent.TestDouble[1], "Dynamic Agent double assign IEnumeration");

            Assert.AreEqual(2, agent.TestDouble.Count, "Dynamic Agent double Count");
        }
        
        
        [TestMethod]
        public void TestInt()
        {
            dynamic agent = new Agent();

            agent.TestInt = new Bin<int>(1);
            Assert.AreEqual(1, agent.TestInt[0], "Dynamic Agent int initialisation");

            agent.TestInt.Add(2);
            Assert.AreEqual(2, agent.TestInt[1], "Dynamic Agent int append");

            var l = new List<int> { 3, 4 };
            agent.TestInt.Add(l);

            Assert.AreEqual(3, agent.TestInt[2], "Dynamic Agent int add IEnumeration");
            Assert.AreEqual(4, agent.TestInt[3], "Dynamic Agent int add IEnumeration");


            agent.TestInt.First = 0;
            Assert.AreEqual(0, agent.TestInt[0], "Dynamic Agent int set First");
            Assert.AreEqual(0, agent.TestInt.First, "Dynamic Agent int get First");

            agent.TestInt.AssignFrom(l);
            Assert.AreEqual(3, agent.TestInt[0], "Dynamic Agent int assign IEnumeration");
            Assert.AreEqual(4, agent.TestInt[1], "Dynamic Agent int assign IEnumeration");

            Assert.AreEqual(2, agent.TestInt.Count, "Dynamic Agent int Count");


            agent.Init<int>("SecondTestInt");
            agent.SecondTestInt.Add(1);
            Assert.AreEqual(1, agent.SecondTestInt.First);
        }
 
        [TestMethod]
        public void TestString()
        {
            dynamic agent = new Agent();

            agent.TestString = new Bin<string>("First");
            Assert.AreEqual("First", agent.TestString[0], "Dynamic Agent string initialisation");

            agent.TestString.Add("Second");
            Assert.AreEqual("Second", agent.TestString[1], "Dynamic Agent string append");

            var l = new List<string> { "Third", "Fourth" };
            agent.TestString.Add(l);

            Assert.AreEqual("Third", agent.TestString[2], "Dynamic Agent string add IEnumeration");
            Assert.AreEqual("Fourth", agent.TestString[3], "Dynamic Agent string add IEnumeration");


            agent.TestString.First = "Zero";
            Assert.AreEqual("Zero", agent.TestString[0], "Dynamic Agent string set First");
            Assert.AreEqual("Zero", agent.TestString.First, "Dynamic Agent string get First");

            agent.TestString.AssignFrom(l);
            Assert.AreEqual("Third", agent.TestString[0], "Dynamic Agent string assign IEnumeration");
            Assert.AreEqual("Fourth", agent.TestString[1], "Dynamic Agent string assign IEnumeration");

            Assert.AreEqual(2, agent.TestString.Count, "Dynamic Agent int Count");

        }

        [TestMethod]
        public void TestVector3D()
        {
            var v0 = new Vector3D(0, 0, 0);
            var v1 = new Vector3D(1, 1, 1);
            var v2 = new Vector3D(2, 2, 2);
            var v3 = new Vector3D(3, 3, 3);
            var v4 = new Vector3D(4, 4, 4);

            dynamic agent = new Agent();

            agent.TestVector3D = new Bin<Vector3D>(v1);
            Assert.AreEqual(v1, agent.TestVector3D[0], "Dynamic Agent string initialisation");

            agent.TestVector3D.Add(v2);
            Assert.AreEqual(v2, agent.TestVector3D[1], "Dynamic Agent string append");

            var l = new List<Vector3D> { v3, v4 };
            agent.TestVector3D.Add(l);

            Assert.AreEqual(v3, agent.TestVector3D[2], "Dynamic Agent string add IEnumeration");
            Assert.AreEqual(v4, agent.TestVector3D[3], "Dynamic Agent string add IEnumeration");


            agent.TestVector3D.First = v0;
            Assert.AreEqual(v0, agent.TestVector3D[0], "Dynamic Agent string set First");
            Assert.AreEqual(v0, agent.TestVector3D.First, "Dynamic Agent string get First");

            agent.TestVector3D.AssignFrom(l);
            Assert.AreEqual(v3, agent.TestVector3D[0], "Dynamic Agent string assign IEnumeration");
            Assert.AreEqual(v4, agent.TestVector3D[1], "Dynamic Agent string assign IEnumeration");

            Assert.AreEqual(2, agent.TestVector3D.Count, "Dynamic Agent int Count");
        }

    }
}
