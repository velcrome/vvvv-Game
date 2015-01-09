using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Nodes
{
    [PluginInfo(
        Name = "Store", 
        Category = "Game", 
        Help = "Stores Agents, is the root of all Behavior Trees", 
        AutoEvaluate = true,
        Tags = "velcrome")]
    public class ElementStoreNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {
        #region pins and fields
        #pragma warning disable 649, 169


        [Input("Delete Slice")]
        ISpread<int> FDeleteIndex;

        [Input("Delete", IsSingle = true, IsBang = true)]
        ISpread<bool> FDeleteNow;

        [Input("Clear", IsSingle = true, IsBang = true)]
        ISpread<bool> FClear;

        [Input("Add Agent")]
        Pin<Agent> FAdd;

        [Input("Limit Agent Count", DefaultValue = 32, IsSingle = true, Visibility = PinVisibility.OnlyInspector)]
        Pin<int> FLimitCount;

        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Input("Tick", AutoValidate = false, IsSingle = true, DefaultBoolean = true, IsToggle = true, Visibility = PinVisibility.OnlyInspector)]
        public Pin<bool> FTick;

        [Output("Output", AutoFlush = false)]
        private Pin<Agent> FOutput;

        [Output("Killed Agents", AutoFlush = false)]
        private Pin<Agent> FKilled;

        private List<Agent> FAgents = new List<Agent>();

        [Import()]
        protected ILogger FLogger;

        #pragma warning restore
        #endregion

        #region evaluation management
        public void OnImportsSatisfied()
        {
            FInput.Connected += connect;

        }

        private void connect(object sender, PinConnectionEventArgs args)
        {
            FInput.Sync();
        }
        #endregion

        public void Evaluate(int SpreadMax)
        {
            FKilled.SliceCount = 0;
            if (FClear[0])
            {
                FKilled.AssignFrom(FAgents);
                FAgents.Clear();
            }

            if (FDeleteNow[0])
            {
                var del = FDeleteIndex.ToList();

                int size = FAgents.Count();

                for (int i = 0; i < del.Count; i++)
                {
                    del[i] = VMath.Zmod(del[i], size);
                }
                del.Sort();

                for (int i = 0; i < del.Count; i++)
                {
//                    if (FAgents.Count > i)
                        FAgents.RemoveAt(del[i] - i);
                }
            }

            var killed = from a in FAgents
                         where a.Killed
                         select a;
            FKilled.AddRange(killed);
            FAgents.RemoveAll(IsKilled);

            if (!FAdd.IsAnyInvalid())
            {
                var count = FAgents.Count;
                foreach (var agent in FAdd)
                {
                    if (agent != null && !Limited(count))
                    {
                        FAgents.Add(agent);
                        count++;
                    }
                }
                FAgents.Sort();
            }

            FTick.Sync();
            if (!FInput.IsAnyInvalid() && FInput.PluginIO.IsConnected && FTick[0])
            {
                FInput[0].Agents.Clear();
                FInput[0].Agents.AddRange(FAgents);

                foreach (var agent in FInput[0].Agents)
                {
                    agent.ReturnCode = ReturnCodeEnum.Success;
                }
                
                FInput.Sync();

                FOutput.SliceCount = 0;
                FOutput.AssignFrom(FInput[0].Agents);
            }
            else
            {
                FOutput.SliceCount = 0;
                FOutput.AssignFrom(FAgents);
            }
            FOutput.Flush();
            FKilled.Flush();

        }

        protected bool Limited(int count)
        {
            return (FLimitCount[0] >= 0) && (count >= FLimitCount[0]);
        }

        private static bool IsKilled(Agent agent)
        {
            return agent.Killed;
        }
    }
}
