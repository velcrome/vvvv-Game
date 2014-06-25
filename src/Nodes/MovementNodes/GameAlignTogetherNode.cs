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
	[PluginInfo(Name = "AlignTogether", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameAlignTogetherNode : AbstractActionNode
	{
		[Input("Strength", DefaultValue = 1.0, AutoValidate = false)]
		protected Pin<double> FStrength;	
		
		protected override void Behave(IEnumerable<IAgent> agents)
		{
			FStrength.Sync();
			
			int i = 0;
			Vector3D heading = new Vector3D();
			foreach (var a in agents) {
				var agent = a.Face<IMoveableAgent>();
				heading += ~agent.Velocity;
				i++;
			}
			heading = heading / (double)i;
			
			i=0;
			foreach (var a in agents) {
				var agent = a.Face<IMoveableAgent>();

				agent.ForceSum += (heading - agent.Velocity) * FStrength[0];
				agent.ReturnCode = ReturnCodeEnum.Success; 
			}
		}
	}
}
