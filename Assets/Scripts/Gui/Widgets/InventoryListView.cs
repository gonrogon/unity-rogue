using UnityEngine;
using Rogue.Game;
using Rogue.Game.Stock;
using Rogue.Core;

namespace Rogue.Gui.Widgets
{
    public class InventoryListView : ListView<InventoryListView.Note>
    {
        public struct Note
        {
            public string name;

            public int stock;
        }

        public void AddItem(string name, int stock)
        {
            var node = CreateNode();

            node.data = new ()
            {
                name  = name,
                stock = stock
            };
        }

        public void Sync(Ident eid)
        {
            RemoveAll();

            var entity    = Rogue.Context.World.Find(eid);
            var cinv      = entity.FindFirstComponent<Game.Comp.Inventory>();

            if (cinv == null)
            {
                return;
            }

            var iid       = cinv.iid;
            var inventory = Rogue.Context.Inventories.Get(iid);

            for (int i = 0; i < inventory.Count; i++)
            {
                Ident  itemId = inventory.At(i);
                string name   = Query.GetName(itemId).value;
                int    stock  = 1;

                AddItem(name, stock);
            }

            /*
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
            */
            Draw();
        }
    }
}
