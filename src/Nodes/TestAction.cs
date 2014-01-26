using System.Collections.Generic;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "TestAction",
        Category = "Game",
        Help = "Necessary base node to patch an action",
        Tags = "")]
    #endregion PluginInfo
    public class TestActionGameNode : AbstractActionNode
    {

        protected override void Behave(IEnumerable<Agent> agents)
        {


            foreach (var agent in agents)
            {
                
                
                var cast = agent.Face<ITestAgent>();


                cast.Name = new Core.Bin<string>("Marko", "Ritter");

                
                FLogger.Log(LogType.Message, cast.Id.ToString());

//                cast.Name = (Core.Bin<string>)new string[] {"Marko", "Ritter"};
  //              cast.Palette = new Core.Bin<RGBAColor>(new RGBAColor(1, 0, 0, 1), new RGBAColor(0, 1, 0, 1));


    //            cast.Position = new Vector3D();
                cast.Health = 77;
                cast.Health++;



            }


        }
    }
}