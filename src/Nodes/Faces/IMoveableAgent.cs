using VVVV.Pack.Game.Core;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface IMoveableAgent : IAgent
    {
        Vector3D Position { get; set; }
        Vector3D Velocity { get; set; }

        Vector3D NextVelocity { get; set; }

    }
}
