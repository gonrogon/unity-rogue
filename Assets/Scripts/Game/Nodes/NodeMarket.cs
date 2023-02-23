using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeMarket : NodeAgentAction
    {
        private Ident m_entity;

        public NodeMarket(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            if (!TryFindVar("targetLocation", out Vec2i targetLocation)) { return NodeState.Failure; }
            if (!TryFindVar("targetId",       out Ident targetId))       { return NodeState.Failure; }
            if (!TryFindVar("marketId",       out Ident marketId))       { return NodeState.Failure; }

            var msg = Context.World.Send(m_entity, new Msg.ActionStore(targetId, marketId));

            return NodeState.Success;
        }
    }
}
