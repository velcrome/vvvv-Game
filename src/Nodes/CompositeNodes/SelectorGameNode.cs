using System.Linq;
using VVVV.Packs.Game;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Selector",
                Category = "Game",
                Help = "Sequence",
                Tags = "")]
    #endregion PluginInfo
    public class SelectorGameNode : AbstractCompositeNode
    {
        public override void Evaluate(int SpreadMax)
        {
            // initialize with all non-running agents
            var continueWith = from agents in FAgents
                               where !agents.RunningNodes.ContainsKey(this)
                               select agents;

            for (int index = 0; index < FInputCount[0]; index++)
            {
                var pin = FInputs[index].IOObject;
                var running = HandleRunningAgents(pin);

                if (IsPinValid(pin))
                {
                    pin[0].Agents.AddRange(continueWith); // add all that are still unsucessful
                    pin[0].Agents.Sort(); // sort in any running agents already in the target list

                    pin.Sync(); // call child

                    // treat all running agents 
                    running = from agents in pin[0].Agents
                              where agents.ReturnCode == ReturnCodeEnum.Running
                              select agents;
                    foreach (var agent in running) agent.AddRunning(this, pin);

                    // the agents that can be processed 
                    continueWith = from agents in pin[0].Agents
                                   where agents.ReturnCode == ReturnCodeEnum.Failure
                                   select agents;

                    // treat all running agents that changed their ReturnCode. 
                    var resetRunning = from agents in pin[0].Agents
                                       where agents.RunningNodes.ContainsKey(this) && agents.RunningNodes[this].Contains(pin) && agents.ReturnCode != ReturnCodeEnum.Running
                                       select agents;
                    foreach (var agent in resetRunning) agent.RemoveRunning(this, pin);
                }
            }

            FinishEvaluation();
        }
    }
}
