using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Name : GameBehaviour<Name>
    {
        private Comp.Name mName;

        private Comp.ItemDecl mDecl;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mName = Entity.FindFirstComponent<Comp.Name>();
            mDecl = Entity.FindFirstAny<Comp.ItemDecl>();

            if (mName == null && mDecl == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Name msg:
                {
                    if (mName != null)
                    {
                        msg.name        += mName.name;
                        msg.description  = mName.description;
                    }
                    else
                    {
                        if (mDecl != null)
                        {
                            msg.name        = Context.ItemTypes.GetName(mDecl.type);
                            msg.description = null;
                        }
                    }
                }
                break;
            }

            return GameMessageState.Continue;
        }
    }
}
