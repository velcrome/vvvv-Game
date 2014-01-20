#region usings
using System;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;


#endregion usings

namespace VVVV.Pack.Game.Nodes

{
    #region PluginInfo
    [PluginInfo(Name = "Sequence",
                Category = "Game",
                Help = "Sequence",
                Tags = "")]
    #endregion PluginInfo
    public class SequenceGameNode : AbstractBehaviorNode
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

        public override void Evaluate(int SpreadMax)
        {
            SpreadMax = FAgents.Count;
            base.Evaluate(SpreadMax);
            

            for (int i = 0; i < FInputCount[0]; i++)
            {
                var pin = FInputs[i].IOObject;
                if (pin.SliceCount > 0 && pin[0] != null)
                {
                    pin[0].Agents.Clear();
                    pin[0].Agents.InsertRange(0, FAgents); // insert all.
                    pin.Sync(); // call child
                }
            }

            PrintCodes(FAgents);
        }        
        
    }
}
