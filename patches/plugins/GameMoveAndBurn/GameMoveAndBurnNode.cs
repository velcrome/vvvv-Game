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
	[PluginInfo(Name = "BurnCalories", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameBurnNode : AbstractDecoratorNode
	{
		[Input("Efficiency", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<double> FEfficiency;

		public override void After(IEnumerable<IAgent> agents)
		{
			int i = 0;
			foreach (var a in agents) {

				var agent = a.Face<IFoodAgent>(false);
				agent.Work(FEfficiency[i]);
				agent.Digest();
				i++;
			}
		}
		
		public override void Before(IEnumerable<IAgent> agents)
		{
			FEfficiency.Sync();
		}
	}
}
