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
			foreach (var a in agents)
			{
			    var agent = a.Face<IMoveableAgent>();

			    var vectors = from peer in agents
                              where peer != agent
			                  where agent.Distance(peer) < FMaxRadius[i] 
			                  select agent.Vector(peer);

				
				var sum = new Vector3D();
				foreach  (var v in vectors)
				{
				    var dist = (FMaxRadius[i] - v.Length)/FMaxRadius[i];
                    sum += (1 - dist) * ~v;
				}
				agent.ForceSum += sum * FStrength[i];
				
				i++;
			}
		}
	}
}
