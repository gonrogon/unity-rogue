using Rogue.Core;
using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Droppable : GameBehaviour<Droppable>
    {
        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Drop msg: return OnDrop(msg);
            }

            return GameMessageState.Continue;
        }

        public GameMessageState OnDrop(Msg.Drop message)
        {
            var loc = Entity.FindFirstComponent<Comp.Location>();
            if (loc is null)
            {
                return GameMessageState.Continue;
            }

            if (!Query.MapIsDropAllow(message.where, Eid))
            {
                return GameMessageState.Consumed;
            }

            loc.position = message.where;
            // Insert the object in the map.
            Context.Map.Add(message.where, Eid);
            message.done = true;

            return GameMessageState.Consumed;
        }
        /*
        public GameMessageState OnDrop2(Msg.Drop message)
        {
            var location = Entity.FindFirstComponent<Comp.Location>();
            var item     = Entity.FindFirstComponent<Comp.Item>();

            if (location == null)
            {
                return GameMessageState.Continue;
            }

            bool  stackFound = false;
            Ident stackable  = Ident.Zero;

            if (item != null && item.stackable)
            {
                stackFound = Context.Map.TryFindFirst(message.where, eid => 
                {
                    var otherItem = World.Find(eid).FindFirstComponent<Comp.Item>();
                    if (otherItem == null)
                    {
                        return false;
                    }

                    return otherItem.itemId == item.itemId;
                },
                out stackable);
            }

            if (stackFound)
            {
                // TODO: Check stacksize.
                World.Find(stackable).FindFirstComponent<Comp.Item>().amount += item.amount;
                Entity.Kill();
            }
            else
            {
                if (!Query.MapIsDropAllow(message.where, Eid))
                {
                    return GameMessageState.Continue;
                }

                location.position = message.where;
                // Insert the object in the map.
                Context.Map.Add(message.where, Eid);
            }

            return GameMessageState.Consumed;
        }
        */
    }
}
