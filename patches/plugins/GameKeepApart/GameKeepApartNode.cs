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
		[Input("ReturnCode")]
		public ISpread<ReturnCodeEnum> FSetCode;

		protected override void Behave(IEnumerable<IAgent> agents)
		{
			int i = 0;
			foreach (var agent in agents) {
				var code = FSetCode[i];
				agent.ReturnCode = code;

				var f = agent.Face<INamedAgent>(true);
				var k = agent.Face<IComplexAgent>(true);

				//          f.Position += new Vector3D(1,1,1);
				//      FLogger.Log(LogType.Message, f.Position.ToString());
				//          f.Position += new Vector3D();
				i++;
			}
		}
	}
}
