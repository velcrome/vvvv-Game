using System.Collections.Generic;
using System.Linq;
using VVVV.Pack.Game.Base;
using VVVV.Packs.Game;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Parallel",
                Category = "Game",
                Help = "Sequence",
                Tags = "")]
    #endregion PluginInfo
    public class ParallelGameNode : AbstractCompositeNode
    {
        #region fields & pins
        // RunningIfAnyRuns takes precedence in any case!
        [Input("Policy", AutoValidate = false, DefaultEnumEntry = "FailIfAnyFails", Visibility = PinVisibility.OnlyInspector, IsSingle = true)]
        public Pin<Policy> FPolicy;


        #endregion

        #region RUNNING specials
        protected override IEnumerable<Agent> HandleRunningAgents(Pin<BehaviorLink> pin)
        {
            //  all running agents are put into their respective pins
            var running = from agents in FAgents
                          where agents.RunningNodes.ContainsKey(this) && agents.RunningNodes[this].Contains(pin)
                          select agents;

            if (!IsPinValid(pin)) {
                // if pin has no connected node, the missing running behaviour fails gracefully
                foreach (var agent in running)
                {
                    agent.RemoveRunning(this, pin);
                    agent.ReturnCode = ReturnCodeEnum.Failure;
                }
                running = new List<Agent>();
            }
            return running;
        }

        #endregion
        
        public override void Evaluate(int SpreadMax)
        {
            // initialize UnitedStates
            FPolicy.Sync();

            var unitedStates = new ReturnCodeEnum[FAgents.Count];
            
            if (FPolicy[0] == Policy.SuccessIfAnySucceeds)
            {
                for (int i = 0; i < FAgents.Count; i++) unitedStates[i] = ReturnCodeEnum.Failure;
            } else
            {
                for (int i = 0; i < FAgents.Count; i++) unitedStates[i] = ReturnCodeEnum.Success;
            }
            
            
            // initialize with all agents
            for (int index = 0; index < FInputCount[0]; index++)
            {
                var pin = FInputs[index].IOObject;

                

                if (IsPinValid(pin))
                {
                    pin[0].Agents.AddRange(FAgents); // add all that are still unsucessful
//                    pin[0].Agents.Sort(); // sort in any running agents already in the target list

                    pin.Sync(); // call child

                    // treat all running agents 
                    var running = from agents in pin[0].Agents
                              where agents.ReturnCode == ReturnCodeEnum.Running
                              select agents;
                    foreach (var agent in running) agent.AddRunning(this, pin);


                    int i = 0;
                    foreach (var agent in FAgents)
                    {
                        switch (unitedStates[i])
                        {
                            case ReturnCodeEnum.Running:
                                break;
                            case ReturnCodeEnum.Success:
                                if (agent.ReturnCode == ReturnCodeEnum.Running) unitedStates[i] = ReturnCodeEnum.Running;
                                else if (FPolicy[0] == Policy.FailIfAnyFails && agent.ReturnCode == ReturnCodeEnum.Failure)
                                    unitedStates[i] = ReturnCodeEnum.Failure;
                                break;
                            case ReturnCodeEnum.Failure:
                                if (agent.ReturnCode == ReturnCodeEnum.Running) unitedStates[i] = ReturnCodeEnum.Running;
                                else if (FPolicy[0] == Policy.SuccessIfAnySucceeds && agent.ReturnCode == ReturnCodeEnum.Success)
                                    unitedStates[i] = ReturnCodeEnum.Success;
                                break;
                        }


                        if (agent.ReturnCode == ReturnCodeEnum.Running) unitedStates[i] = ReturnCodeEnum.Running;

                        i++;
                    }


                    // treat all running agents that changed their ReturnCode. 
                    var resetRunning = from agents in pin[0].Agents
                                       where agents.RunningNodes.ContainsKey(this) && agents.RunningNodes[this].Contains(pin) && agents.ReturnCode != ReturnCodeEnum.Running
                                       select agents;
                    foreach (var agent in resetRunning) agent.RemoveRunning(this, pin);
                }
            }

            for (int i = 0; i < FAgents.Count; i++) FAgents[i].ReturnCode = unitedStates[i];
            FinishEvaluation();
        }
    }

    public enum Policy
    {
        FailIfAnyFails,
        SuccessIfAnySucceeds
    }
}
