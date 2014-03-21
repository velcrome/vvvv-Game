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
	[PluginInfo(Name = "TeleportBy", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameTeleportNode : AbstractActionNode
	{
		[Input("Offset", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<Vector3D> FTeleportBy;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FTeleportBy.Sync();

			int i =0;
			foreach (var a in agents) {
				IMoveableAgent agent = a.Face<IMoveableAgent>(true);
				agent.Position += FTeleportBy[i];
				i++;
			}
		}
	}
}
