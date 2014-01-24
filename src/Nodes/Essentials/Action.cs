#region usings
using System.Collections.Generic;
using VVVV.Pack.Game.Core;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

#endregion usings

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Action",
                Category = "Game",
                Help = "Basic template with one value in/out",
                Tags = "")]
    #endregion PluginInfo
    public class ActionGameNode : AbstractActionNode
    {
        [Input("ReturnCode")]
        public ISpread<ReturnCodeEnum> FSetCode;
        
        protected override void Behave(IEnumerable<Agent> agents)
        {
            int i = 0;
            foreach (Agent agent in agents)
            {
                var code = FSetCode[i];
                agent.ReturnCode = code;
                
                i++;
            }
        }
    }
}
