#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Packs.Game;
using VVVV.Packs.GameElement.Base;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.Streams;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
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
