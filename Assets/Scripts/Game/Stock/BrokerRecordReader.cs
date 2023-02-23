using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Game.Stock
{
    public struct BrokerRecordReader
    {
        private readonly IBrokerRecord m_record;

        public ItemType Type => m_record.Type;

        public bool Stackable => m_record.Stackable;

        public int Total => m_record.Total;

        public int Trading => m_record.Trading;

        public int Count => m_record.Count;

        internal BrokerRecordReader(IBrokerRecord record)
        {
            m_record = record;
        }

        public StockNote GetNote(int index) => m_record.GetNote(index);
    }
}
