using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Lid",
        Category = "Game",
        Help = "Necessary base node to patch an action",
        Tags = "")]
    #endregion PluginInfo
    public class LidGameNode : AbstractActionNode
    {
        [Output("Agents", AutoFlush = false, Order = 1)]
        public ISpread<Agent> FAgentsOut;

        protected override void Behave(IEnumerable<IAgent> agents)
        {
            FAgentsOut.AssignFrom(FAgents);
            FAgentsOut.Flush();
        }
    }
}