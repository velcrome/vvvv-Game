#region usings
using System.Collections.Generic;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.Pack.Game;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

#endregion usings

namespace VVVV.Pack.Game.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Hunting", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameHuntingNode : AbstractDecoratorNode
	{
		[Input("Radius", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<double> FRadius;

		[Input("Calories", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<double> FCalories;
		
		[Input("Hunters", DefaultValue = 0.1)]
		protected Pin<Agent> FHunter;

		public override void After(IEnumerable<IAgent> agents)
		{

		}

		public override void Before(IEnumerable<IAgent> agents)
		{
			FRadius.Sync();
			FCalories.Sync();
			
			int i = 0;
			if ((FHunter.SliceCount == 0) || (FHunter[0] == null)) return; 
			
			
			foreach (dynamic agent in agents) {
				
				var hunters = 	from hunter in FHunter
								where (double)hunter["Health"].First < 0.5
								where agent.Distance(hunter) <= FRadius[i]
								select hunter;
				
			
				foreach (var hunter in hunters.Take(1)) {
					hunter.Feed(FCalories[i] * (double)agent.Health);
					agent.Killed = true;

				}
				agent.ReturnCode = ReturnCodeEnum.Success;
				
				i++;
			}
		}
	}
}
