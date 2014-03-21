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
	[PluginInfo(Name = "Cohesion", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameCohesionNode : AbstractActionNode
	{
		[Input("Strength", DefaultValue = 1.0)]
		protected ISpread<double> FStrength;		

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0;
			Vector3D center = new Vector3D();
			foreach (var a in agents) {
				var agent = a.Face<IMoveableAgent>();
				center += agent.Position;
				i++;
			}
			center = center / (double)i;
			
			i=0;
			foreach (var a in agents) {
				var agent = a.Face<IMoveableAgent>();
				agent.ForceSum += (center - agent.Position) * FStrength[0];
				
				agent.ReturnCode = ReturnCodeEnum.Success;

			}
		}
	}
}
