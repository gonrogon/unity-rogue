using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Door : GameBehaviour<Door>
    {
        private Comp.Lock mLock;

        private Comp.Block mBlock;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mLock  = Entity.FindFirstComponent<Comp.Lock>();
            mBlock = Entity.FindFirstComponent<Comp.Block>();

            if (mLock == null || mBlock == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.OpenClose msg: return OnOpenClose(msg);
            }

            return GameMessageState.Continue;
        }

        public GameMessageState OnOpenClose(Msg.OpenClose message)
        {
            switch (message.type)
            {
            
                case SwitchType.Open:   { mLock.Open(); }   break;
                case SwitchType.Close:  { mLock.Close(); }  break;
                case SwitchType.Toggle: { mLock.Toggle(); } break;
            }

            message.closed = mLock.closed;
            mBlock.enabled = mLock.closed;
            
            return GameMessageState.Consumed;
        }
    }
}
