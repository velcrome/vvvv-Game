using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo

    [PluginInfo(Name = "FrameDelay",
        AutoEvaluate = true,
        Category = "Game",
        Help = "",
        Tags = "Agent")]
    #endregion PluginInfo
    public class FrameDelayAgentNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {
        #region fields & pins

        [Input("Input", CheckIfChanged = true)]
        protected IDiffSpread<Agent> FInput;

        [Input("Default", AutoValidate = false, BinSize = 0)]
        protected Pin<Agent> FDefault;

        [Input("Initialize", IsBang = true, DefaultBoolean = false)]
        protected ISpread<bool> FInit;

        [Output("Agent", AutoFlush = false, AllowFeedback = true)]
        public Pin<Agent> FOutput;


        [Import()]
        protected IMainLoop FMainLoop;

        [Import()]
        protected ILogger FLogger;

        protected bool IsFirstFrame = true;
        protected List<Agent> LastFrameAgents = new List<Agent>(); 

        
        #endregion

        public void OnImportsSatisfied()
        {
            FMainLoop.OnResetCache += Push;
  //          FInput.Changed += InputChanged;

        }

        private void InputChanged(IDiffSpread<Agent> spread)
        {
            FLogger.Log(LogType.Debug, "change " + FInput.SliceCount);
        }

        protected void Push(object sender, EventArgs eventArgs)
        {
//            FLogger.Log(LogType.Debug, "push "+ FInput.SliceCount);

            LastFrameAgents.Clear();
            foreach (var agent in FInput)
            {
                if (agent != null) LastFrameAgents.Add(agent);
            }
            
        }

        public void Evaluate(int SpreadMax)
        {
            if (IsFirstFrame || FInit[0])
            {
                FOutput.AssignFrom(FDefault);
                IsFirstFrame = false;
            } else
            {
                FOutput.AssignFrom(LastFrameAgents);
            }
            
            FOutput.Flush();
        }
    }
}