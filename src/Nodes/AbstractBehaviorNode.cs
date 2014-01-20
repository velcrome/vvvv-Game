using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.Core.Logging;
using VVVV.Packs.Game;
using VVVV.Packs.GameElement.Base;
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

        #endregion fields & pins
        
        
        public virtual void OnImportsSatisfied()
        {
            link = new BehaviorLink(FAgents);
            FOutput.SliceCount = 1;
            FOutput[0] = link;
            FOutput.Flush();

            FOutput.Disconnected += disconnect;

        }

        private void disconnect(object sender, PinConnectionEventArgs args)
        {
            FLogger.Log(LogType.Message, "hello " + this.ToString());
            FAgents.Clear();

            Evaluate(0);            
        }

        protected void connect(object sender, PinConnectionEventArgs args)
        {
            ( (Pin<BehaviorLink>) sender).Sync();
        }

        protected void PrintCodes(IEnumerable<Agent> agents)
        {
            FLogger.Log(LogType.Message, "Hi "+this.ToString());
            
            FReturnCode.SliceCount = FAgents.Count;
            int i = 0;
            foreach (Agent agent in agents)
            {
                i++;
                FReturnCode[i] = agent.ReturnCode.ToString();
            }
            FReturnCode.Flush();
        }

        public virtual void Evaluate(int SpreadMax)
        {
            SpreadMax = FAgents.Count;

            FOutput.SliceCount = Math.Max(1, SpreadMax);
            FOutput[0] = link;
            FOutput.Flush();

/*      TODO: check for security, if more than one parent is connected.
            IPin pin = (IPin)FOutput.PluginIO;            
            if (pin.GetConnectedPins().Length > 1)
            {
                var pins = pin.GetConnectedPins();
                var node = pins[0].ParentNode;


                throw new Exception("Please do not connect more than one node downstream!");
            }
*/

             
        }

    }
}
