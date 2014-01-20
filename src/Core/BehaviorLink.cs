using System.Collections.Generic;
using VVVV.Packs.GameElement.Base;

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