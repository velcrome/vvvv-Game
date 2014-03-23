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
	[PluginInfo(Name = "Kill", Category = "Game", Help = "Simply Kill", Tags = "")]
	#endregion PluginInfo
	public class GameDeathNode : AbstractActionNode
	{
		[Input("Kill", IsSingle = true, IsToggle = true)]
		public ISpread<bool> FEnable;

        [Input("ReturnCode", IsSingle = true)]
        public ISpread<ReturnCodeEnum> FSetCode;
        
        protected override void Behave(IEnumerable<IAgent> agents)
		{
			if (!FEnable[0]) return;
			
			foreach (var agent in agents) {
				agent.Killed = true;
                agent.ReturnCode = FSetCode[0];

			}
		}
	}
}
