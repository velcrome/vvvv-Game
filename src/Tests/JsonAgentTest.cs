using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VVVV.Pack.Game.Core;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game
{
    [TestClass]
    public class JsonAgentTest
    {

        [TestMethod]
        public void TestToString()
        {
            var agent = new Agent();

            agent.Add("Test", "foo");
            agent["Test"].Add("bar");

            agent["Path"] = new Bin<Vector3D>(new Vector3D(), new Vector3D(1, 1, 1));

            var s = agent.ToString();
            s += "";
            Assert.AreEqual("Agent\n Test (string) \t: foo[string], bar[string], \r\n Path (vector3d) \t: VVVV.Utils.VMath.Vector3D[vector3d], VVVV.Utils.VMath.Vector3D[vector3d], \r\n", s);
        }

        [TestMethod]
        public void TestJson()
        {
            var agent = new Agent();

            agent.Add("Test", "foo");
            agent["Test"].Add("bar");

            agent["Path"] = new Bin<Vector3D>(new Vector3D(), new Vector3D(1,1,1));

            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.None;

           string s = JsonConvert.SerializeObject(agent, settings);


            Agent copy = JsonConvert.DeserializeObject<Agent>(s);

            Assert.AreEqual("foo", copy["Test"].First);
            Assert.AreEqual("bar", copy["Test"][1]);

            Assert.AreEqual(0.0, ((Vector3D)copy["Path"].First).x);
            Assert.AreEqual(1.0, ((Vector3D)copy["Path"][1]).x);

            // does not look pretty yet. waiting for beta32
            Assert.AreEqual("{\r\n  \"Data\": {\r\n    \"Test\": {\r\n      \"Type\": \"string\",\r\n      \"Bin\": [\r\n        \"foo\",\r\n        \"bar\"\r\n      ]\r\n    },\r\n    \"Path\": {\r\n      \"Type\": \"vector3d\",\r\n      \"Bin\": [\r\n        {\r\n          \"x\": 0.0,\r\n          \"y\": 0.0,\r\n          \"z\": 0.0\r\n        },\r\n        {\r\n          \"x\": 1.0,\r\n          \"y\": 1.0,\r\n          \"z\": 1.0\r\n        }\r\n      ]\r\n    }\r\n  }\r\n}", s, "Json failed");
            Assert.Inconclusive();
        }
    }
}
