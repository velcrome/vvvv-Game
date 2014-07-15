using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.AgentNodes
{
    [PluginInfo(Name = "Search", Category = "Game", Help = "Allows LINQ queries for Agents", Tags = "Agent, LINQ", Author = "velcrome")
    ]
    public class GameSearchNode : IPluginEvaluate
    {
#pragma warning disable 649, 169
        [Input("Input")]
        private IDiffSpread<Agent> FInput;

        [Input("Where", DefaultString = "Foo = \"bar\"")]
        private IDiffSpread<string> FWhere;

        [Input("SendQuery", IsSingle = true, IsBang = true)]
        private IDiffSpread<bool> FSendQuery;

        [Output("Message", AutoFlush = false)]
        private ISpread<ISpread<Agent>> FOutput;

        [Output("Former Slice", AutoFlush = false)]
        private ISpread<ISpread<int>> FFormerSlice;

        [Output("Query", AutoFlush = false)]
        private ISpread<string> FQuery;

        [Import()]
        protected ILogger FLogger;
#pragma warning restore

        public void Evaluate(int SpreadMax)
        {
            if (!FInput.IsChanged && !FSendQuery.IsChanged && !FQuery.IsChanged && !FSendQuery[0]) return;


            if (FSendQuery[0])
            {
                SpreadMax = FWhere.SliceCount;
                FQuery.SliceCount = SpreadMax;
                FOutput.SliceCount = SpreadMax;
                FFormerSlice.SliceCount = SpreadMax;


                for (int i = 0; i < SpreadMax; i++)
                {

                    var query = FWhere[i].Split('=');

                    string attrib = query[0].Trim();
                    string val = "";

                    for (int j = 1; j < query.Count(); j++) val += query[j];
                    val = val.Trim(' ');//.Trim('\"');

                    FQuery[i] = "FROM Agent IN Input WHERE " + attrib + " = " + val + " SELECT Agent";

                    var result =
                        from m in FInput
                        where m != null
                        where m[attrib].Count > 0 && m[attrib][0].Equals(val)
                        select m;

                    FOutput[i].AssignFrom(result);

                    FFormerSlice[i].SliceCount = 0;

                    foreach (var m in result)
                    {
                        for (int j = 0; j < FInput.SliceCount; j++)
                        {

                            if (m.Equals(FInput[j]))
                            {
                                FFormerSlice[i].Add(j);
                                break;
                            }
                        }
                    }
                }

                if (SpreadMax == 1 && ((FOutput[0].SliceCount == 0) || (FOutput[0][0] == null)))
                {
                    FOutput.SliceCount = 0;
                }

                FFormerSlice.Flush();
                FQuery.Flush();
                FOutput.Flush();
            }
        }
    }
}
