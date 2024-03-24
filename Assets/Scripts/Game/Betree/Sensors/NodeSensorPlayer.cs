using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;
using System.Collections.Generic;

namespace Rogue.Game.Betree
{
    public class NodeSensorPlayer : NodeActionBase
    {
        public override NodeState OnUpdate()
        {
            return NodeState.Failure;

            if (!Blackboard.TryGet("sensorArea", out object obj))
            {
                return NodeState.Failure;
            }

            Ident eid;

            switch (obj)
            {
                case Vec2i v:              if (Query.MapGetFirstPlayer(v, out eid)) { Blackboard.Set("sensorTarget", eid); return NodeState.Success; } break;
                case IEnumerable<Vec2i> l: if (Query.MapGetFirstPlayer(l, out eid)) { Blackboard.Set("sensorTarget", eid); return NodeState.Success; } break;
            }

            return NodeState.Failure;
        }
    }
}
