using VVVV.Nodes;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

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