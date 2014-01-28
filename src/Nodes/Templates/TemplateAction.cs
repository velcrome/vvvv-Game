using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Template",
        Category = "Game",
        Help = "Template node to patch an action",
        Tags = "")]
    #endregion PluginInfo
    public class TemplateActionGameNode : AbstractActionNode
    {

        protected override void Behave(IEnumerable<IAgent> agents)
        {


            foreach (var agent in agents)
            {
                
                
                var cast = agent.Face<IMoveableAgent>();





            }


        }
    }
}