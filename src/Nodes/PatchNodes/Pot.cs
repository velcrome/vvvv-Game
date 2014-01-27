#region usings
using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;


#endregion usings

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Pot",
                Category = "Game",
                Help = "Necessary base node to patch an action or decorator",
                Tags = "")]
    #endregion PluginInfo
    public class PotGameNode : AbstractDecoratorNode
    {
        [Input("Sink", AutoValidate = false, Order = 1)]
        public Pin<BehaviorLink> FSink;

        [Input("Agents", AutoValidate = false, Order = 2)]
        public IDiffSpread<Agent> FEditedAgents;

        [Input("ReturnCode", AutoValidate = false, Order = 3)]
        public ISpread<ReturnCodeEnum> FReturn;

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FEditedAgents.Changed += editAgents;
        }

        private void editAgents(IDiffSpread<Agent> spread)
        {
            FAgents.Clear();
            FAgents.AddRange(spread);

            if (IsPinValid(FInput))
            {
                FInput[0].Agents.Clear();
                FInput[0].Agents.AddRange(spread);

            }
        }


        public override void Evaluate(int SpreadMax)
        {
            Behave(FAgents);
           
            if (IsPinValid(FInput))
            {
                // valid FInput means, this is a decorator
                FInput[0].Agents.AddRange(FAgents);
                FInput.Sync();
            } 
            // no valid FInput means this is an action

            int i = 0;
            foreach (var agent in FAgents)
            {
                agent.ReturnCode = FReturn[i];
                i++;
            } 

            FinishEvaluation();
        }

        protected override void Behave(IEnumerable<Agent> agents)
        {
            if (IsPinValid(FSink))
            {
                FSink[0].Agents.AddRange(FAgents);
            }
            FSink.Sync();

            FEditedAgents.Sync();

            FReturn.Sync();
        }
    }

}
