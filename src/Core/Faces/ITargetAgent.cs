using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface ITargetAgent : IMoveableAgent
    {
        Vector3D Target { get; set; }
        Vector3D Rotation { get; set; }

        Vector3D AimAtTarget(double targetFocus = 1.0, double agility = 1.0);
    }

    public static partial class AgentAPI
    {
        public static Vector3D AimAtTarget(this IAgent agent, double targetFocus = 1.0, double agility = 1.0 )
        {
            var a = agent.Face<ITargetAgent>();

            var rotation = VMath.Slerp(a.Velocity, a.Target, targetFocus);
            a.Rotation = VMath.Slerp(a.Rotation, rotation, agility);

            return rotation;
        }

        public static void MoveToTarget(this IAgent agent, double speed = 1.0)
        {
            var a = agent.Face<ITargetAgent>();
            a.ForceSum += speed*~a.Rotation;
        }

    }
}
