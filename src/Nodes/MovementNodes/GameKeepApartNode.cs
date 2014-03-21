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
	[PluginInfo(Name = "KeepApart", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameKeepApartNode : AbstractActionNode
	{
		[Input("Strength", DefaultValue = 0.5)]
		protected ISpread<double> FStrength;		

		[Input("Max Radius", DefaultValue = 1.0)]
		protected ISpread<double> FMaxRadius;		

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0; 
			foreach (dynamic agent in agents) {
				Vector3D pos = agent.Position.First;  
				var forces = 	(IEnumerable<Vector3D>)from agent2 in agents 
								where VMath.Dist(pos, agent2.PositionHistory(0)) < FMaxRadius[i]
								select pos - agent2.PositionHistory(0);
				
				var sum = new Vector3D();
				foreach  (var force in forces)
            	{
       				sum += force;
                }
				agent.ForceSum.First += sum * FStrength[i];
				
				i++;
			}
		}
	}
}
