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
	[PluginInfo(Name = "Move", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameMoveNode : AbstractActionNode
	{
		[Input("Max Speed", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<double> FMaxSpeed;

		[Input("Agility", DefaultValue = 0.5, MinValue=0.0001, MaxValue=1.0, AutoValidate = false)]
		protected Pin<double> FAgility;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FMaxSpeed.Sync();
			FAgility.Sync();
			
			foreach (var a in agents) {
				IMoveableAgent agent = a.Face<IMoveableAgent>(true);
				
				agent.Velocity += FAgility[0] * (agent.ForceSum);
				if (VMath.Polar(agent.Velocity).z > FMaxSpeed[0]) {
					agent.Velocity = FMaxSpeed[0] * ~agent.Velocity;
				}
				
				agent.Position += agent.Velocity; 
				agent.ForceSum *= 0;
//				agent.ForceSum *= FAgility[0];
//				agent.ForceSum = agent.Velocity;
			}
		}
	}
}
