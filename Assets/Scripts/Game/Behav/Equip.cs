using UnityEngine;
using Rogue.Core;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Equip : GameBehaviour<Equip>
    {
        private Comp.Location m_location;

        private Comp.Body m_body;

        private Comp.Inventory m_inventory;

        private Body MyBody => Context.Bodies.Get(m_body.bid);

        private Inventory MyInventory => Context.Inventories.Get(m_inventory.iid);

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            m_location  = Entity.FindFirstComponent<Comp.Location>();
            m_body      = Entity.FindFirstComponent<Comp.Body>();
            m_inventory = Entity.FindFirstComponent<Comp.Inventory>();

            if (m_location == null || m_body == null || m_inventory == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            bool done = false;

            switch (message)
            {
                case Msg.Drop drop:         { done = drop .done = Drop(drop.what, drop.where); } break;
                case Msg.PickUp pick:       { done = pick .done = PickUp(pick.what); } break;
                case Msg.ActionStore store: { done = store.done = Store(store); } break;
            }

            return done ? GameMessageState.Consumed : GameMessageState.Continue;
        }

        private bool Store(Msg.ActionStore msg)
        {
            bool found = false;

            if (MyBody.IsHeld(msg.what))        { found = true; MyBody.Drop(msg.what); }
            if (MyInventory.Contains(msg.what)) { found = true; MyInventory.Drop(msg.what); }

            if (!found)
            {
                return false;
            }

            if (!Context.World.Send(msg.where, msg).done)
            {
                return false;
            }

            return true;
        }

        private bool PickUp(Ident eid)
        {
            if (!Context.World.Send(eid, new Msg.PickUp(eid)).done)
            {
                return false;
            }

            MyInventory.Add(eid);

            return true;
        }

        private bool Drop(Ident eid) => Drop(eid, m_location.position);

        private bool Drop(Ident eid, Vec2i where)
        {
            bool found = false;

            if (MyBody.IsHeld(eid))        { found = true; MyBody.Drop(eid); }
            if (MyInventory.Contains(eid)) { found = true; MyInventory.Drop(eid); }

            if (!found)
            {
                return false;
            }

            if (!Context.World.Send(eid, new Msg.Drop(eid, where)).done)
            {
                return false;
            }

            return true;
        }

        private bool Hold(Ident eid)
        {
            if (!MyBody.FindHolding(eid))
            {
                return false;
            }

            if (!Context.World.Send(eid, new Msg.PickUp(eid)).done)
            {
                return false;
            }

            MyBody.Hold(eid);

            return true;
        }

        private bool HoldFromInventory(Ident eid)
        {
            if (!MyInventory.Contains(eid))
            {
                return false;
            }

            if (!MyBody.FindHolding(eid))
            {
                return false;
            }

            MyInventory.Drop(eid);
            MyBody.Hold(eid);

            return true;
        }
    }
}
