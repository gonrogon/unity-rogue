using Rogue.Core;
using Rogue.Coe;
using Rogue.Map;
using GG.Mathe;
using System;

namespace Rogue.Game
{
    public static partial class Query
    {
        public static bool IsInInventory(Ident entity, Ident what)
        {
            GameEntity ent = Context.World.Find(entity); 
            
            var invt  = ent.FindFirstComponent<Comp.Inventory>();
            if (invt != null)
            {
                return Context.Inventories.Get(invt.iid).Contains(what);
            }

            var body  = ent.FindFirstComponent<Comp.Body>();
            if (body != null)
            {
                return Context.Bodies.Get(body.bid).IsHeld(what);
            }

            return false;
        }
    }
}
