using System;
using System.Collections.Generic;
using VVVV.Pack.Game.Core;


namespace VVVV.Pack.Game.Faces
{
    public interface IAgent : IComparable, ICloneable, IDisposable
    {
        DateTime BirthTime { get; set; }
        ReturnCodeEnum ReturnCode { get; set; }
        string Id { get; set; }

        IEnumerable<Bin> this[params string[] keys] { get; }

//        void AddRunning(object node, Pin<BehaviorLink> pin);
 //       void RemoveRunning(object node, Pin<BehaviorLink> pin);
        void AddRunning(object node, object pin);
        void RemoveRunning(object node, object pin);


        void Add(string name, object val);
        void Assign(string name, object val);

    }
}
