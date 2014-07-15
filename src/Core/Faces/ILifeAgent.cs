using System;


namespace VVVV.Pack.Game.Faces
{
    public enum LifeState
    {
        Spawn,
        Life,
        Decay,
    }
    
    public interface ILifeAgent : IAgent
    {
        int LifeState { get; set; }
        bool IsHit { get; set; }
    }

    public static partial class AgentAPI
    {

        public static void Hit(this IAgent agent, bool hit = true)
        {
            dynamic a = agent;
            a.IsHit = hit;
            if (hit)
            {
                a.HitTime = DateTime.Now;
            }

        }

    }
}
