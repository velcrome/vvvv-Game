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
	[PluginInfo(Name = "IsOutsideBox", Category = "Game", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class GameIsOutsideBoxNode : AbstractDecoratorNode
	{
		[Input("Center", AutoValidate = false)]
		protected Pin<Vector3D> FCenter;

		[Input("Width", AutoValidate = false, DefaultValues = new double[] {1.0, 1.0, 1.0})]
		protected Pin<Vector3D> FWidth;

		public override void After(IEnumerable<IAgent> agents)
		{
		}
		
		public override void Before(IEnumerable<IAgent> agents)
		{
			int i = 0;
			FCenter.Sync();
			FWidth.Sync();

			foreach (var a in agents) {

				var agent = a.Face<IMoveableAgent>();
				
				var center = FCenter[i];
				var width = FWidth[i] /2;
				
				bool outside = false;
				if (isOutside(agent.Position.x, center.x, width.x)) outside = true;
				if (isOutside(agent.Position.y, center.y, width.y)) outside = true;
				if (isOutside(agent.Position.z, center.z, width.z)) outside = true;
				
				i++;

				if (!outside) agent.ReturnCode = ReturnCodeEnum.Failure; // will interrupt further evaluation upstream.

			}
		}
		
		private bool isOutside(double input, double center, double width) {
			width = VMath.Abs(width);			
			return (input < center-width || input > center+width);
		}
	}
}
