using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game
{
    [TestClass]
    public class FaceAgentTest
    {
        [TestMethod]
        public void TestExtensionMethod()
        {
            var agent = new Agent().Face<INamedAgent>(true);
//            agent.SetRandomName();

        }


        [TestMethod]
        public void TestAccess()
        {
            var agent = new Agent().Face<ITestAgent>();
            agent["Access"] = new Bin<bool>();

            agent.Init("stream", typeof(Stream), false);
            try
            {
                var f = agent["stream"].First;
            }
            catch (Exception e)
            {
                Assert.Inconclusive();
            }

            Assert.AreEqual(false, agent["Access"].First);

            Assert.AreEqual(1, agent["Access"].Count);


            agent.Init("Secure", true);
            agent["Secure"].Add(true);

        
            Assert.AreEqual(true, agent["Secure"][1]);
            Assert.AreEqual(2, agent["Secure"].Count);
        }

        [TestMethod]
        public void TestInt()
        {
            var agent = new Agent().Face<ITestAgent>();
    //        agent.Init("SingleInt", typeof(int));
            agent.SingleInt++;
            Assert.AreEqual(1, agent["SingleInt"].First, "Face Agent int initialisation");
          
            agent.TestInt = new Bin<int>(1);
            Assert.AreEqual(1, agent.TestInt[0], "Face Agent int initialisation");

            agent.TestInt.Add(2);
            Assert.AreEqual(2, agent.TestInt[1], "Face Agent int append");

            var l = new List<int> {3, 4};
            agent.TestInt.Add(l);

            Assert.AreEqual(3, agent.TestInt[2], "Face Agent int add IEnumeration");
            Assert.AreEqual(4, agent.TestInt[3], "Face Agent int add IEnumeration");


            agent.TestInt.First = 0;
            Assert.AreEqual(0, agent.TestInt[0], "Face Agent int set First");
            Assert.AreEqual(0, agent.TestInt.First, "Face Agent int get First");

            agent.TestInt.AssignFrom(l);
            Assert.AreEqual(3, agent.TestInt[0], "Face Agent int assign IEnumeration");
            Assert.AreEqual(4, agent.TestInt[1], "Face Agent int assign IEnumeration");

           
            Assert.AreEqual(2, agent.TestInt.Count, "Face Agent int Count");
        }

        [TestMethod]
        public void TestDouble()
        {
            var agent = new Agent().Face<ITestAgent>();

            agent.TestDouble = new Bin<double>(1.0);

            Assert.AreEqual(1.0, agent.TestDouble[0], "Face Agent double initialisation");

            agent.TestDouble.Add(2.0);
            Assert.AreEqual(2.0, agent.TestDouble[1], "Face Agent double append");

            var l = new List<double> {3.0, 4.0};
            agent.TestDouble.Add(l);

            Assert.AreEqual(3.0, agent.TestDouble[2], "Face Agent double add IEnumeration");
            Assert.AreEqual(4.0, agent.TestDouble[3], "Face Agent double add IEnumeration");


            agent.TestDouble.First = 0.0;
            Assert.AreEqual(0.0, agent.TestDouble[0], "Face Agent double set First");
            Assert.AreEqual(0.0, agent.TestDouble.First, "Face Agent double get First");

            agent.TestDouble.AssignFrom(l);
            Assert.AreEqual(3.0, agent.TestDouble[0], "Face Agent double assign IEnumeration");
            Assert.AreEqual(4.0, agent.TestDouble[1], "Face Agent double assign IEnumeration");

            Assert.AreEqual(2, agent.TestDouble.Count, "Face Agent double Count");
        }

        [TestMethod]
        public void TestString()
        {
            var agent = new Agent().Face<ITestAgent>();

            agent.TestString = new Bin<string>("First");
            Assert.AreEqual("First", agent.TestString[0], "Face Agent string initialisation");

            agent.TestString.Add("Second");
            Assert.AreEqual("Second", agent.TestString[1], "Face Agent string append");

            var l = new List<string> {"Third", "Fourth"};
            agent.TestString.Add(l);

            Assert.AreEqual("Third", agent.TestString[2], "Face Agent string add IEnumeration");
            Assert.AreEqual("Fourth", agent.TestString[3], "Face Agent string add IEnumeration");


            agent.TestString.First = "Zero";
            Assert.AreEqual("Zero", agent.TestString[0], "Face Agent string set First");
            Assert.AreEqual("Zero", agent.TestString.First, "Face Agent string get First");

            agent.TestString.AssignFrom(l);
            Assert.AreEqual("Third", agent.TestString[0], "Face Agent string assign IEnumeration");
            Assert.AreEqual("Fourth", agent.TestString[1], "Face Agent string assign IEnumeration");

            Assert.AreEqual(2, agent.TestString.Count, "Face Agent int Count");

        }

        [TestMethod]
        public void TestVector3D()
        {
            var v0 = new Vector3D(0, 0, 0);
            var v1 = new Vector3D(1, 1, 1);
            var v2 = new Vector3D(2, 2, 2);
            var v3 = new Vector3D(3, 3, 3);
            var v4 = new Vector3D(4, 4, 4);

            var agent = new Agent().Face<ITestAgent>();

            agent.TestVector3D = new Bin<Vector3D>(v1);
            Assert.AreEqual(v1, agent.TestVector3D[0], "Face Agent string initialisation");

            agent.TestVector3D.Add(v2);
            Assert.AreEqual(v2, agent.TestVector3D[1], "Face Agent string append");

            var l = new List<Vector3D> {v3, v4};
            agent.TestVector3D.Add(l);

            Assert.AreEqual(v3, agent.TestVector3D[2], "Face Agent string add IEnumeration");
            Assert.AreEqual(v4, agent.TestVector3D[3], "Face Agent string add IEnumeration");


            agent.TestVector3D.First = v0;
            Assert.AreEqual(v0, agent.TestVector3D[0], "Face Agent string set First");
            Assert.AreEqual(v0, agent.TestVector3D.First, "Face Agent string get First");

            agent.TestVector3D.AssignFrom(l);
            Assert.AreEqual(v3, agent.TestVector3D[0], "Face Agent string assign IEnumeration");
            Assert.AreEqual(v4, agent.TestVector3D[1], "Face Agent string assign IEnumeration");

            Assert.AreEqual(2, agent.TestVector3D.Count, "Face Agent int Count");
        }

        [TestMethod]
        public void TestSingle()
        {
            var v0 = new Vector3D(0, 0, 0);
            var v1 = new Vector3D(1, 1, 1);

            var agent = new Agent().Face<ITestAgent>();

            agent.SingleInt = 1;
            Assert.AreEqual(1, agent.SingleInt, "Face Agent int");

            agent.SingleDouble = 1.0;
            Assert.AreEqual(1.0, agent.SingleDouble, "Face Agent double");

            agent.SingleString = "First";
            Assert.AreEqual("First", agent.SingleString, "Face Agent string");

            agent.SingleVector3D = v1;
            Assert.AreEqual(v1, agent.SingleVector3D, "Face Agent Vector3D");

            agent.SingleVector3D = v0;
            Assert.AreEqual(v0, agent.SingleVector3D, "Face Agent Vector3D");

        }




    }
}
