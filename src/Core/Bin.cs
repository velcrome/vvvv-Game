using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;


namespace VVVV.Pack.Game.Core
{
    [Serializable]
    [JsonConverter(typeof(BinSerializer))]
    public class Bin<T> : Bin
    {
        public Bin()
        {
            
        }
        
        
        public Bin (params T[] values) 
        {
            foreach (var v in values)
            {
                Add(v);
            }

        }

        public Bin(IEnumerable<T> values)
        {
            Add(values);
        } 

        public override object First {
            get { return this[0]; }
            set
            {
                if (this.GetInnerType() == value.GetType())
                    this[0] = value;
                else throw new Exception(value.GetType().ToString() + " cannot be the first in Bin<" + this.GetInnerType() +">");
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

        /*
         * next two methods maybe unnecessary...
         */
        public static explicit operator Bin<T>(T[] s)  // explicit generic array to Bin conversion operator
        {
            return new Bin<T>(s);  
        }
        public static explicit operator Bin<T>(T s)  // explicit generic frist value to Bin conversion operator
        {
            return new Bin<T>(s);  // explicit conversion
        }
                


        // implicit conversion
        public static implicit operator T(Bin<T> sl)  
        {
            return (T)sl.First;  
        }


    }
    
    
    /// <summary>
	/// Description of 
	/// </summary>
	[Serializable]
    [JsonConverter(typeof(BinSerializer))]
	public abstract class Bin : ArrayList, ISerializable
	{
        public virtual Type GetInnerType() {
    		if (this.Count == 0) return typeof(object);
				else return this[0].GetType();
		}



        public virtual string GetTypeIdentity
        {
            get
            {
                if (this.Count == 0) return "Empty Spreadlist has neither a Type nor a TypeIdentity.";
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

        public virtual object First
        {
            get { return this[0]; }
            set
            {
                this[0] = value;
                if (!TypeIdentity.Instance.ContainsKey(value.GetType()))
                {
                    throw new Exception(value.GetType() + " is not a supported Type in TypeIdentity.cs");
                }
                if (this.GetInnerType() != value.GetType())
                {
                    throw new Exception(value.GetType().ToString() +" cannot be the first among " +this.GetInnerType());
                }
            }
        }


        protected Bin(params object[] values) : base()
        {
            foreach (var v in values)
            {
                this.Add(v);
            }

        }

        #region Serialization
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			for (int i=0;i<this.Count;i++)
			{
				info.AddValue(i.ToString(CultureInfo.InvariantCulture), this[i]);
				
			}
		}
        #endregion

        #region ISpread vs. ArrayList compromise

//      Think of this as a combined Add and AddRange with internal type checks.
        public override int Add(object val)
        {
            var index = this.Count;  //proper return as of ArrayList.Add()
//            if (val == null) return index;
            
            if (TypeIdentity.Instance.ContainsKey(val.GetType())) 
            {
                return base.Add(val);
            } 

            if (val is IEnumerable ) // string should be treated differently, but that is implicit in the last 4 lines
            {
                var enumerable = (IEnumerable)val;

                Type type;
                try
                {
                    var num = enumerable.GetEnumerator();

                    num.MoveNext();
                    type = num.Current.GetType();
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot add object " + enumerable.ToString() + " to Bin because cannot determine type. Maybe empty?", e);

                }
                if (this.GetInnerType() == type)
                {
                    foreach (var o in enumerable)
                    {
                        base.Add(o);
                    }
                    return index;
                }
            } 

            throw new Exception("Cannot add this value, it is neither a Enumeration of matching registered Type nor a matching Type.");
        }

        public void AssignFrom(IEnumerable enumerable) {
			this.Clear();
    		this.Add(enumerable);
            
		}

        // TODO: implement if necessary - with necessary Type checks in place
        protected new void Insert(int index, object value)
        {
            
        }

        // TODO: implementation not recommended, LINQ depends on IEnumerable
        protected new void InsertRange(int index, ICollection c)
        {

        }


        #endregion

        #region alternative constructor for runtime typing of the bin
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
                throw new Exception(type.ToString() + " is not a supported Type in TypeIdentity.cs");
            }
        }
        // creates aweful syntax
        //public static Bin FromIEnumerable(IEnumerable enumerable) 
        //{
        //    Type type;
        //    try
        //    {
        //        var num = enumerable.GetEnumerator();

        //        num.MoveNext();
        //        type = num.Current.GetType();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Cannot add object " + enumerable.ToString() + " to Bin because cannot determine type. Maybe empty?",e);

        //    }
            
        //    var bin = Bin.New(type);
        //    bin.AssignFrom(enumerable);
        //    return bin;
        //}

        #endregion


        #region Essentials
        public new Bin Clone()
        {
            Bin c = Bin.New(this.GetInnerType());
            c.AssignFrom(this);
            return c;
        }
        #endregion




	}
	
}
