using Rogue.Coe;
using UnityEngine;

namespace Rogue.Game.Behav
{
    public class Picker : GameBehaviour<Picker>
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
            var cInv = Entity.FindFirstComponent<Comp.Inventory>();

            if (cInv is null || cInv.iid.IsZero)
            {
                Debug.Log("I don't have a place where to store the item");

                return GameMessageState.Continue;
            }

            Debug.Log("I picked a " + Context.World.Send(message.what, new Msg.Name()).name);

            Context.Inventories.Get(cInv.iid).Add(message.what);
            // Forward the message to the picked item.
            Context.World.Send(message.what, message);

            return GameMessageState.Consumed;
        }
    }
}
