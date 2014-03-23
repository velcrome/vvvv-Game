using System;
using System.Linq;
using System.Reflection;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.Pack.Game.Nodes;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.AgentNodes
{

    public abstract class AbstractFacedDynamicNode : AbstractDynamicNode
    {
        #region fields & pins

        [Input("Agent", Order = 0)]
        protected Pin<Agent> FInput;


        [Input("Face", EnumName = "AllAgentFaces", IsSingle = true, Order =  1)]
        protected IDiffSpread<EnumEntry> FFace;

        [Input("Scan", IsBang = true, IsSingle = true, Order = 2)]
        protected IDiffSpread<bool> FScan;


        [Output("Agent")]
        public Pin<Agent> FOutput;

        protected Type[] AllAgentFaces;
        protected bool firstFrame = true;

        #endregion

        public override void OnImportsSatisfied()
        {
            ScanForFaces(FScan);
            FConfig.Changed += HandleConfigChange;

            FScan.Changed += ScanForFaces;
            FFace.Changed += ApplyFace;
        }

        protected void ScanForFaces(IDiffSpread<bool> spread)
        {
            if (firstFrame || FScan[0])
            {

                var baseType = typeof (IAgent);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => baseType.IsAssignableFrom(p) && (p != typeof (Agent)));

                var names = new string[types.Count()];
                AllAgentFaces = types.ToArray();

                int i = 0;
                foreach (var face in AllAgentFaces)
                {
                    names[i] = face.ToString();
                    i++;
                }

                EnumManager.UpdateEnum("AllAgentFaces", baseType.ToString(), names);

                AgentSkills.Methods.Clear(); // Reset Method Cache
            }
        }

        protected void ApplyFace(IDiffSpread<EnumEntry> spread)
        {
            if (firstFrame) // during patch loading it is not useful to re-apply a Face. Just stick to whatever is in the config pin.
            {
                firstFrame = false;
                return;
            }

            if (AllAgentFaces.Length == 0) ScanForFaces(FScan);
            
            Type face = AllAgentFaces[FFace[0].Index];
            var fields = face.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            string config = "";
            foreach (var field in fields)
            {
                Type type = field.PropertyType;
                if (type.IsSubclassOf(typeof(Bin)))
                    type = type.GetGenericArguments()[0];
                
                if (TypeIdentity.Instance.ContainsKey(type))
                {
                    config += TypeIdentity.Instance[type];
                    config += " " + field.Name;
                    config += ", ";
                }
            }
            config = config.TrimEnd(new char[] { ' ', ',' });
            FConfig[0] = config;
        }


    }

}
