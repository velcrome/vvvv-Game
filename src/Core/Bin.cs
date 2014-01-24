using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;



namespace VVVV.Pack.Game.Core
{
    [Serializable]
    public class Bin<T> : Bin
    {
        public Bin()
        {
            
        }
        
        
        public Bin (params T[] values) 
        {
            foreach (var v in values)
            {
                this.Add(v);
            }

        } 
        
        public override Type GetInnerType()
        {
            return typeof(T);
        }

        public override string GetTypeIdentity
        {
            get
            {
                var supportedTypes = TypeIdentity.Instance;
                if (supportedTypes.ContainsKey(typeof(T)))
                    return supportedTypes[typeof(T)];
                else return "Unsupported Type in TypeIdentity.";
            }
        }

        public new T[] ToArray()
        {
            return (T[])this.ToArray(typeof(T));
        }


        public static explicit operator Bin<T>(T[] s)  // explicit array to Spreadlist conversion operator
        {
            Bin<T> sl = (Bin<T>)Bin.New(typeof(T));  // explicit conversion
            sl.AssignFrom(s);
            return sl;
        }


        public static explicit operator Bin<T>(T s)  // explicit string to Spreadlist conversion operator
        {
            Bin<T> sl = (Bin<T>)Bin.New(typeof(T));  // explicit conversion
            sl.Add(s);
            return sl;
        }

        public static implicit operator T(Bin<T> sl)  // implicit digit to byte conversion operator
        {
            return (T)sl[0];  // implicit conversion
        }

   

    }
    
    
    /// <summary>
	/// Description of 
	/// </summary>
	[Serializable]
	public class Bin : ArrayList, ISerializable
	{
        public virtual Type GetInnerType() {
    		if (this.Count == 0) return typeof(object);
				else return this[0].GetType();
		}

        public virtual string GetTypeIdentity
        {
            get
            {
                if (this.Count == 0) return "Empty Spreadlist has neither a Type nor a TypeIdentity";
                else
                {
                    var type = this[0].GetType();
                    var supportedTypes = TypeIdentity.Instance;
                    if (supportedTypes.ContainsKey(type))
                        return supportedTypes[type];
                    else return "Unsupported Type in TypeIdentity.";
                }
            }
        }


        //public SpreadList()
        //{
        //    throw new Exception("Should not create an empty SpreadList");
        //}

        public Bin(params object[] values) : base()
        {
            foreach (var v in values)
            {
                this.Add(v);
            }

        } 

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			for (int i=0;i<this.Count;i++)
			{
				info.AddValue(i.ToString(CultureInfo.InvariantCulture), this[i]);
				
			}
		}
		
		public void AssignFrom(IEnumerable source) {
			this.Clear();
            foreach (object o in source) {
				this.Add(o);
			}
			
		}
		
		public new Bin Clone() {
			Bin c = Bin.New(this.GetType());
			c.AssignFrom(this);
            
			return c;
		}

        public static Bin New(Type type)
        {
            Type spreadType = typeof(Bin<>).MakeGenericType(type);
            if (TypeIdentity.Instance.ContainsKey(type))
            {
                var sl = Activator.CreateInstance(spreadType);
                return (Bin)sl;
            }
            else
            {
                throw new Exception(type.ToString() +" is not a valid Type, it needs to be listed in TypeIdentity.cs");
            }
        }


        
    }
	
}
