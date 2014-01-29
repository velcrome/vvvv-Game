using VVVV.Pack.Game.Core;

namespace VVVV.Pack.Game.Faces
{
    public interface INamedAgent : IAgent
    {
        string Name { get; set; }
        string SetRandomName();
    }
}
namespace VVVV.Pack.Game.Faces
{
    public static class API
    {
        public static string SetRandomName(this AbstractGameNode node, INamedAgent agent)
        {
            var str = "random";
            agent.Init("name", str);
            return str;

        }
    }   
}
