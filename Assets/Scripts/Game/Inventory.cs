using System.Diagnostics;
using Rogue.Core.Collections;
using Rogue.Core;

namespace Rogue.Game
{
    public class Inventory
    {
        private readonly IdentList m_items = new ();

        public int Count => m_items.Count;

        public Ident At(int index) => m_items[index];

        public bool Contains(Ident eid) => m_items.Contains(eid);

        public void Add(Ident eid) 
        {
            Debug.Assert(!Contains(eid), "Identifier is already in the inventory");

            m_items.Add(eid);
        }

        public void Drop(Ident eid)
        {
            int index = m_items.IndexOf(eid);
            if (index < 0)
            {
                return;
            }

            ArrayUtil.RemoveAndSwap(m_items, index);
        }

        public void Clear()
        {
            m_items.Clear();
        }
    }
}
