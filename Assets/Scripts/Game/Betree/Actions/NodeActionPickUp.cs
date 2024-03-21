﻿using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeActionPickUp : NodeActionBase
    {
        private readonly string m_varWhat = "what";

        public NodeActionPickUp() {}

        public NodeActionPickUp(string what)
        {
            m_varWhat = what;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_varWhat, out Ident what)) { return NodeState.Failure;}

            var msg = Context.World.Send(AgentState.eid, new Msg.PickUp(what));
            if (msg.done)
            {
                return NodeState.Success;
            }

            #if DEBUG && UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"Unable to pick up item {m_varWhat}");
            #endif

            return NodeState.Failure;
        }
    }
}
