using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public interface IMoveableAgent : IAgent
    {
        Vector3D Position { get; set; } // Last Frame Position
        Vector3D Velocity { get; set; } // Last Frame Velocity

        Vector3D ForceSum { get; set; } // Sum of all Forces acting on the Agent

        int HistoryCount { get; set; } 

        Vector3D SetRandomPosition(double width); //optional header 
        Vector3D Move(double maxSpeed = double.PositiveInfinity, double agility = 1.0);
    }

    
    public static partial class AgentAPI
    {
        public static Vector3D Move(this IAgent agent, double maxSpeed = double.PositiveInfinity, double agility = 1.0)
        {
            var a = agent.Face<IMoveableAgent>();

            if (a.HistoryCount < 0) a.HistoryCount = 0;

            var velocity = a.Velocity;
            velocity += agility * (a.ForceSum);
            if (VMath.Polar(velocity).z > maxSpeed)
            {
                velocity = maxSpeed * ~velocity;
            }


            if (a.HistoryCount > 0)
            {
                var pos = a.PositionHistory();
                pos.Insert(0, velocity + a.Position);
                while (pos.Count > a.HistoryCount) pos.RemoveAt(a.HistoryCount);
            }
            else a.Position += velocity;


            if (a.HistoryCount > 0)
            {
                var vel = a.VelocityHistory();
                vel.Insert(0, velocity);
                while (vel.Count > a.HistoryCount) vel.RemoveAt(a.HistoryCount);
            }
            else a.Velocity = velocity;

            if (a.HistoryCount > 0)
            {
                var forceSum = a.ForceSumHistory();
                forceSum.Insert(0, velocity);
                while (forceSum.Count > a.HistoryCount) forceSum.RemoveAt(a.HistoryCount);
            }
            else a.ForceSum *= 0;

            return a.Position;
        }

        public static Bin<Vector3D> PositionHistory(this IAgent agent)
        {
            return (Bin<Vector3D>)agent["Position"];
        }
        public static Vector3D PositionHistory(this IAgent agent, int index)
        {
            return (Vector3D)agent["Position"][index];
        }

        public static Bin<Vector3D> VelocityHistory(this IAgent agent)
        {
            return (Bin<Vector3D>)(agent["Velocity"]);
        }
        public static Vector3D VelocityHistory(this IAgent agent, int index)
        {
            return (Vector3D)(agent["Velocity"][index]);
        }

        public static Bin<Vector3D> ForceSumHistory(this IAgent agent)
        {
            return (Bin<Vector3D>)agent["ForceSum"];
        }
        public static Vector3D ForceSumHistory(this IAgent agent, int index)
        {
            return (Vector3D)agent["ForceSum"][index];
        }

        
        public static Vector3D SetRandomPosition(this IAgent agent, double width)
        {
            var a = agent.Face<IMoveableAgent>();
            var v3 = VMath.RandomVector3D()*width; 
            a.Position = v3;
            return v3;
        }
    }
}
