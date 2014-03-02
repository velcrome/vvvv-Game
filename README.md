vvvv-Game
=========

This pack is a framework for quickly patching and developing [Behavior Trees](http://en.wikipedia.org/wiki/Behavior_Trees#Game_A.I_Modeling) in [vvvv](http://www.vvvv.org). 

A central piece of this development is the [Agent](src/Core/Agent.cs), a custom dynamic c# type. 
 

Background
============

Conceived during development of commercial games this breed of Behavior Tree (BT) proved itself helpful when designing AIs competing against players (halo) or coexisting with the player(spore). 

The AIs were called Autonomous Agents or short Agents. These BTs offered a similar freedom as HFSM (what vvvv can clumsily model as a patch of framedelayed Automata), but were a lot easier to modularize and kept stable even at more complex forms of behavior, in realtime and in teams of interacting Agents. Also, [timed character motion](http://www.youtube.com/watch?v=UoQetSc3p30) could be streamlined with the Agents decisions and actions.

Alex J. Champandard has had [nice](http://twvideo01.ubm-us.net/o1/vault/gdc10/slides/ChampandardDaweHernandezCerpa_BehaviorTrees.pdf) [overviews](http://aigamedev.com/open/article/bt-overview/) on them for quite some time. [Björn Knafla](http://www.altdevblogaday.com/2011/02/24/introduction-to-behavior-trees/) has another good series on them.

This implementation strives to make game development in vvvv easier by providing a proper base for developing your own custom behavior nodes. It solves a lot of the challenges presented in [this thread](http://vvvv.org/forum/vvvv-particles-library)

To allow for all that high-level functionality, the framework is purley CPU-based, so do not expect the same scalability you might get from GPU based particle systems. Be smart about it, and use the GPU for gloss, and vvvv-Game for interactive and complex rule-based behavior.

Getting Started
===============

Every BT starts at its root called `Store (Game)`, because it stores all the Agents that inhabit the BT. You can add new Agents to it (for example with `Birth (Game)`), sniper individual Agents, and add behavior nodes.

The primary plan groups `Selector` [?](http://aigamedev.com/open/article/selector/), `Sequence` [?](http://aigamedev.com/open/article/sequence/), `Parallel` [?](http://aigamedev.com/open/article/parallel/) are fully functional as compiled [plugins](src/Nodes/CompositeNodes). Depending on the Agent's ReturnCode, it might or might not traverse up a pin (every Agent always has its own way in the tree, thats why they called them autonomous).

There are plugin templates such as `ActionTemplate (Game)`,  `ConditionTemplate (Game)`,  `DecoratorTemplate (Game)` [?](http://aigamedev.com/open/rticle/decorator/). If you feel like patching, you can clone `TemplatePatch (Game)`. 


Agent with many Faces 
=============================

Each Agent basically has its own mind in this Game

It can be metaphored with a peculiar blackboard to scribble data freely on it, all to maintain its own physical state (*int Health, Vector3Dd Position), its prior perception of the current world (Vector3D EnemyTarget, double DangerLevel*) and its memory on decisions of what to strive for next (*Vector3D[] PathToTarget, Time NextTimeToCheckForEnemies*).

You can see the blackboard with `ToString (object)`, `ToString (Json)` or `Split (Agent)`, with `Set (Agent)` you can modify the properties. 
In good tradition with vvvv, any data you add as a property can be one or many items, taking full advantage of spreading. Possible items are basic types such as *double*, *bool*, *int*, *float* and *string*, but also more advanced ones like *Raw*, *Color*, *Vector2D*, *Vector3D*, *Vector4D*, *Transform*, and *Time*

In C# terms, Agent is a fully [dynamic](http://www.codeproject.com/Articles/69407/The-Dynamic-Keyword-in-C-4-0) class. This means you can use c# to script really quick, if you are okay with loosing Code Completion and safe type handling. 

The implemented Agent in this framework can adopt to any mask, or what I call a Face, from the very simple (just a [name](src/Core/Faces/INamedAgent.cs)) to the almost physical ([IGravityAgent](src/Core/Faces/IGravityAgent.cs)). With vvvv you can apply a custom Face to any Agent during runtime. The agent stays the same dynamic, it is just the mask, the Face, and the mindset that comes with the mask. As with any good agent, you can reapply a new Face anytime you want, it is done in a blitz.

To make a new custom Face, simply clone `TemplateFace (Game)` with <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>Enter</kbd> and edit the provided interface.
It must inherit IAgent to be found by the framework, but can inherit any number of other Faces, if you want to make use of ready-made modular functionality. 

As a bonus you can also extend the skillset of the Agent by writing [extension methods](http://www.codeproject.com/Articles/34209/Extension-Methods-in-C) in your part of *public static partial class AgentAPI*

Best Practise
============
1. Any complex behavior is only as good as the individual actions.
1. When cloning save all your plugins in *packs/vvvv-Game/Nodes/plugins* and all patches in *packs/vvvv-Game/Nodes/modules*
1. Test your actions thoroughly, and keep the test as a help patch to demonstrate its purpose in *packs/vvvv-Game/Nodes/modules*
1. If you choose to patch your actions, use only [stateless nodes](http://vvvv.org/documentation/node08.workshop.framebasedanimations) between `Pot` and `Lid`. write potential states on the Agents blackboard. No `Damper`! 
1. Make use of Faces. It will shorten development time in the long run, give you code completion and compiler errors.
1. If you make a bunch of plugins all relating to the same Face, start to make additions to *AgentAPI* whenever you duplicate code that relates to the Agent
1. When you make a nice and modular set of behaviors, don't be shy to share. 

Last Words about Graph Evaluation
=================================

While the multipurpose toolkit is a great way to enable rapid development of BTs, its node evaluation model does not allow for BTs directly. At this stage of development I tried to make the black box fit by making critical use of the Pin.Sync() method. Relevant spreads of Agents however get pushed into their children upwards, so there is no need for FrameDelay loops of values or even Agent instances within a tree.

It is a tree grown on the condition that a node's *Evaluate()* will not be called until the BehaviorLink pin is synced. This condition seems fulfilled, as long as plugins can be indefinitely prevented from being evaluated. 

1. all the plugins input pins should be on *AutoValidate = false* to prevent top-down evaluation. 
1. all their output pins should be on *AutoFlush = false* to maintain data integrity .
1. Also the first *Sync* into the node should be the BehaviorLink pin.

Unfortuately BTs are upside down now (mathematically it does not matter at all, but now it feels more like a scene graph of behaviors than the flow of Agents I would have preferred). 

>This situation is not ideal, because it can create strange effects in more convoluted scenarios. However, there are potential rewrites that keep any custom node compatible. For example the framework could be made to traverse the tree in a separate thread (building up some kind of secondary shadow graphs) and doublebuffering the pins, which means the Debug View will not provide proper timings for the individual nodes. Or devvvvs add a possiblility to microcontrol the Evaluate() call. Anyway, for now I consider this pack a solid 0.9

Libraries
=========

Additional external libraries are available via Nuget.org.

[Impromptu-Interface](https://github.com/ekonbenefits/impromptu-interface) under [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0)
> for [Duck Typing](http://ericlippert.com/2014/01/02/what-is-duck-typing/) and [Schönfinkeling](http://en.wikipedia.org/wiki/Currying)

[Json.NET](http://james.newtonking.com/json) under [MIT License](http://opensource.org/licenses/MIT)
> for serializing Agents. Useful for saving Agents to disc.

License
=======

![CC 4.0BY NC SAt](http://i.creativecommons.org/l/by-nc-sa/4.0/88x31.png)

Marko Ritter (www.intolight.de)

This software is distributed under the [CC Attribution-NonCommercial-ShareAlike 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/) license.

If you want to use it commercially or have any other reason why this license does not fit your need, write a quick email to <license@intolight.de> for a different license. Usually you will get away with either a flattr or a useful addition to the framework (Faces, API extensions, help patches, nodes, docu). This pack is for the community, really, and hand-crafted commercial work is a valued part of our community. 

Of course intolight does provide [official billing](http://www.intolight.de/impressum), too.

If your product contains parts of this framework and has an annual net revenue in excess of €100.000 (or any other currency, international exchange market rates apply) contact us immediately and please tell us more about the nature of your product so intolight can decide about licensing. 
