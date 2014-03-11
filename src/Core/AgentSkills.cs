using System;
using System.Collections.Generic;

namespace VVVV.Pack.Game
{
    public class AgentSkills
    {
        private static Dictionary<string, Delegate> _dict;
        public static Dictionary<string, Delegate> Methods
        {
            get 
            {
                if (_dict == null) _dict = new Dictionary<string, Delegate>();
                return _dict;
            } 
            private set { _dict = value; }
        }
    }
}
