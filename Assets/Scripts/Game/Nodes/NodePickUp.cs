using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodePickUp : NodeAgentAction
    {
        private Ident m_entity;

        //private Vec2i m_targetLocation;

        //private Ident m_targetId;

        public NodePickUp(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            if (!TryFindVar("targetLocation", out Vec2i targetLocation)) { return NodeState.Failure; }
            if (!TryFindVar("targetId",       out Ident targetId))       { return NodeState.Failure; }

            var msg = Context.World.Send(m_entity, new Msg.PickUp(targetId));

            return NodeState.Success;
        }
    }
}
