using VVVV.Pack.Game.Core;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface IComplexAgent : IAgent
    {
        Bin<string> TestString { get; set; }
        Bin<int> TestInt { get; set; }
        Bin<double> TestDouble { get; set; }
        Bin<Vector3D> TestVector3D { get; set; }

        string SingleString { get; set; }
        int SingleInt { get; set; }
        double SingleDouble { get; set; }
        Vector3D SingleVector3D { get; set; }
    }
}
