#region usings
using System.Collections.Generic;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

#endregion usings

namespace VVVV.Pack.Game.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "ContainInBox", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameContainInBoxNode : AbstractActionNode
	{
		[Input("Minimum", DefaultValues = new double[] {-1,-1,-1}, AutoValidate = false)]
		protected Pin<Vector3D> FMin;

		[Input("Maximum", DefaultValues = new double[] {1,1,1}, AutoValidate = false)]
		protected Pin<Vector3D> FMax;

		[Input("Mapping XYZ", AutoValidate = false)]
		protected Pin<TMapMode> FMode;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FMin.Sync();
			FMax.Sync();
			FMode.Sync();

			int i = 0;
			foreach (var a in agents) {
	//			IMoveableAgent agent = a.Face<IMoveableAgent>(false);
				
				var min = FMin[i];
				var max = FMax[i];
				
				var	pos = (Vector3D)a["Position"].First;
//				var	pos = agent.Position;
				
				if (pos.x < min.x || pos.x > max.x) pos.x = VMath.Map(pos.x, min.x, max.x, min.x, max.x, FMode[i*3]);
				if (pos.y < min.y || pos.y > max.y) pos.y = VMath.Map(pos.y, min.y, max.y, min.y, max.y, FMode[i*3+1]);
				if (pos.z < min.z || pos.z > max.z) pos.z = VMath.Map(pos.z, min.z, max.z, min.z, max.z, FMode[i*3+2]);
				
				a["Position"].First = pos;
//				agent.Position = pos;
				
				i++;
			}
		}
	}
}
