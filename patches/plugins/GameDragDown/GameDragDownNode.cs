#region usings
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
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
	[PluginInfo(Name = "DragDown", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameDragDownNode : AbstractActionNode
	{
		[Input("DownDirection", DefaultValues = new double[] {0,-1,0}, AutoValidate = false)]
		protected Pin<Vector3D> FDownDirection;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0;
			FDownDirection.Sync();

			foreach (var a in agents) {
				dynamic agent = a;
				agent.ForceSum.First += FDownDirection[i];

				i++;
				a.ReturnCode = ReturnCodeEnum.Success;

			}
		}
	}
}
