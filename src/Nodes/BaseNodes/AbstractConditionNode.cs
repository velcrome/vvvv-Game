using System.Collections.Generic;
using System.Linq;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game
{
    public abstract class AbstractConditionNode : AbstractDecoratorNode
    {
        [Input("Input when False", AutoValidate = false, Order=2)]
        protected Pin<BehaviorLink> FInputFalse;

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInputFalse.Connected += connect;
        }

        public override void Evaluate(int spreadMax)
        {
            StartEvaluation();

            Before(FAgents);

            var notFailedAgents = from agent in FAgents
                                  where agent.ReturnCode != ReturnCodeEnum.Failure
                                  select agent;

            bool pinTrue = IsPinValid(FInput);
            bool pinFalse = IsPinValid(FInputFalse);



            var result = Condition(notFailedAgents);

            int i = 0;
            foreach (var agent in notFailedAgents)
            {
                if (result.ElementAt(i))
                {
                    if (pinTrue) FInput[0].Agents.Add(agent);
                }
                else
                {
                    if (pinFalse) FInputFalse[0].Agents.Add(agent);
                }
                i++;

            }

            if (pinTrue) FInput.Sync(); // call child
            if (pinFalse) FInputFalse.Sync(); // call child

            After(notFailedAgents);
            FinishEvaluation();
        }

        public abstract IEnumerable<bool> Condition(IEnumerable<IAgent> agents);

        public override void Before(IEnumerable<IAgent> agents)
        {
            
        }

        public override void After(IEnumerable<IAgent> agents)
        {

        }

    }
}
