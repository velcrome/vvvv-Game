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

        void Add(string name, object val);
        void Assign(string name, object val);

        Bin this[string name] { get; set; }
        IEnumerable<Bin> this[params string[] keys] { get; }

        T Face<T>() where T : class, IAgent;

        void Init(string name, Type type);

    }
}
