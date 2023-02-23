using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Pickable : GameBehaviour<Pickable>
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
                case Msg.PickUp msg: return OnPickUp(msg);
            }

            return GameMessageState.Continue;
        }

        public GameMessageState OnPickUp(Msg.PickUp message)
        {
            var loc = Entity.FindFirstComponent<Comp.Location>();
            if (loc is null)
            {
                return GameMessageState.Continue;
            }
            // Remove the object from the map.
            Context.Map.Remove(loc.position, Eid);
            message.done = true;
            return GameMessageState.Consumed;
        }
    }
}
