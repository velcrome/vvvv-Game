#region usings
using System.Collections.Generic;
using System.Linq;
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
	[PluginInfo(Name = "IsFoodStatus", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameIsFoodStatusNode : AbstractDecoratorNode
	{
		[Input("FoodStatus", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<FoodStatus> FStatus;

		public override void After(IEnumerable<IAgent> agents)
		{

		}

		public override void Before(IEnumerable<IAgent> agents)
		{
			FStatus.Sync();

			int i = 0;
			foreach (var agent in agents) {
				if ((string)agent["FoodStatus"].First == FStatus[i].ToString())
					agent.ReturnCode = ReturnCodeEnum.Success;
					else agent.ReturnCode = ReturnCodeEnum.Failure;
			
				i++;
			}
		}
	}
}
