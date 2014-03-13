using System;
using VVVV.Nodes;
using VVVV.Pack.Game.AgentNodes;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo

    [PluginInfo(Name = "Cons",
        Category = "Game",
        Help = "Cons Agents",
        Tags = "Agent")]

    #endregion PluginInfo

    public class ConsAgentNode : Cons<Agent>{}

}