using System;
using VVVV.Pack.Game.AgentNodes;
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
    public class InitAgendNode : AbstractFacedDynamicNode
    {
        #region fields & pins

        [Input("Init First Element", IsToggle = true, DefaultBoolean = true, IsSingle = true)]
        protected ISpread<bool> FInitFirst;

        #endregion

        protected override IOAttribute DefinePin(string name, Type type)
        {
            var attr = new InputAttribute(name);
            attr.BinVisibility = PinVisibility.Hidden;
            attr.AutoValidate = false;

            attr.Order = FCount;
            attr.BinOrder = FCount + 1;
            return attr;
        }

        public override void Evaluate(int SpreadMax)
        {
            SpreadMax = FInput.SliceCount;
            if (FInput.IsAnyInvalid()) SpreadMax = 0;

            foreach (string pinName in FPins.Keys)
            {
                ((ISpread)FPins[pinName].RawIOObject).SliceCount = SpreadMax;
            }
            FOutput.SliceCount = SpreadMax;


            for (int i = 0; i < SpreadMax; i++)
            {
                var agent = FInput[i];

                 Type face = AllAgentFaces[FFace[0].Index];
                 agent.Init(face, FInitFirst[0]);
            }
            FOutput.AssignFrom(FInput);
        }




    }
}