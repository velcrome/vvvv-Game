using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game
{
    public abstract class AbstractBehaviorNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {

        #region fields & pins
       
        [Output("Output", AutoFlush = false)]
        public Pin<BehaviorLink> FOutput;

        [Output("Return Code", AutoFlush = false)]
        public Pin<string> FReturnCode;

        [Import()]
        public ILogger FLogger;

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
            return pin.SliceCount > 0 && pin[0] != null && pin.PluginIO.IsConnected;
        }
        #endregion

        #region essentials
        protected void FinishEvaluation()
        {
//            FLogger.Log(LogType.Message, "Hi "+this.ToString());
            
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

        public virtual void Evaluate(int SpreadMax)
        {
/*      TODO: check for security, if more than one parent is connected.
            IPin pin = (IPin)FOutput.PluginIO;            
            if (pin.GetConnectedPins().Length > 1)
            {
                var pins = pin.GetConnectedPins();
                var node = pins[0].ParentNode;


                throw new Exception("Please do not connect more than one node downstream!");
            }
*/

            FinishEvaluation();             
        }

    }
}
