using System;
using VVVV.Pack.Game.Core;
using VVVV.Pack.Game.Faces;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Nodes
{
    #region PluginInfo

    [PluginInfo(Name = "Moveable",
        Category = "Game",
        Help = "Manages IMoveableAgent",
        Tags = "Agent")]
    #endregion PluginInfo
    public class MoveableAgentNode : IPluginEvaluate
    {
        #region fields & pins

        [Input("Agent")]
        protected Pin<Agent> FInput;

        [Input("History", DefaultValue = 5.0, AutoValidate = false)]
        protected ISpread<int> FHistoryCount;

        [Output("Agent", AutoFlush = false)]
        public Pin<Agent> FOutput;

        [Output("Transform", AutoFlush = false)]
        public Pin<Matrix4x4> FTransform;

        [Output("Position", AutoFlush = false)]
        public ISpread<ISpread<Vector3D>> FPosition;

        [Output("Velocity", AutoFlush = false)]
        public ISpread<ISpread<Vector3D>> FVelocity;

        [Output("ForceSum", AutoFlush = false)]
        public ISpread<ISpread<Vector3D>> FForceSum;

        #endregion

        public void Evaluate(int SpreadMax)
        {
            SpreadMax = FInput.SliceCount; 

            if (FInput.IsAnyInvalid()) SpreadMax = 0;
            
            FHistoryCount.Sync();
        	
        	FTransform.SliceCount = SpreadMax;
            FPosition.SliceCount = SpreadMax;
            FVelocity.SliceCount = SpreadMax; 
            FForceSum.SliceCount = SpreadMax;

            for (int i = 0; i < SpreadMax; i++)
            {
                IAgent agent = FInput[i];
                agent["HistoryCount"].First = FHistoryCount[i];

                FPosition[i].AssignFrom(agent.PositionHistory());
                FVelocity[i].AssignFrom(agent.VelocityHistory());
                FForceSum[i].AssignFrom(agent.ForceSumHistory());

                var rot = VMath.PolarVVVV(agent.VelocityHistory(0));
                var transform = VMath.RotateY(VMath.Pi / 2) * VMath.Rotate(rot) * VMath.Translate(agent.PositionHistory(0));
                

                FTransform[i] = transform;
            }
            FOutput.AssignFrom(FInput);
            
            
            FTransform.Flush();
            FPosition.Flush();
            FVelocity.Flush();
            FForceSum.Flush();

            FOutput.Flush();
        }




    }
}