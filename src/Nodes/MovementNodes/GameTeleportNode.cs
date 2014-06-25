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
	[PluginInfo(Name = "TeleportBy", Category = "Game", Help = "Move the current position by a vector.", Tags = "")]
	#endregion PluginInfo
	public class GameTeleportNode : AbstractActionNode
	{
		[Input("Offset", AutoValidate = false)]
		protected Pin<Vector3D> FTeleportBy;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FTeleportBy.Sync();

			int i =0;
            foreach (dynamic agent in agents)
            {
                agent.Position += FTeleportBy[i];
                i++;
            }
		}
	}

    #region PluginInfo
    [PluginInfo(Name = "TeleportTo", Category = "Game", Help = "Move the current position to a vector", Tags = "")]
    #endregion PluginInfo
    public class GameTeleportToNode : AbstractActionNode
    {
        [Input("Input", AutoValidate = false)]
        protected Pin<Vector3D> FTeleportBy;

        protected override void Behave(IEnumerable<IAgent> agents)
        {
            FTeleportBy.Sync();

            int i = 0;
            foreach (dynamic agent in agents)
            {
                agent.Position = FTeleportBy[i];
                i++;
            }
        }
    }
}
