using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game
{
    public abstract class AbstractGameNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {

        #region fields & pins
       
        [Output("Output", AutoFlush = false)]
        public Pin<BehaviorLink> FOutput;

        [Output("Return Code", AutoFlush = false)]
        public Pin<string> FReturnCode;

        [Import()]
        public ILogger FLogger;

        [Import()]
        public IPluginHost2 FHost;

        public List<Agent> FAgents = new List<Agent>();
        protected BehaviorLink link;

        [Import()]
        protected IMainLoop FMainLoop;
        
        #endregion fields & pins

        #region evaluation management
        public virtual void OnImportsSatisfied()
        {
            link = new BehaviorLink(FAgents);
            FOutput.SliceCount = 1;
            FOutput[0] = link;
            FOutput.Flush();

            FOutput.Disconnected += disconnect;
            FMainLoop.OnPrepareGraph += init;

        }

        private void init(object sender, EventArgs e)
        {
            FAgents.Clear();
        }

        private void disconnect(object sender, PinConnectionEventArgs args)
        {
            FAgents.Clear();

            FOutput.SliceCount = 1;
            FOutput[0] = link;
            FOutput.Flush();
        }

        protected void connect(object sender, PinConnectionEventArgs args)
        {
            ( (Pin<BehaviorLink>) sender).Sync();
        }

        public bool IsPinValid(Pin<BehaviorLink> pin)
        {
            return !pin.IsAnyInvalid() && pin.PluginIO.IsConnected;
        }
        #endregion

        #region essentials
        protected void FinishEvaluation()
        {

//          Flush all output pins.

            FReturnCode.SliceCount = FAgents.Count;
            int i = 0;
            foreach (Agent agent in FAgents)
            {
                FReturnCode[i] = agent.ReturnCode.ToString();
                i++;
            }
            FReturnCode.Flush();

            FOutput.SliceCount = Math.Max(1, FAgents.Count);
            FOutput[0] = link;
            FOutput.Flush();
        }
        #endregion

        protected virtual void StartEvaluation()
        {
            //          Sync all input pins.

            foreach (var p in FHost.GetPins())
            {
//                FLogger.Log(LogType.Message, p.ToString());

            }            
        }

        public abstract void Evaluate(int spreadMax);


    }
}
