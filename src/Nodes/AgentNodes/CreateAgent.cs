using System;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.AgentNodes
{
    //#region PluginInfo
    //[PluginInfo(Name = "Create", Category = "Game", Help = "Creates an Agent",
    //    Tags = "Agent")]
    //#endregion PluginInfo
    public class CreateAgentNode : AbstractFacedDynamicNode
    {
        protected override IOAttribute DefinePin(string name, Type type)
        {
            var attr = new InputAttribute(name);
            attr.BinVisibility = PinVisibility.Hidden;
            attr.BinSize = 1;
            attr.Order = FCount;
            attr.BinOrder = FCount + 1;
            //                attr.AutoValidate = false;  // need to sync all pins manually
            return attr;
        }


        public override void Evaluate(int SpreadMax)
        {
            FInput.Dispose();
            
            

            for (int i = 0; i < SpreadMax; i++)
            {
                Agent agent = FInput[i];
                foreach (string name in FPins.Keys)
                {
                    
                    var pin = ((ISpread<ISpread>)FPins[name].RawIOObject)[i];
                    agent.Assign(name, pin);
                }
            }

            FOutput.AssignFrom(FInput);
            FOutput.Flush();
        }
    }
}
