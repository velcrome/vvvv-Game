using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ImpromptuInterface;
using VVVV.Pack.Game.Faces;



namespace VVVV.Pack.Game.Core
{
    [DataContract]
    public class Agent : DynamicObject, IDisposable, IComparable
    {
        #region pins & fields

        [DataMember]
		Dictionary<string, Bin> Data = new Dictionary<string, Bin>() ;
		
//		[DataMember]
		public string Id {
			get;
			private set;
		}

  //      [DataMember]
        public bool Dispose
        {
            get; set; 
        }

    //    [DataMember]
        public ReturnCodeEnum ReturnCode
        {
            get;
            set;
        }

      //  [DataMember]
        public DateTime BirthTime
        {
            get;
            private set;
        }


        public Dictionary<object, ArrayList> RunningNodes { get; private set; }

        #endregion

        public Agent()
        {
            RunningNodes = new Dictionary<object, ArrayList>();
            Id = Guid.NewGuid().ToString();
            BirthTime = DateTime.Now;
            Dispose = false;
        }

        #region duck casting

        public T Face<T>() where T : class, IAgent
        {
            return Impromptu.ActLike<T>(this);
        //    return null;
        }


        #endregion

        #region DynamicObject

        // If you try to get a value of a property  
        // not defined in the class, this method is called. 
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool success = false; 
            string name = binder.Name;
            result = null;

            // If the property name is not found in a dictionary, it was most likely not initialized
            // we cannot add it on the fly, because we do not know its type.
            // binder.ReturnType is always System.Object

            if (!Data.ContainsKey(name))
            {
                throw new Exception(name + " has not been initialized!");
//                Data[name] = SpreadList.New(binder.ReturnType);
            } else
            {
                success = true;
                result = Data[name];
            }
            return success;


        }

        // If you try to set a value of a property that is 
        // not defined in the class, this method is called. 
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Assign(binder.Name, value);
            return true;
        }
        #endregion DynamicObject

        #region DynamicCast
        //public override bool TryConvert(
        //    ConvertBinder binder, out object result)
        //{
        //    // Converting to string.  
        //    binder.Explicit.
        //    if (TypeIdentity.Instance.ContainsKey(binder.Type) )
        //    {
        //        result = Data["t"];
        //        return true;
        //    }

        //    // Converting to integer. 
        //    if (binder.Type == typeof(int))
        //    {
        //        result = Data["Numeric"];
        //        return true;
        //    }

        //    // In case of any other type, the binder  
        //    // attempts to perform the conversion itself. 
        //    // In most cases, a run-time exception is thrown. 
        //    return base.TryConvert(binder, out result);
        //}


        #endregion


        #region Bin Quick Access. No range modulo

        // you can use the ["key"] Operator to access a specific attribute 
        public Bin this[string name]
        {
            get
            {
                if (Data.ContainsKey(name)) return Data[name];
                else
                {
                    Add(name, null);
                    return Data[name];
                }
            }
            set { Data[name] = (Bin)value; }
        }

        // you can use the ["key0", "key1", "key2"] Operator to retrieve a range of specific attributes
        public IEnumerable<Bin> this[params string[] keys]
        {
            get
            {
                return keys.Select(key => Data[key]).AsEnumerable();
            }
        }


        public void Init<T>(string name)
        {
            if (TypeIdentity.Instance.ContainsKey(typeof(T)))
            {
                Data[name] = new Bin<T>();
            }
            else throw new Exception(typeof(T).ToString() + " is not a supported Type in TypeIdentity.");
        }

        // you can add any object defined in TypeIdentity, it will be automatically be wrapped by a Bin<> 
        // alternatively all instances in the enumerable will be added, e.g. from a linq query
        public void Add(string name, object val)
        {
            Type type = typeof(object);
            if (val is Type)
            {
                type = (Type)val;
            }
            else if (TypeIdentity.Instance.ContainsKey(val.GetType()))
            {
                type = val.GetType();

            } 
            else if (val is IEnumerable && !(val is string)) // odd thing about strings...
            {
                var e = (IEnumerable)val;
                var num = e.GetEnumerator();
                num.MoveNext();  // necessary to get to [0]
                type = num.Current.GetType();
            } else
            {
                throw new Exception("Cannot determine type or add object "+val.ToString()+ " to "+name+" because it is empty.");
            }


            if (!Data.ContainsKey(name)) 
                Data.Add(name, Bin.New(type));
            else
            {
                if (Data[name].GetInnerType() != type) throw new Exception("Tried to add "+type+" into a Bin<"+Data[name].GetInnerType()+">.");
                
            }
            var bin = Data[name];

            
            bin.Add(val);
        }

        public void Assign(string name, object val)
        {
            Type type;
            if (val is IEnumerable && !(val is string))
            {
                var e = (IEnumerable) val;
                var num = e.GetEnumerator();
                num.MoveNext();
                type = num.Current.GetType();
            } else
            {
                type = val.GetType();
            }

            
            if (!Data.ContainsKey(name))
                Data.Add(name, Bin.New(type));
                else Data[name].Clear();

            var spread = Data[name];

            if (val is IEnumerable && !(val is string) ) // unfortunately string is IEnumerable
            {
                foreach (var o in (IEnumerable)val)
                {
                    spread.Add(o);
                }
            } else spread.Add(val);
        }

        #endregion SpreadList Access

        #region Essentials

        public int CompareTo(object other)
        {
            if (other == null) return 0;
            else if (other is Agent)
                return BirthTime.CompareTo(((Agent)other).BirthTime);
            else return 0;
        }
        
        public object Clone()
        {
            var m = new Agent();

            foreach (string name in Data.Keys)
            {
                Bin list = Data[name];
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Agent\n");
            foreach (string name in Data.Keys)
            {

                sb.Append(" " + name +" ("+ TypeIdentity.Instance[Data[name].GetInnerType()]+") \t: ");
                foreach (object o in Data[name])
                {
                    string typeIdentity = TypeIdentity.Instance[o.GetType()];
                    sb.Append(o.ToString() + "["+typeIdentity+"], ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        void IDisposable.Dispose()
        {
 //           foreach(var spread in Data.Values) spread.Clear();
            Data.Clear();
        }

        #endregion Essentials

        #region Running Cache
        public void AddRunning(object node, object pin)
        {
            var list = RunningNodes.ContainsKey(node) ? RunningNodes[node] : new ArrayList();
            if (!list.Contains(pin)) list.Add(pin);
            RunningNodes[node] = list;
        }

        public void RemoveRunning(object node, object pin)
        {
            var list = RunningNodes.ContainsKey(node) ? RunningNodes[node] : new ArrayList();
            if (list.Contains(pin)) list.Remove(pin);
            if (list.Count == 0)
                RunningNodes.Remove(node);
            else RunningNodes[node] = list;
        }
        #endregion
    }

}
