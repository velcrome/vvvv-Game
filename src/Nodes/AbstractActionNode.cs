using System.Collections.Generic;
using VVVV.Pack.Game;
using VVVV.Packs.Game;
using VVVV.Packs.GameElement.Base;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
    public abstract class AbstractActionNode : AbstractBehaviorNode
    {

        protected abstract void Behave(IEnumerable<Agent> agents);

        public override void Evaluate(int SpreadMax)
        {
            base.Evaluate(SpreadMax);
            Behave(FAgents);
            PrintCodes(FAgents);
        }
    }
}