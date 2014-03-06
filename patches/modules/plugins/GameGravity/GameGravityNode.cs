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
	[PluginInfo(Name = "Gravity", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameGravityNode : AbstractActionNode
	{
		[Input("Center", DefaultValues = new double[] {0.0, 0.0, 0.0} ) ]
		protected Pin<Vector3D> FCenter;

		[Input("CenterMass", DefaultValue = 1.0 ) ]
		protected Pin<double> FCenterMass;		

		[Input("Constant", DefaultValue = 6.674E-11, Visibility = PinVisibility.OnlyInspector) ]
		protected Pin<double> FGravitationalConstant;		
		
		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0;
			FCenter.Sync();
			FCenterMass.Sync();

			foreach (var a in agents) {
				var agent = a.Face<IGravityAgent>();
				
				var distance = VMath.Dist(agent.Position, FCenter[i]);
				var otherMass = FCenterMass[i];				
				
				agent.ForceSum += FGravitationalConstant[0] * agent.Mass * otherMass / distance*distance; // one of Newtons Laws

				i++;
				agent.ReturnCode = ReturnCodeEnum.Success;

			}
		}
	}
	
	public interface IGravityAgent : IMoveableAgent{
		double Mass {get;set;}
		
	}
	
}
