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
			foreach (var a in agents) {

				var agent = a.Face<IMoveableAgent>(true);
				agent.ReturnCode = ReturnCodeEnum.Success; 
	
				int j=0;
				foreach  (var a2 in agents)
            	{
					var agent2 = a2.Face<IMoveableAgent>(true);
            		
            		if (i != j) // if its the same agent dont do it
            		{
            			Vector3D pos1 = agent.Position;
            			Vector3D pos2 =agent2.Position;
            			
            			double dist = VMath.Dist(pos1, pos2);
            			if ( dist<FMaxRadius[0] ) //if I'm within range
            				agent.ForceSum += FStrength[0]*(pos1 - pos2);
            		}
            		j++;
                }
				i++;
			}
		}
	}
}
