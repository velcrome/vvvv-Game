using ImpromptuInterface.Dynamic;
using ImpromptuInterface.InvokeExt;
using VVVV.Pack.Game.Core;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


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
        public static string SetRandomName(this Agent agent)
        {
            var str = "random";
            agent.Init("name", str);
            return str;

        }
    }   
}
