using Rogue.Core.Collections;
using Rogue.Core;

namespace Rogue.Game
{
    public class InventorySystem
    {
        private readonly IdentMap<Inventory> m_inventories = new ();

        public Inventory Get(Ident iid)
        {
            return m_inventories.Get(iid);
        }

        public Ident Add()
        {
            return m_inventories.Add(new Inventory());
        }

        public Ident Add(Inventory inventory)
        {
            return m_inventories.Add(inventory);
        }

        public void Remove(Ident iid)
        {
            m_inventories.Release(iid);
        }
    }
}
