using System;
using VVVV.Pack.Game.AgentNodes;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo

    [PluginInfo(Name = "Kill",
        Category = "Game",
        Help = "Marks an Agent to be killed. Will be removed by the Store.",
        Tags = "Agent")]
    #endregion PluginInfo
    public class KillAgentNode : IPluginEvaluate
    {
        #region fields & pins

        [Input("Agent")]
        protected Pin<Agent> FInput;

        [Input("Kill", IsToggle = true, DefaultBoolean = false, AutoValidate = false)]
        protected ISpread<bool> FKill;

        [Output("Agent", AutoFlush = false)]
        public Pin<Agent> FOutput;
        #endregion

        public void Evaluate(int SpreadMax)
        {
            SpreadMax = FInput.SliceCount;
            
            if (FInput.IsAnyInvalid()) SpreadMax = 0;
            FKill.Sync();

            for (int i = 0; i < SpreadMax; i++)
            {
                var agent = FInput[i];
                agent.Killed = FKill[i];



            }
            FOutput.AssignFrom(FInput);
            FOutput.Flush();
        }




    }
}