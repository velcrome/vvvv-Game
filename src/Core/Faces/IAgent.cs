using System;
using System.Collections.Generic;
using VVVV.Pack.Game.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Pack.Game.Faces
{
    public interface IAgent : IComparable, ICloneable, IDisposable
    {
        DateTime BirthTime { get; set; }
        ReturnCodeEnum ReturnCode { get; set; }
        string Id { get; set; }

        IEnumerable<Bin> this[params string[] keys] { get; }

        void AddRunning(object node, Pin<BehaviorLink> pin);
        void RemoveRunning(object node, Pin<BehaviorLink> pin);


        void Add(string name, object val);
        void Assign(string name, object val);

    }
}
