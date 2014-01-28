#region usings

using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

#endregion usings

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Birth",
                Category = "Game",
                Help = "Sequence",
                Tags = "")]
    #endregion PluginInfo
    public class BirthGameNode : IPluginEvaluate
    {
        #region fields & pins

        [Input("Insert", IsBang = true)]
        protected ISpread<bool> FInsert;

        [Input("Spread Count", MinValue = 0, DefaultValue = 1)]
        protected ISpread<int> FCount;

        [Output("Agents", AutoFlush = false)] 
        protected Pin<Agent> FAgents;
        
        #endregion fields & pins


        public void Evaluate(int SpreadMax)
        {
            FAgents.SliceCount = 0;
            SpreadMax = FCount.CombineWith(FInsert);
            for (int i = 0; i < SpreadMax;i++ )
            {
                if (FInsert[i])
                    for (int j=0;j<FCount[i];j++)
                    {
                        var a = new Agent();

                        
                        FAgents.Add(a);
                    }
            }
            FAgents.Flush();
        }

    }
}
