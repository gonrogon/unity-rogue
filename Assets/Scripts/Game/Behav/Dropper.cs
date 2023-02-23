using Rogue.Coe;
using UnityEngine;

namespace Rogue.Game.Behav
{
    public class Dropper : GameBehaviour<Picker>
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
            var cInv = Entity.FindFirstComponent<Comp.Inventory>();

            if (cInv is null || cInv.iid.IsZero)
            {
                Debug.Log("I don't have a place from where to drop the item");

                return GameMessageState.Continue;
            }

            Debug.Log("I drop a " + Context.World.Send(message.what, new Msg.Name()).name);

            Context.Inventories.Get(cInv.iid).Drop(message.what);
            // Forward the message to the picked item.
            Context.World.Send(message.what, message);

            return GameMessageState.Consumed;
        }
    }
}
