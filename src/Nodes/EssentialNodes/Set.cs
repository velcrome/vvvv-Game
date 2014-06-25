using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;

using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "SetAttribute", AutoEvaluate = true, Category = "Game",
        Help = "Sets Attributes", Tags = "")]
    #endregion PluginInfo

    public class SetNode : AbstractDynamicNode, IPartImportsSatisfiedNotification
    {
        #region fields & pins

        [Input("Input", AutoValidate = false)]
        public Pin<BehaviorLink> FInput;

        [Output("Output", AutoFlush = false)]
        public Pin<BehaviorLink> FOutput;

        [Output("Return Code", AutoFlush = false)]
        public Pin<string> FReturnCode;

        public List<Agent> FAgents = new List<Agent>();
        protected BehaviorLink link;

        [Import()]
        protected IMainLoop FMainLoop;

        #endregion fields & pins

        protected override IOAttribute DefinePin(string name, Type type)
        {
            var attr = new InputAttribute(name);
            attr.BinVisibility = PinVisibility.Hidden;
            attr.BinSize = 1;
            attr.AutoValidate = false;


            attr.Order = FCount;
            attr.BinOrder = FCount + 1;
            return attr;
        }
        
        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInput.Connected += connect;

            link = new BehaviorLink(FAgents);
            FOutput.SliceCount = 1;
            FOutput[0] = link;
            FOutput.Flush();

            FOutput.Disconnected += disconnect;
            FMainLoop.OnPrepareGraph += init;
        }

        public override void Evaluate(int spreadMax)
        {
            FLogger.Log(LogType.Error, FAgents.Count.ToString());
            
            foreach (string pinName in FPins.Keys)
            {
                var pin = ((ISpread)FPins[pinName].RawIOObject);
                pin.Sync();

                int i = 0;
                foreach (var agent in FAgents)
                    agent.Assign(pinName, pin[i++]);
            }
            

            if (IsPinValid(FInput))
            {
                FInput[0].Agents.AddRange(FAgents);
                FInput.Sync(); // call child
            }


            FinishEvaluation();
        }

        #region evaluation management


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
            ((Pin<BehaviorLink>)sender).Sync();
        }

        public bool IsPinValid(Pin<BehaviorLink> pin)
        {
            return !pin.IsAnyInvalid() && pin.PluginIO.IsConnected;
        }
        #endregion

        #region essentials
        /// <summary>
        /// Flush all output pins.
        /// </summary>        
        protected void FinishEvaluation()
        {
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
    }
}
