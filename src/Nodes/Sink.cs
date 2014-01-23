#region usings
using System;
using System.Collections.Generic;
using VVVV.Pack.Game;
using VVVV.Pack.Game.Base;
using VVVV.Packs.Game;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;


#endregion usings

namespace VVVV.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Pot",
                Category = "Game",
                Help = "Necessary base node to patch an action or decorator",
                Tags = "")]
    #endregion PluginInfo
    public class SinkGameNode : AbstractBehaviorNode
    {
        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Input("Agents", AutoValidate = false)]
        public IDiffSpread<Agent> FEditedAgents;

        [Input("ReturnCode", AutoValidate = false)]
        public ISpread<ReturnCodeEnum> FReturn;

        public virtual void OnImportsSatisfied()
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
            SpreadMax = FAgents.Count;
            
            if (IsPinValid(FInput))
            {
                FInput[0].Agents.AddRange(FAgents);
            }

            FInput.Sync();

//            FEditedAgents.Sync();
  //          FReturn.Sync();


            FinishEvaluation();

        }
    }

    #region PluginInfo
    [PluginInfo(Name = "ActionLid",
                Category = "Game",
                Help = "Necessary base node to patch an action",
                Tags = "")]
    #endregion PluginInfo
    public class ActionSourceGameNode : AbstractActionNode
    {
        [Output("Agents", AutoFlush = false)]
        public ISpread<Agent> FAgentsOut;

        protected override void Behave(IEnumerable<Agent> agents)
        {
            FAgentsOut.AssignFrom(FAgents);
            FAgentsOut.Flush();
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "DecoratorLid",
                Category = "Game",
                Help = "Necessary base node to patch a decorator. DOES NOT WORK ATM",
                Tags = "BROKEN")]
    #endregion PluginInfo
    public class DecoratorSourceGameNode : AbstractBehaviorNode
    {
        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Output("Agents", AutoFlush = false)]
        public ISpread<Agent> FAgentsOut;

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInput.Connected += connect;
        }

        public override void Evaluate(int SpreadMax)
        {
            if (!IsPinValid(FInput)) throw new Exception("Decorator cannot be used unconnected");

            FAgentsOut.AssignFrom(FAgents);
            FAgentsOut.Flush();


            // TODO: make sure Pot finished incorporating change before syncing further upstream

            FInput[0].Agents.AddRange(FAgents); // add all.
            FInput.Sync();


            FinishEvaluation();
        }

    }

}
