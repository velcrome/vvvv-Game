using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using ImpromptuInterface;
using VVVV.Pack.Game.Faces;


namespace VVVV.Pack.Game.Core
{
    [DataContract]
    public class Agent : DynamicObject, IAgent
    {
        #region pins & fields

        [DataMember]
		Dictionary<string, Bin> Data = new Dictionary<string, Bin>() ;
		
		[DataMember]
		public string Id {
			get;
			private set;
		}

        [DataMember]
        public ReturnCodeEnum ReturnCode
        {
            get;
            set;
        }

        [DataMember]
        public DateTime BirthTime
        {
            get;
            private set;
        }

        protected static Dictionary<string, Delegate> SkillMethods;
        public Dictionary<object, ArrayList> RunningNodes { get; private set; }

        #endregion

        public Agent()
        {
            if (SkillMethods == null) SkillMethods = new Dictionary<string, Delegate>();
            RunningNodes = new Dictionary<object, ArrayList>();
            Id = Guid.NewGuid().ToString();
            BirthTime = DateTime.Now;
        }

        #region duck casting

        public T Face<T>(bool makeSafe = true) where T : class, IAgent
        {
            var face = ImpromptuInterface.Impromptu.ActLike<T>(this);
            Init(typeof (T), makeSafe);
            return face;
        }

       

        #endregion

        #region DynamicObject

        // Calling an extension method defined in GameAPI. If a valid proxy delegate is found will be cached globally among all Agents
        // - has no runtime check if parameters of the cached method matches
        // - no overloading supported yet.
        // - caching might stop adopting changes during runtime, no?
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var methodName = binder.Name;

            if (!SkillMethods.ContainsKey(methodName)) {
                var methods = typeof (AgentAPI).GetMethods(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).First<MethodInfo>;
                
                if (methods.GetLength > 1)
                    throw new Exception(methodName +"()  cannot be overloaded yet", e);

                if (methods.GetLength = 0)
                    throw new Exception(methodName + "() not found. Define it in the partial class AgentAPI", e);

                MethodInfo extensionMethod = methods.First<MethodInfo>; 
                Expression[] parameters = extensionMethod.GetParameters()
                                            .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                                            .ToArray();  

                if (parameters.Length != args.Count() + 1) throw new Exception("Something seems wrong with the arguments for " + binder.Name + "()");

                var call = Expression.Call(extensionMethod, parameters);
                var func = Expression.Lambda(call, binder.Name, false, (ParameterExpression[])parameters).Compile();

                SkillMethods.Add(methodName, func);
            }

            var curry = Impromptu.Curry(SkillMethods[methodName], args.Length + 1);
            curry = curry(this);  // first argument is the instance of IAgent
            foreach (var arg in args) curry = curry(arg); // followed by whatever parameters you sent in
            result = curry;
            return true;
        }

        // If you try to get a field not defined in Agent, this method is called. 
        // Is not suited for initialisation, because binder.ReturnType is always object
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;

            if (!Data.ContainsKey(name))
            {
                    throw new Exception(name + " has not been initialized. Also no matching "+name+"() in AgentAPI could be found");
            }
            
            result = Data[name];
            return true;


        }

        // If you try to set a value of a property that is 
        // not defined in the class, this method is called. 
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Assign(binder.Name, value);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var type = binder.Type;

            if (type.IsInterface && type.IsAssignableFrom(typeof(IAgent))) {
                result = Impromptu.ActLike(this, binder.Type);
                return true;
            } 
            return base.TryConvert(binder, out result);
            
        }
        #endregion DynamicObject

        #region Quick Access.

        // you can use the ["key"] Operator to access a specific attribute 
        public Bin this[string name]
        {
            get
            {
                if (Data.ContainsKey(name)) return Data[name];
                else
                {
                    throw new Exception(name+" was not initialized, do so before accessing it with the Agent[string] operator.");
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

        
        // initializes all properties as listed in the Interface provided. Must inherit IAgent. Adds a first element in the Bin.
        public void Init(Type faceType, bool populateFirst = true)
        {
            if (!faceType.IsInterface || !typeof(IAgent).IsAssignableFrom(faceType)) throw new Exception("Something wrong with "+faceType.Name+"? ");

            var baseProperties = typeof(IAgent).GetProperties();             
            foreach (var prop in faceType.GetProperties())
            {
                if (!baseProperties.Contains(prop))
                {
                    Init(prop.Name, prop.PropertyType, populateFirst);
                }
             }
        }

        public void Init<T>(string name, bool populateFirst=false) 
        {
            Init(name, typeof(T), populateFirst);
        }

        public void Init(string name, Type type, bool populateFirst=false)
        {
            if (type.IsSubclassOf(typeof(Bin)) && type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            
            
            if (!TypeIdentity.Instance.ContainsKey(type)) throw new Exception(type.ToString() + " is not a supported Type in TypeIdentity.");
            if (!Data.ContainsKey(name) || Data[name].GetInnerType() != type)
            {
                Data[name] = Bin.New(type);
                if (populateFirst)
                {
                    var initFirst = Data[name].First;
                }
            }
        }

        public void Init(string name, object val)
        {
            Type type = typeof (object);
            
            if (TypeIdentity.Instance.ContainsKey(val.GetType()))
            {
                type = val.GetType();

            }
            else if (val is IEnumerable && !(val is string)) // odd thing about strings, they are IEnumerible...
            {
                var e = (IEnumerable) val;
                var num = e.GetEnumerator();
                num.MoveNext(); // necessary to get to [0]
                type = num.Current.GetType();
            }
            else
            {
                throw new Exception("Cannot determine type or add object " + val.ToString() + " to " + name +
                                    " because it is empty.");
            }

            Init(name, type, false);
            Data[name].Add(val);
        }

        public void Assign(string name, object val)
        {
            if (Data.ContainsKey(name)) Data.Remove(name);
            Init(name, val);
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
            var copy = new Agent();

            foreach (string name in Data.Keys)
            {
                Bin bin = Data[name];
                copy[name] = bin.Clone();

                // really deep cloning
                try
                {
                    for (int i = 0; i < bin.Count; i++)
                    {
                        bin[i] = ((ICloneable)bin[i]).Clone();
                    }
                }
                catch (Exception err)
                {
                    err.ToString(); // no warning
                    // not cloneble. so keep it
                }
            }

            return copy;
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
