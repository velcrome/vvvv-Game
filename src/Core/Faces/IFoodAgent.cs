using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVVV.Pack.Game.Faces;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Faces
{
    public enum FoodStatus
    {
        Starving,
        Hungry,
        Normal,
        Replete
    }
    
    public interface IFoodAgent : IMoveableAgent
    {
        double Health { get; set; } // 0 to 1
        double Energy { get; set; } // 0 to 1

        string FoodStatus { get; set; }

        void Work(); // automatically digests
        void Feed(); // automatically digests
        void Digest(); // calculates Health and FoodStatus

    }

    public static partial class AgentAPI
    {
        
        public static double Digest(this IAgent agent)
        {
            dynamic a = agent;

            double energy = a["Energy"].First;
            energy = VMath.Clamp(energy, 0, 1);
            a["Energy"].First = energy;

            //  https://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427e2uth181l2b            
            a["Health"].First = (Math.Tanh(energy * 4.0 - 2) + 1) / 2.0; 

            var health = VMath.Clamp(a["Health"].First, 0, 1) * 4;
            
            switch ((int)Math.Floor(health))
            {
                case 0:
                    a.FoodStatus = FoodStatus.Starving.ToString();
                    break;
                case 1:
                    a.FoodStatus = FoodStatus.Hungry.ToString();
                    break;
                case 2:
                    a.FoodStatus = FoodStatus.Normal.ToString();
                    break;
                case 3:
                    a.FoodStatus = FoodStatus.Replete.ToString();
                    break;
            }
                
            return health;
        }

        
        public static void Work(this IAgent agent, double efficiency = 0.1)
        {
            dynamic a = agent;
            a.Energy -= a.Velocity.Length * efficiency;
            Digest(a);
        }

        public static void Feed(this IAgent agent, double foodAmount)
        {
            dynamic a = agent;
            a.Energy.First += foodAmount;
            Digest(a);
        }    
    
    }
}
