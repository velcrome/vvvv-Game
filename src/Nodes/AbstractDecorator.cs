using System.Collections.Generic;
using VVVV.Packs.Game;
using VVVV.Packs.GameElement.Base;
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
            SpreadMax = FAgents.Count;
            
            base.Evaluate(SpreadMax);
            
            Behave(FAgents);

            if (FInput.SliceCount > 0 && FInput[0] != null)
            {
                FInput[0].Agents.Clear();
                FInput[0].Agents.InsertRange(0, FAgents); // insert all.
                FInput.Sync(); // call child
            }

            PrintCodes(FAgents);
        }

        protected abstract void Behave(IEnumerable<Agent> agents);
    }
}
