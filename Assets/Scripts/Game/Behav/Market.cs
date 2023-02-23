using UnityEngine;
using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Market : GameBehaviour<Market>
    {
        private Comp.Inventory m_inventory;

        private Inventory MyInventory => Context.Inventories.Get(m_inventory.iid);

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            m_inventory = Entity.FindFirstComponent<Comp.Inventory>();

            if (m_inventory == null)
            {
                return false;
            }

            return true;
        }

        public override void OnInit()
        {
            Context.Stock.OnMarketCreated(Entity.Id);
        }

        public override void OnQuit()
        {
            Context.Stock.OnMarketRemoved(Entity.Id);
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            bool done = false;

            switch (message)
            {
                case Msg.ActionStore store: done = Store(store); break;
            }

            return done ? GameMessageState.Consumed : GameMessageState.Continue;
        }

        private bool Store(Msg.ActionStore msg)
        {
            MyInventory.Add(msg.what);
            msg.done = true;

            return true;
        }
    }
}
