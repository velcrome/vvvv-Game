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

        protected bool firstFrame = true;
        protected List<Agent> lastFrame = new List<Agent>(); 

        
        #endregion

        public void OnImportsSatisfied()
        {
            FMainLoop.OnPrepareGraph += Push;
  //          FInput.Changed += InputChanged;

        }

        private void InputChanged(IDiffSpread<Agent> spread)
        {
            FLogger.Log(LogType.Debug, "change " + FInput.SliceCount);
        }

        protected void Push(object sender, EventArgs eventArgs)
        {
//            FLogger.Log(LogType.Debug, "push "+ FInput.SliceCount);

            lastFrame.Clear();
            foreach (var agent in FInput)
            {
                if (agent != null) lastFrame.Add(agent);
            }
            
        }

        public void Evaluate(int SpreadMax)
        {
            if (firstFrame || FInit[0])
            {
                FOutput.AssignFrom(FDefault);
                firstFrame = false;
            } else
            {
                FOutput.AssignFrom(lastFrame);
            }
            
            FOutput.Flush();
        }
    }
}