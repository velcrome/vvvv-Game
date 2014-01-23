#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Pack.Game.Base;
using VVVV.Packs.Game;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;


#endregion usings

namespace VVVV.Pack.Game.Nodes

{
    public abstract class AbstractCompositeNode : AbstractBehaviorNode
    {
        #region fields & pins
        // A spread which contains our inputs
        public Spread<IIOContainer<Pin<BehaviorLink>>> FInputs = new Spread<IIOContainer<Pin<BehaviorLink>>>();

        [Config("Input Count", DefaultValue = 2, MinValue = 1)]
        public IDiffSpread<int> FInputCount;

        [Import]
        protected IIOFactory FIOFactory;

        #endregion fields & pins


        #region pin management
        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();
            FInputCount.Changed += HandleInputCountChanged;
        }

        private void HandlePinCountChanged<T>(ISpread<int> countSpread, Spread<IIOContainer<T>> pinSpread, Func<int, IOAttribute> ioAttributeFactory) where T : class
        {
            pinSpread.ResizeAndDispose(
                countSpread[0],
                (i) =>
                {
                    var ioAttribute = (InputAttribute)ioAttributeFactory(i + 1);
                    ioAttribute.AutoValidate = false;
                    var io =FIOFactory.CreateIOContainer<T>(ioAttribute);

                    var pin = (IIOContainer<Pin<BehaviorLink>>) io;
                    pin.IOObject.Connected += connect;
                    return io;
                }
            );
        }

        private void HandleInputCountChanged(IDiffSpread<int> sender)
        {
            HandlePinCountChanged(sender, FInputs, (i) => new InputAttribute(string.Format("Input {0}", i)));
        }
        #endregion

        #region RUNNING specials
        protected virtual IEnumerable<Agent> HandleRunningAgents(Pin<BehaviorLink> pin)
        {
            //  all running agents are put into their respective pins
            var running = from agents in FAgents
                          where agents.RunningNodes.ContainsKey(this) && agents.RunningNodes[this].Contains(pin)
                          select agents;

            if (IsPinValid(pin))
            {
                pin[0].Agents.AddRange(running);
            } else {
                // if pin has no connected node, the missing running behaviour fails gracefully
                foreach (var agent in running)
                {
                    agent.RemoveRunning(this, pin);
                    agent.ReturnCode = ReturnCodeEnum.Failure;
                }
                running = new List<Agent>();
            }
            return running;
        }

        #endregion


    }
}
