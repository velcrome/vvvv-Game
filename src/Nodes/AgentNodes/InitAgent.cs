using System;
using VVVV.Pack.Game.AgentNodes;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo

    [PluginInfo(Name = "Init",
        Category = "Game",
        Help = "inits an Agent",
        Tags = "Agent")]
    #endregion PluginInfo
    public class InitAgentNode : AbstractFacedDynamicNode
    {
        #region fields & pins

        [Input("Init First Element", IsToggle = true, DefaultBoolean = true, IsSingle = true)]
        protected ISpread<bool> FInitFirst;

        #endregion

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

        public override void Evaluate(int SpreadMax)
        {
            SpreadMax = FInput.SliceCount;
            if (FInput.IsAnyInvalid()) SpreadMax = 0;

            for (int i = 0; i < SpreadMax; i++)
            {
                var agent = FInput[i];

                Type face = AllAgentFaces[FFace[0].Index];
                agent.Init(face, FInitFirst[0]);

                foreach (string pinName in FPins.Keys)
                {
                    var pin = ((ISpread) FPins[pinName].RawIOObject);
                    pin.Sync();
                    agent.Assign(pinName, pin[i]);
                }

            }
            FOutput.AssignFrom(FInput);

        }




    }
}