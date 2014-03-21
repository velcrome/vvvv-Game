using System.Linq;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Sequence",
                Category = "Game",
                Help = "Sequence",
                Tags = "")]
    #endregion PluginInfo
    public class SequenceGameNode : AbstractCompositeNode 
    {
        public override void Evaluate(int spreadMax)
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
                    pin[0].Agents.AddRange(continueWith); // add all that are still sucessful
                    pin[0].Agents.Sort(); // sort in any running agents already in the target list

                    pin.Sync(); // call child

                    // treat all running agents 
                    running = from agent in pin[0].Agents
                              where agent.ReturnCode == ReturnCodeEnum.Running
                              select agent;
                    foreach (var agent in running) agent.AddRunning(this, pin);

                    // the agents that can be processed 
                    continueWith = from agent in pin[0].Agents
                                   where agent.ReturnCode == ReturnCodeEnum.Success
                                   select agent;

                    // treat all running agents that changed their ReturnCode. can be done 
                    var resetRunning = from agent in pin[0].Agents
                                       where agent.RunningNodes.ContainsKey(this) && agent.RunningNodes[this].Contains(pin) && agent.ReturnCode != ReturnCodeEnum.Running
                                       select agent;
                    foreach (var agent in resetRunning) agent.RemoveRunning(this, pin);


                }
            }

            FinishEvaluation();
        }
    }
}
