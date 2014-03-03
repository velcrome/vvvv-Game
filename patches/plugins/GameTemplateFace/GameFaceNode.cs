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
// 1. search and replace all occurances of "TemplateFace" with "YourName"
// 2. edit the interface
// 3. edit methods in the partial class AgentAPI
// 4. edit the Behave() of the Plugin accordingly

	#region PluginInfo
	[PluginInfo(Name = "TemplateFace", Category = "Game", Help = "Basic template with one value in/out", Tags = "ITemplateFaceAgent")]
	#endregion PluginInfo
	public class GameTemplateFaceAgentNode : AbstractActionNode
	{
		protected override void Behave(IEnumerable<IAgent> agents)
		{
			foreach (var a in agents) {
				var agent = a.Face<ITemplateFaceAgent>();
				agent.SetFoo(0.54);
				agent.ReturnCode = ReturnCodeEnum.Success;

			}
		}
	}
	
	public interface ITemplateFaceAgent: IAgent{
		double Foo {get;set;}
		Bin<Transform> Bar {get;set;}

		
	}


	    public static partial class AgentAPI
    {
        public static double SetFoo(this IAgent agent, double foo)
        {
            var a = agent.Face<ITemplateFaceAgent>();
            foo *= 2;
	    a.Foo = foo;
            return foo;
        }
    }	
}
