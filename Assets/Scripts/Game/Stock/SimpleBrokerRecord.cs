using System;

namespace Rogue.Game.Stock
{
    internal class SimpleBrokerRecord : BaseBrokerRecord
    {
        private StockNote m_note;

        public SimpleBrokerRecord(ItemType type)
        {
            Type      = type;
            Stackable = true;
            m_note    = new StockNote() { type = type, eid = Core.Ident.Zero, stackable = true };
        }

        public override int GetNoteCount()
        {
            return 1;
        }

        public override StockNote GetNote(int index)
        {
            if (m_note == null)
            {
                throw new IndexOutOfRangeException();
            }

            if (m_note != null && index > 0)
            {
                throw new IndexOutOfRangeException();
            }

            return m_note;
        }
    }
}
