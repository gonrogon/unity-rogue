using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Attack : GameBehaviour<Attack>
    {
        private Comp.Location mLocation;

        public Attack() : base(1000) {}

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
            Vec2i target = mLocation.position + message.dir;
            target.x = Mathf.Clamp(target.x, 0, Context.Map.Cols);
            target.y = Mathf.Clamp(target.y, 0, Context.Map.Rows);
            
            var cBody = Entity.FindFirstComponent<Comp.Body>();
            if (cBody is null || cBody.bid.IsZero)
            {
                return GameMessageState.Continue;
            }
            // Check if the entity has a weapon.
            if (!Query.BodyGetWeaponsInHands(cBody.bid, out Core.Ident lid, out Core.Ident rid))
            {
                return GameMessageState.Continue;
            }
            // Check if there is an enemy.
            if (!Query.MapGetFirstEnemy(target, Eid, out Core.Ident enemy))
            {
                return GameMessageState.Continue;
            }

            if (!lid.IsZero)
            {
                World.Send(lid, new Msg.Hit(lid, enemy));

                return GameMessageState.Consumed;
            }

            if (!rid.IsZero)
            {
                World.Send(rid, new Msg.Hit(rid, enemy));

                return GameMessageState.Consumed;
            }

            /*
            if (Query.MapIsPassable(dest, Eid))
            {
                Debug.Log($"I move by ({message.dir.x}, {message.dir.y}) to ({dest.x}, {dest.y})");

                Context.Map.Move(mLocation.position, dest, Eid);
                mLocation.position = dest;

                message.cost  = 100;
                message.state = Msg.ActionState.Good;
            }
            else
            {
                Debug.Log("Impassable");
            }
            */
            return GameMessageState.Consumed;
        }
    }
}
