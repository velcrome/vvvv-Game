#region usings
using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;


#endregion usings

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Decorator",
                Category = "Game",
                Help = "Basic template with one value in/out",
                Tags = "")]
    #endregion PluginInfo
    public class DecoratorTestGameNode : AbstractDecoratorNode
    {
        public override void Before(IEnumerable<IAgent> agents)
        {
        }

        public override void After(IEnumerable<IAgent> agents)
        {
        }
    
    }
}
