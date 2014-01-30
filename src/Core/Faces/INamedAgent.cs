using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Faces
{
    public interface INamedAgent : IAgent
    {
        string Name { get; set; }
        string SetRandomName();
    }

    public static partial class AgentAPI
    {
        public static string SetRandomName(this IAgent agent)
        {
            if (!randomized)
            {
                randomized = true;
                allNames.AddRange( new string[] {"Marko", "Sebl" });
            }
            var str = allNames[VMath.Random.Next(allNames.Count)];
            agent["Name"].First = str;
            return str;

        }

        private static List<string> allNames = new List<string>();
        private static bool randomized = false;
    }   
}
