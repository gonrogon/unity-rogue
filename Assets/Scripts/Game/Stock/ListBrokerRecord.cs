using System.Collections.Generic;

namespace Rogue.Game.Stock
{
    internal class ListBrokerRecord : BaseBrokerRecord
    {
        private readonly List<StockNote> m_list = new();

        public ListBrokerRecord(ItemType type)
        {
            Type      = type;
            Stackable = true;
        }

        public override int GetNoteCount()
        {
            return m_list.Count;
        }

        public override StockNote GetNote(int i) 
        {
            return m_list[i];
        }

        public override void Add(StockNote note)
        {
            base  .Add(note);
            m_list.Add(note);
        }

        public override void Remove(StockNote note)
        {
            base  .Remove(note);
            m_list.Remove(note);
        }

        public override void Sync()
        {
            base.Sync();

            for (int i = 0; i < m_list.Count;)
            {
                if (m_list[i].removed)
                {
                    Core.ArrayUtil.RemoveAndSwap(m_list, i);
                    continue;
                }

                i++;
            }
        }
    }
}
