using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Base;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Root",
                AutoEvaluate = true,
                Category = "Game",
                Help = "Basic template with one value in/out",
                Tags = "")]
    #endregion PluginInfo
    public class RootGameNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {
        #region fields & pins
        [Input("Input", DefaultValue = 1.0, AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Output("Return Code")]
        public Pin<string> FReturnCode;

        [Import()]
        public ILogger FLogger;


        public List<Agent> FAgents = new List<Agent>();

        #endregion fields & pins
        
        public void OnImportsSatisfied()
        {
            FInput.Connected += connect;

        }

        private void connect(object sender, PinConnectionEventArgs args)
        {
            FInput.Sync();
        }

        public void Evaluate(int SpreadMax)
        {
            if (FInput.SliceCount < 1 || FInput[0] == null) return;



            SpreadMax = VMath.Random.Next(10)+1;

            FAgents.Clear();

            for (int tmp = 0; tmp < SpreadMax; tmp++)
            {
                FAgents.Add(new Agent());
            }

            FInput[0].Agents.Clear();
            FInput[0].Agents.InsertRange(0, FAgents);

            FInput.Sync();


            FReturnCode.SliceCount = SpreadMax;
            int i = 0;
            foreach (Agent agent in FAgents)
            {
                var code = agent.ReturnCode;
                FReturnCode[i] = code.ToString();
                i++;
            }

            FReturnCode.Flush();
        }


    }
}
