#region usings
using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using VVVV.Pack.Game.Core;

using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V2.NonGeneric;

#endregion usings

namespace VVVV.Pack.Game.Nodes
{

    public abstract class DynamicNode : IPluginEvaluate, IPartImportsSatisfiedNotification
    {

        #region fields & pins

        [Input("Interface", DefaultString = "IAgent", IsSingle = true)] public IDiffSpread<string> FType;

        [Config("Configuration", DefaultString = "string Foo")] public IDiffSpread<string> FConfig;

        [Input("Verbose", Visibility = PinVisibility.OnlyInspector, IsSingle = true, DefaultBoolean = true)] public
            ISpread<bool> FVerbose;

        [Import()] protected ILogger FLogger;

        [Import()] protected IIOFactory FIOFactory;

        protected Dictionary<string, IIOContainer> FPins = new Dictionary<string, IIOContainer>();
        protected Dictionary<string, string> FTypes = new Dictionary<string, string>();

        protected int FCount = 2;

        #endregion fields & pins

        #region type

        public DynamicNode()
        {
        }


        protected bool TypeUpdate()
        {

            // Todo: find all params in the Interface 

            if (FType[0].ToLower() == "none") return false;


            return true;
        }

        #endregion type


        #region pin management



        public void OnImportsSatisfied()
        {
            FConfig.Changed += HandleConfigChange;
        }

        protected virtual void HandleConfigChange(IDiffSpread<string> configSpread)
        {
            FCount = 0;
            List<string> invalidPins = FPins.Keys.ToList();

            string[] config = configSpread[0].Trim().Split(',');
            foreach (string pinConfig in config)
            {
                string[] pinData = pinConfig.Trim().Split(' ');

                try
                {
                    string typeName = pinData[0].ToLower();
                    string name = pinData[1];

                    bool create = false;
                    if (FPins.ContainsKey(name) && FPins[name] != null)
                    {
                        invalidPins.Remove(name);

                        if (FTypes.ContainsKey(name))
                        {
                            if (FTypes[name] != typeName)
                            {
                                FPins[name].Dispose();
                                FPins[name] = null;
                                create = true;
                            }

                        }
                        else
                        {
                            // key is in FPins, but no type defined. should never happen
                            create = true;
                        }
                    }
                    else
                    {
                        FPins.Add(name, null);
                        create = true;
                    }

                    if (create)
                    {
                        Type type = typeof (string);
                        foreach (Type key in TypeIdentity.Instance.Keys)
                        {
                            if (TypeIdentity.Instance[key] == typeName)
                            {
                                type = key;
                            }
                        }

                        IOAttribute attr = DefinePin(name, type);
                            // each implementation of DynamicNode must create its own InputAttribute or OutputAttribute (
                        Type pinType = typeof (ISpread<>).MakeGenericType((typeof (ISpread<>)).MakeGenericType(type));
                            // the Pin is always a binsized one
                        FPins[name] = FIOFactory.CreateIOContainer(pinType, attr);

                        FTypes.Add(name, typeName);
                    }
                    FCount += 2; // total pincount. always add two to account for data pin and binsize pin
                }
                catch (Exception ex)
                {
                    var e = ex;
                    FLogger.Log(LogType.Debug, ex.ToString());
                    FLogger.Log(LogType.Debug, "Invalid Descriptor in Config Pin");
                }
            }
            foreach (string name in invalidPins)
            {
                FPins[name].Dispose();
                FPins.Remove(name);
                FTypes.Remove(name);
            }
        }

        #endregion pin management

        #region tools

        protected VVVV.PluginInterfaces.V2.NonGeneric.ISpread ToISpread(IIOContainer pin)
        {
            return (VVVV.PluginInterfaces.V2.NonGeneric.ISpread) (pin.RawIOObject);
        }

        protected VVVV.PluginInterfaces.V2.NonGeneric.IDiffSpread ToIDiffSpread(IIOContainer pin)
        {
            return (VVVV.PluginInterfaces.V2.NonGeneric.IDiffSpread) (pin.RawIOObject);
        }

        protected VVVV.PluginInterfaces.V2.ISpread<T> ToGenericISpread<T>(IIOContainer pin)
        {
            return (VVVV.PluginInterfaces.V2.ISpread<T>) (pin.RawIOObject);
        }

        #endregion tools

        #region abstract methods

        protected abstract IOAttribute DefinePin(string name, Type type);

        public abstract void Evaluate(int SpreadMax);

        #endregion abstract methods
    }

    #region PluginInfo
    [PluginInfo(Name = "Split", AutoEvaluate = true, Category = "Game.Agent",
        Help = "Splits all Agents into custom dynamic pins", Tags = "Dynamic, Bin")]
    #endregion PluginInfo

    public class SplitAgentNode : DynamicNode
    {
        [Input("Input")] 
        protected Pin<Agent> FInput;
        
        [Output("Timestamp", AutoFlush = false)] 
        protected ISpread<string> FTimeStamp;

        protected override IOAttribute DefinePin(string name, Type type)
        {
            var attr = new OutputAttribute(name);
            attr.BinVisibility = PinVisibility.Hidden;
            attr.AutoFlush = false;

            attr.Order = FCount;
            attr.BinOrder = FCount + 1;
            return attr;
        }

        public override void Evaluate(int SpreadMax)
        {
            TypeUpdate();
            
            SpreadMax = FInput.SliceCount;
            if (FInput.IsAnyEmpty()) SpreadMax = 0;


            foreach (string pinName in FPins.Keys)
            {
                ToISpread(FPins[pinName]).SliceCount = SpreadMax;
                FTimeStamp.SliceCount = SpreadMax;
            }

            for (int i = 0; i < SpreadMax; i++)
            {
                Agent agent = FInput[i];

                FTimeStamp[i] = agent.BirthTime.ToString();
                FTimeStamp.Flush();

                foreach (string name in FPins.Keys)
                {
                    var spread = (ISpread) ToISpread(FPins[name])[i];
                    var bin = agent[name];

                    var count = bin.Count;
                    spread.SliceCount = count;
                    for (int j = 0; j < count; j++)
                    {
                        spread[j] = bin[j];
                    }
                    ToISpread(FPins[name]).Flush();
                }
            }
        }


        #region PluginInfo
        [PluginInfo(Name = "Set", Category = "Game.Agent", Help = "Adds or edits an Agent",
            Tags = "Dynamic, Bin, velcrome")]
        #endregion PluginInfo
        public class SetAgentNode : DynamicNode
        {
            [Input("Input")] 
            protected Pin<Agent> FInput;

            [Output("Output", AutoFlush = false)] 
            protected Pin<Agent> FOutput;

            protected override IOAttribute DefinePin(string name, Type type)
            {
                var attr = new InputAttribute(name);
                attr.BinVisibility = PinVisibility.Hidden;
                attr.BinSize = -1;
                attr.Order = FCount;
                attr.BinOrder = FCount + 1;
                //                attr.AutoValidate = false;  // need to sync all pins manually
                return attr;
            }


            public override void Evaluate(int SpreadMax)
            {
                if (FInput.IsAnyEmpty())
                {
                    FOutput.SliceCount = 0;
                    FOutput.Flush();
                    return;
                }
                SpreadMax = FInput.SliceCount;

                for (int i = 0; i < SpreadMax; i++)
                {
                    Agent agent = FInput[i];
                    foreach (string name in FPins.Keys)
                    {
                        var pin = (IEnumerable) ToISpread(FPins[name])[i];
                        agent.Assign(name, pin);
                    }
                }

                FOutput.AssignFrom(FInput);
                FOutput.Flush();
            }
        }
    }



}