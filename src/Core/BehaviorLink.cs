using System.Collections.Generic;
using System.Linq;
using VVVV.Pack.Game.Base;

namespace VVVV.Pack.Game
{
    public class BehaviorLink
    {
        public List<Agent> Agents;

        public BehaviorLink(List<Agent> agentList)
        {
            this.Agents = agentList;
        }

        public override  string ToString()
        {
            return base.ToString() + "\nAgents:" + Agents.Count.ToString();

        }
    }
}