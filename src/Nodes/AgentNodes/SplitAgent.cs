using System;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Nodes;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

namespace VVVV.Pack.Game.AgentNodes
{
    #region PluginInfo
    [PluginInfo(Name = "Split", AutoEvaluate = true, Category = "Game",
        Help = "Splits all Agents into custom dynamic pins", Tags = "Agent")]
    #endregion PluginInfo
    public class SplitAgentNode : AbstractFacedDynamicNode
    {
        [Output("Timestamp", AutoFlush = false)]
        protected ISpread<string> FTimeStamp;

        protected override IOAttribute DefinePin(string name, Type type)
        {
            var attr = new OutputAttribute(name);
            attr.BinVisibility = PinVisibility.Hidden;

            attr.AutoFlush = false;

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
                FTimeStamp.SliceCount = SpreadMax;
            }

            for (int i = 0; i < SpreadMax; i++)
            {
                Agent agent = FInput[i];

                FTimeStamp[i] = agent.BirthTime.ToString();

                foreach (string name in FPins.Keys)
                {
                    var pinSpread = (ISpread)((ISpread) FPins[name].RawIOObject)[i];
                    var bin = agent[name];

                    pinSpread.SliceCount = bin.Count;
                    for (int j = 0; j < bin.Count; j++) pinSpread[j] = bin[j];

                }
            }
            
            FOutput.AssignFrom(FInput);
            
            FTimeStamp.Flush();
            foreach (var pin in FPins.Values)
            {
                ((ISpread) pin.RawIOObject).Flush();
            }

        }
    }

}
