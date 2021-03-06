using System;
using System.Collections.Generic;
using System.IO;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Pack.Game.Core
    
{
    public class TypeIdentity : Dictionary<Type, string>
    {
        private static TypeIdentity _instance;
        public static TypeIdentity Instance
        {
            get { 
                if (_instance == null) _instance = new TypeIdentity();
                return _instance;
            }
            private set { throw new NotImplementedException(); }
        }

        public TypeIdentity()
	    {
            // This is the only place where you need to add new datatypes.
            // Requirements: 
            //- serializable (for json and general persisting)
            //- needs standard constructor, otherwise exempt in Bin.First
            //- should not implement IEnumerable

            Add(typeof(bool), "bool".ToLower());
            Add(typeof(int), "int".ToLower());
            Add(typeof(double), "double".ToLower());
            Add(typeof(float), "float".ToLower());
            Add(typeof(string), "string".ToLower());

            Add(typeof(RGBAColor), "Color".ToLower());
            Add(typeof(Matrix4x4), "Transform".ToLower());
            Add(typeof(Vector2D), "Vector2D".ToLower());
            Add(typeof(Vector3D), "Vector3D".ToLower());
            Add(typeof(Vector4D), "Vector4D".ToLower());

            Add(typeof(Stream), "Raw".ToLower()); //cannot be initialized when using autoinit by e.g. calling Faces<>
            Add(typeof(DateTime), "Time".ToLower());
            
	    }

        public Type FindKey(string typeName)
        {
            Type type = typeof(string);
            typeName = typeName.ToLower();
            foreach (Type key in Keys)
            {
                if (this[key] == typeName)
                {
                    type = key;
                }
            }
            return type;
        }

    }
}
