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

/*        [Input("Edit Message")]
        IDiffSpread<Agent> FEdit;

        [Input("Edit Slice")]
        IDiffSpread<int> FEditIndex;

        [Input("Edit", IsSingle = true, IsBang = true)]
        ISpread<bool> FEditNow;

*/  
        [Input("Delete Slice")]
        ISpread<int> FDeleteIndex;

        [Input("Delete", IsSingle = true, IsBang = true)]
        ISpread<bool> FDeleteNow;

        [Input("Clear", IsSingle = true, IsBang = true)]
        ISpread<bool> FClear;

        [Input("Add Agent")]
        ISpread<Agent> FAdd;

        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Output("Output", AutoFlush = false)]
        private Pin<Agent> FOutput;


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
            if (FClear[0]) FAgents.Clear();

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
                    FAgents.RemoveAt(del[i] - i);
                }
            }

            FAgents.AddRange(FAdd);
            FAgents.Sort();

            if (FInput.SliceCount > 0 && FInput[0] != null && FInput.PluginIO.IsConnected)
            {
                FInput[0].Agents.AddRange(FAgents);
      //          FInput[0].Agents.Sort();
                FInput.Sync();

                FOutput.AssignFrom(FInput[0].Agents);
            }
            else
            {
                FOutput.AssignFrom(FAgents);
            }
            FOutput.Flush();

        }
    }
}
