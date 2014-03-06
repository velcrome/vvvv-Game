using System;
using System.Collections.Generic;
using System.Linq;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game
{
    public abstract class AbstractDecoratorNode : AbstractGameNode
    {
        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInput.Connected += connect;
        }




        public override void Evaluate(int spreadMax)
        {
            StartEvaluation();
            Before(FAgents); // hook for a priori checks

            var notFailedAgents = from agent in FAgents
                                where agent.ReturnCode != ReturnCodeEnum.Failure
                                select agent;
            if (IsPinValid(FInput))
            {
                FInput[0].Agents.AddRange(notFailedAgents); 
                FInput.Sync(); // call child
            }

            After(notFailedAgents); // hook for post priori checks

            FinishEvaluation();
        }

        public abstract void Before(IEnumerable<IAgent> agents);
        public abstract void After(IEnumerable<IAgent> agents);

    }
}
