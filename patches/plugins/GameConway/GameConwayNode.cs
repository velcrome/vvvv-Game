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
	[PluginInfo(Name = "Conway", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameConwayNode : AbstractActionNode
	{
		[Input("Current Count", IsSingle = true, DefaultValue = 1, AutoValidate = false)]
		public Pin<int> FCurrentAgentCount;

		[Input("Max Count", IsSingle = true, DefaultValue = 100, AutoValidate = false)]
		public Pin<int> FMaxAgentCount;

		[Input("Starve", IsSingle = true, DefaultValue = 1, AutoValidate = false)]
		public Pin<int> FStarveCount;

		[Input("Feed", IsSingle = true, DefaultValue = 3, AutoValidate = false)]
		public Pin<int> FCloneCount;

		[Input("Radius", IsSingle = true, DefaultValue = 1.0, AutoValidate = false)]
		public Pin<double> FRadius;
		
		
		[Output("Cloned Agents", AutoFlush = false)]
		public Pin<Agent> FCloneOut;
		
		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FCurrentAgentCount.Sync();
			FMaxAgentCount.Sync();
			
			FStarveCount.Sync();
			FCloneCount.Sync();
			FRadius.Sync();
			
			
			FCloneOut.SliceCount = 0; 
			var diff = FMaxAgentCount[0] - FCurrentAgentCount[0];
			
			var starve = FStarveCount[0];
			var clone = FCloneCount[0];
			var radius = FRadius[0];
			
			foreach (dynamic agent in agents) { 
				var closeBy = 	from peer in agents
							  	where peer != agent
								where VMath.Dist((Vector3D)peer["Position"].First, (Vector3D)agent["Position"].First) < radius
								select peer;

				if (closeBy.Count() < starve) {
					agent.Feed(0.005);
					
					if (agent.Health < 0.1) {
						agent.Killed = true;
						agent.ReturnCode = ReturnCodeEnum.Failure;
					}
				}
				
				if ((closeBy.Count() >= clone)) {

					if (agent.Health >= 0.9) {
						if (diff > 0) {
							agent.Burn(0.5);
							FCloneOut.Add((Agent)agent.Clone());
							diff--;
						}
					} 
				}
				
				FCloneOut.Flush();
			}
			
			
			
		}
	}
}
