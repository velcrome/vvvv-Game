using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;

namespace VVVV.Pack.Game.Nodes
{
    public abstract class AbstractActionNode : AbstractBehaviorNode
    {

        protected abstract void Behave(IEnumerable<IAgent> agents);

        public override void Evaluate(int SpreadMax)
        {
            Behave(FAgents);
            FinishEvaluation();
        }
    }
}