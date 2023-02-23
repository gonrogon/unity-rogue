using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Move : GameBehaviour<Move>
    {
        private Comp.Location mLocation;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mLocation = Entity.FindFirstComponent<Comp.Location>();

            if (mLocation == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.ActionMove msg: return OnMove(msg);
            }

            return GameMessageState.Continue;
        }

        public GameMessageState OnMove(Msg.ActionMove message)
        {
            Vec2i dest = mLocation.position + message.dir;
            dest.x = Mathf.Clamp(dest.x, 0, Context.Map.Cols);
            dest.y = Mathf.Clamp(dest.y, 0, Context.Map.Rows);

            if (Query.MapIsPassable(dest, Eid))
            {
                Context.Map.Move(mLocation.position, dest, Eid);
                mLocation.position = dest;

                message.done  = true;
                message.cost  = 100;
                message.state = Msg.ActionState.Good;
            }
            else
            {
                Debug.Log("Impassable");
            }

            return GameMessageState.Consumed;
        }
    }
}
