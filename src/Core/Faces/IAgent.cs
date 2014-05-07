using System;
using System.Collections.Generic;
using VVVV.Pack.Game.Core;


namespace VVVV.Pack.Game.Faces
{
    public interface IAgent : IComparable, ICloneable, IDisposable
    {
        DateTime BirthTime { get;  }

        ReturnCodeEnum ReturnCode { get; set; }
        string Id { get;  }

        Bin this[string name] { get; set; }
        IEnumerable<Bin> this[params string[] keys] { get; }

        T Face<T>(bool makeSafe = true) where T : class, IAgent;

        void Init(string name, object val);
        void Init(string name, Type type, bool populateFirst = false);
        
        // face must be IAgent or any other interface extending IAgent. will init all Properties in one go
        // will be automatically called by Face<>()
        void Init(Type face, bool populateFirst = true);

        bool Killed { get; set; }
    }
}
