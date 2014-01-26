using VVVV.Pack.Game.Core;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface ITestAgent : IAgent
    {
        string Test { get; set; }
        Bin<string> Name { get; set; }
        Vector3D Position { get; set; }
        int Health { get; set; }
        Bin<RGBAColor> Palette { get; set; }
    }
}
