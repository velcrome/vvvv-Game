using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface IMoveableAgent : IAgent
    {
        Vector3D Position { get; set; } // Last Frame Position
        Vector3D Velocity { get; set; } // Last Frame Velocity

        Vector3D ForceSum { get; set; } // Sum of all Forces acting on the Agent

        Vector3D SetRandomPosition(double width); 

    }

    public static partial class AgentAPI
    {
        public static Vector3D SetRandomPosition(this IAgent agent, double width)
        {
            var v3 = VMath.RandomVector3D()*width;
            agent["Position"].First = v3;
            return v3;
        }
    }
}
