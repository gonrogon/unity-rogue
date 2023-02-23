using UnityEngine;
using Rogue.Game;
using Rogue.Game.Stock;
using Rogue.Core;

namespace Rogue.Gui.Widgets
{
    public class StockListView : ListView<StockListView.Note>
    {
        public struct Note
        {
            public ItemType type;

            public Ident eid;

            public string name;

            public int stock;

            public bool trading;
        }

        public void AddItem(ItemType type, Ident eid, string name, int stock, bool trading)
        {
            var node = CreateNode();

            node.data = new ()
            {
                type    = type,
                eid     = eid,
                name    = name,
                stock   = stock,
                trading = trading,
            };
        }

        public void SyncFromContext()
        {
            Sync(Rogue.Context.Stock);
        }

        public void Sync(StockSystem stock)
        {
            RemoveAll();
            /*
            foreach (var key in stock.Stock.Keys)
            {
                for (int i = 0; i < stock.Stock.Count(key); i++)
                {
                    var note = stock.Stock.Get(key, i);

                    if (note.eid.IsZero)
                    {
                        AddItem(note.type, Ident.Zero, Rogue.Context.ItemTypes.GenerateName(note.type), note.stock);
                    }
                    else
                    {
                        AddItem(note.type, note.eid, Query.GetName(note.eid).value, note.stock);
                    }
                }
            }
            */
            var broker = stock .Broker;
            var types  = broker.GetTypes();

            foreach (ItemType type in types)
            {
                BrokerRecordReader reader = broker.GetRecord(type);

                for (int i = 0; i < reader.Count; i++)
                {
                    StockNote note = reader.GetNote(i);
                    string    name;

                    if (note.stackable)
                    {
                        name = Rogue.Context.ItemTypes.GenerateName(type);
                    }
                    else
                    {
                        name = Query.GetName(note.eid).value;
                    }

                    AddItem(note.type, note.eid, name, note.stackable ? reader.Total : 1, note.trading);
                }
            }

            Draw();
        }

        public void Sell(int id)
        {
            Note note = GetNode(id).data;

            if (note.eid.IsZero)
            {
                Rogue.Context.Stock.SellAll(note.type);
            }
            else
            {
                Rogue.Context.Stock.Sell(note.type, note.eid);
            }
        }
    }
}
