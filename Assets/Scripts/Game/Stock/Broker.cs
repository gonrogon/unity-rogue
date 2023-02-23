using System.Collections.Generic;
using Rogue.Core;

namespace Rogue.Game.Stock
{
    public class Broker
    {
        private readonly Dictionary<ItemType, IBrokerRecord> m_records = new();

        public ItemType[] GetTypes()
        {
            var list = new ItemType[m_records.Count];
            int tidx = 0;

            foreach (var key in m_records.Keys)
            {
                list[tidx++] = key;    
            }

            return list;
        }

        public BrokerRecordReader GetRecord(ItemType type)
        {
            return new(FindRecord(type));
        }

        public void Add(StockNote note)
        {
            if (note.stackable)
            {
                AddStackable(note);
            }
            else
            {
                AddUnique(note);
            }
        }

        private void AddStackable(StockNote note)
        {
            IBrokerRecord record = FindRecord(note.type);
            if (record == null)
            {
                record = m_records[note.type] = new SimpleBrokerRecord(note.type);
            }

            record.Add(note);
        }

        private void AddUnique(StockNote note)
        {
            IBrokerRecord record = FindRecord(note.type);
            if (record == null)
            {
                record = m_records[note.type] = new ListBrokerRecord(note.type);
            }

            record.Add(note);
        }

        public void Remove(StockNote note)
        {
            m_records[note.type].Remove(note);
        }

        public void Sell(StockNote note)
        {
            m_records[note.type].Sell(note);
        }

        public void SellAll(ItemType type)
        {
            m_records[type].SellAll();
        }

        public int SellMarketContent(Ident marketId)
        {
            int       total     = 0;
            Ident     iid       = Context.World.Find(marketId).FindFirstComponent<Comp.Inventory>().iid;
            Inventory inventory = Context.Inventories.Get(iid);

            for (int i = 0; i < inventory.Count; i++)
            {
                int basePrice = Context.World.Send(inventory.At(i), new Msg.Price()).price;
                if (basePrice > 0)
                {
                    total += basePrice;
                }
                // Destroy the item.
                Context.World.Find(inventory.At(i)).Kill();
            }
            inventory.Clear();

            return total;
        }

        #region @@@ UTILITIES @@@

        private IBrokerRecord FindRecord(ItemType type)
        {
            if (!m_records.TryGetValue(type, out var record))
            {
                return null;
            }

            return record;
        }

        #endregion
    }
}
