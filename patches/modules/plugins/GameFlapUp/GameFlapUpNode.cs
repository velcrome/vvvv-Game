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
	[PluginInfo(Name = "FlapUp", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameFlapUpNode : AbstractActionNode
	{
		[Input("Flap", DefaultValue = 0.0, IsBang = true, AutoValidate = false)]
		protected Pin<bool> FPushFlap;

		[Input("Strength", DefaultValue = 0.1, AutoValidate = false)]
		protected Pin<double> FStrength;

		private Vector3D FlapDirection = new Vector3D(0,1,0);
		
		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0;
			FPushFlap.Sync();
			FStrength.Sync();
			
			

			foreach (var a in agents) {
				
				var agent = a.Face<IMoveableAgent>(); 
				if (FPushFlap[i]) agent.ForceSum += FlapDirection * FStrength[i];
				

				i++;
				agent.ReturnCode = ReturnCodeEnum.Success;

			}
		}
	}
}
