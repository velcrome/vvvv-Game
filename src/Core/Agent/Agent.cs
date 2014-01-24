using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VVVV.Packs.Game;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Base
{
    [DataContract]
    public class Agent : DynamicObject, IComparable<Agent>
    {
		[DataMember]
		Dictionary<string, SpreadList> Data = new Dictionary<string, SpreadList>();
		
		[DataMember]
		public string Id {
			get;
			private set;
		}

        [DataMember]
        public bool KissOfDeath
        {
            get; set; 
        }

        [DataMember]
        public ReturnCodeEnum ReturnCode { 
            get;
            set;
        }

        [DataMember]
        public DateTime BirthTime
        {
            get; private set; 
        }


        public Dictionary<object, List<Pin<BehaviorLink>>> RunningNodes = new Dictionary<object, List<Pin<BehaviorLink>>>();
		
		public Agent()
		{
		    Id = Guid.NewGuid().ToString();
		    BirthTime = DateTime.Now;
		    KissOfDeath = false;
		}


        #region DynamicObject

        // If you try to get a value of a property  
        // not defined in the class, this method is called. 
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;

            // If the property name is found in a dictionary, 
            // set the result parameter to the property value and return true. 
            // Otherwise, return false. 
            SpreadList spreadList = null;
            bool success = Data.TryGetValue(name, out spreadList);
            result = spreadList;
            return success;
        }

        // If you try to set a value of a property that is 
        // not defined in the class, this method is called. 
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Data[binder.Name] = (SpreadList)value;
            return true;
        }
 
        #endregion DynamicObject

        #region SpreadList Access

        // you can use the ["key"] Operator to access a specific attribute 
        public SpreadList this[string name]
        {
            get
            {
                if (Data.ContainsKey(name)) return Data[name];
                else return null;
            }
            set { Data[name] = (SpreadList)value; }
        }

        // you can use the ["key0", "key1", "key2"] Operator to retrieve a range of specific attributes
        public IEnumerable<SpreadList> this[params string[] keys]
        {
            get
            {
                return keys.Select(key => Data[key]).AsEnumerable();
            }
        }

        // you can add any object, it will be automatically be wrapped by a SpreadList. 
        // that is unless you try to add a SpreadList 
        public void Add(string name, object val)
        {
            if (val is SpreadList)
            {
                if (Data.ContainsKey(name)) Data[name].AddRange((SpreadList) val);
                    else Data.Add(name, (SpreadList)val);
            }
            else
            {
                if (!Data.ContainsKey(name)) Data.Add(name, new SpreadList());
                Data[name].Add(val);
            }
        }

        public void Assign(string name, object val)
        {
            if (!Data.ContainsKey(name))
                Data.Add(name, new SpreadList());
            else Data[name].Clear();

            Add(name, val);
        }

        public void AssignFrom(string name, IEnumerable en)
        {
            if (!Data.ContainsKey(name))
                Data.Add(name, new SpreadList());
                else Data[name].Clear();

            foreach (object o in en) Data[name].Add(o);
        }

        public void AddFrom(string name, IEnumerable en)
        {
            if (!Data.ContainsKey(name))
                Data.Add(name, new SpreadList());

            foreach (object o in en) Data[name].Add(o);
        }
        #endregion SpreadList Access

        #region Access Default
        public SpreadList GetOrDefault(string name, object defaultValue)
        {
            if (!Data.ContainsKey(name))
            {
                Add(name, defaultValue);                
            }
            return Data[name];
        }

        public object GetFirstOrDefault(string name, object defaultValue)
        {
            return GetOrDefault(name, defaultValue)[0];
        }
        #endregion Access Default

        #region Essentials
        public object Clone()
        {
            var m = new Agent();

            foreach (string name in Data.Keys)
            {
                SpreadList list = Data[name];
                m.Add(name, list.Clone());

                // really deep cloning
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = ((ICloneable)list[i]).Clone();
                    }
                }
                catch (Exception err)
                {
                    err.ToString(); // no warning
                    // not cloneble. so keep it
                }
            }

            return m;
        }

        public int CompareTo(Agent other)
        {
            if (other == null) return 0;
                else return BirthTime.CompareTo(other.BirthTime);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Agent "+  (BirthTime.Second*1000+BirthTime.Millisecond).ToString()+" -> " + ReturnCode +"\n");
            foreach (string name in Data.Keys)
            {

                sb.Append(" " + name + " \t: ");
                foreach (object o in Data[name])
                {
                    sb.Append(o.ToString() + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion Essentials

        #region Running Cache
        public void AddRunning(object node, Pin<BehaviorLink> pin)
        {
            var list = RunningNodes.ContainsKey(node) ? RunningNodes[node] : new List<Pin<BehaviorLink>>();
            if (!list.Contains(pin)) list.Add(pin);
            RunningNodes[node] = list;
        }

        public void RemoveRunning(object node, Pin<BehaviorLink> pin)
        {
            var list = RunningNodes.ContainsKey(node) ? RunningNodes[node] : new List<Pin<BehaviorLink>>();
            if (list.Contains(pin)) list.Remove(pin);
            if (list.Count == 0) 
                RunningNodes.Remove(node);
                else RunningNodes[node] = list;
        }
        #endregion
    }
}
