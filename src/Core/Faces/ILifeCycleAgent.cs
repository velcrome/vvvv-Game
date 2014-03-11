using System;
using VVVV.Utils.VMath;


namespace VVVV.Pack.Game.Faces
{
    public enum LifeStatus
    {
        Fledging,
        Alive,
        Dying
    }
    
    
    public interface ILifeCycleAgent : IAgent
    {
        string LifeStatus { get; set; }
        DateTime TimeOfDeath { get; set; }

        TimeSpan Age(); //optional header 
        TimeSpan RestLifeTime();
        TimeSpan TotalLifeTime();
        double LifeTimeProgress();
    }

    public static partial class AgentAPI
    {
        public static LifeStatus GetStatus(this IAgent agent)
        {
            var a = agent.Face<ILifeCycleAgent>();
            LifeStatus status;
            Enum.TryParse<LifeStatus>(a.LifeStatus, false, out status);

            return status;
        }

        public static void SetStatus(this IAgent agent, LifeStatus status)
        {
            var a = agent.Face<ILifeCycleAgent>();
            a.LifeStatus = status.ToString();
        }
        
        public static TimeSpan Age(this IAgent agent)
        {
            return agent.BirthTime - DateTime.Now;
        }

        public static TimeSpan RestLifeTime(this IAgent agent)
        {
            var a = agent.Face<ILifeCycleAgent>();
            if (a.TimeOfDeath < DateTime.Now)
            {
                return new TimeSpan(0);
            } else
            {
                return a.TimeOfDeath - DateTime.Now;
            }
        }

        public static TimeSpan TotalLifeTime(this IAgent agent)
        {
            var a = agent.Face<ILifeCycleAgent>();
            if (a.TimeOfDeath < a.BirthTime)
            {
                return new TimeSpan(0);
            }
            else
            {
                return a.TimeOfDeath - a.BirthTime;
            }            
        }

        public static double LifeTimeProgress(this IAgent agent)
        {
            var a = agent.Face<ILifeCycleAgent>();
            var total = a.TotalLifeTime();

            if (total <= new TimeSpan(0)) return 0.9999999; // no valid TimeOfDeath? -> living on the edge
                else return ((double)a.Age().Ticks / total.Ticks);
        }

    }
}
