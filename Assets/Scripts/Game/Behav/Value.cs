using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Value : GameBehaviour<Value>
    {
        private Comp.ItemDecl m_decl;

        private Comp.Item m_item;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            m_decl = Entity.FindFirstAny<Comp.ItemDecl>();
            m_item = Entity.FindFirstAny<Comp.Item>();

            if (m_decl == null || m_item == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Price msg:
                {
                    msg.price = m_decl.price;
                }
                break;
            }

            return GameMessageState.Continue;
        }
    }
}
