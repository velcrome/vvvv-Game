using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using VVVV.Core.Logging;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Nodes
{

    #region PluginInfo
    [PluginInfo(Name = "Init",
        Category = "Game.Agent",
        Help = "",
        Tags = "")]
    #endregion PluginInfo
    public class InitAgentNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {
        #region fields & pins

        [Input("Agent")]
        protected Pin<Agent> FInput;
        
        [Input("Face", EnumName = "AllAgentFaces")]
        protected IDiffSpread<EnumEntry> FFace;

        [Input("Scan", IsBang = true, IsSingle = true)]
        protected ISpread<bool> FScan;

        [Output("Agent")]
        public Pin<Agent> FOutput;

        private Type[] AllAgentFaces;

        [Import()]
        public ILogger FLogger;
        #endregion

        public void OnImportsSatisfied()
        {
            Update();
        }
        
        private void Update() {

            var baseType = typeof(IAgent);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && (p != typeof(Agent)) );

            var names = new string[types.Count()];
            AllAgentFaces = types.ToArray();

            int i = 0;
            foreach (var face in AllAgentFaces)
            {
                names[i] = face.ToString();
                i++;
            }

            EnumManager.UpdateEnum("AllAgentFaces",baseType.ToString(), names);  
        }
        
        
        public void Evaluate(int SpreadMax)
        {
            if (FScan[0])
            {
                Update();
            }

            if (FInput.IsAnyEmpty())
            {
                SpreadMax = 0;
                FOutput.SliceCount = 0;
            }
            else
            {
                SpreadMax = FInput.SliceCount;
                FOutput.AssignFrom(FInput);
            }

            var baseProperties = typeof (IAgent).GetProperties();

            for (int i = 0; i < SpreadMax; i++)
            {
                var agent = FInput[i];
                
                // use all given faces and apply them to each Agent
                for (int j = 0; j < FFace.SliceCount; j++)
                {
                    Type face = AllAgentFaces[FFace[j].Index];
                    agent.Init(face);

                }
            }



        }


    }
}
