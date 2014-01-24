using System;
using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game
{
    public abstract class AbstractDecoratorNode : AbstractBehaviorNode
    {
        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInput.Connected += connect;
        }

        public override void Evaluate(int SpreadMax)
        {
            Behave(FAgents);

            if (IsPinValid(FInput))
            {
                FInput[0].Agents.AddRange(FAgents); // add all.
                FInput.Sync(); // call child
            } else
            {
                throw new Exception("Decorator cannot be used unconnected");
            }

            FinishEvaluation();
        }

        protected abstract void Behave(IEnumerable<Agent> agents);
    }
}
