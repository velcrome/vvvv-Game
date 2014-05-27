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
	public class GameIsOutsideBoxNode : AbstractConditionNode
	{
		[Input("Center", AutoValidate = false, IsSingle = true)]
		protected Pin<Vector3D> FCenter;

		[Input("Width", AutoValidate = false, DefaultValues = new double[] {1.0, 1.0, 1.0}, IsSingle = true)]
		protected Pin<Vector3D> FWidth;

		public override void After(IEnumerable<IAgent> agents)
		{
		}

	    public override bool Condition(IAgent agent)
	    {
            var a = agent.Face<IMoveableAgent>();

            var center = FCenter[0];
            var width = FWidth[0] / 2;

            bool outside = false;
            if (isOutside(a.Position.x, center.x, width.x)) outside = true;
            if (isOutside(a.Position.y, center.y, width.y)) outside = true;
            if (isOutside(a.Position.z, center.z, width.z)) outside = true;

	        return outside;
	    }

	    public override void Before(IEnumerable<IAgent> agents)
		{
			FCenter.Sync();
			FWidth.Sync();

		}
		
		private bool isOutside(double input, double center, double width) {
			width = VMath.Abs(width);			
			return (input < center-width || input > center+width);
		}
	}
}
