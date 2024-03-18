using System.Collections.Generic;
using Rogue.Core;
using Rogue.Core.Collections;
using Rogue.Coe;
using Rogue.Map;
using GG.Mathe;
using UnityEngine;

namespace Rogue.Game.Stock
{
    /// <summary>
    /// Defines a stock system.
    /// 
    /// The stock system is resposible for maintaining a list of the available items around the world. 
    /// </summary>
    public class StockSystem
    {
        /*
        public struct Note
        {
            public ItemType type;

            public Ident eid;

            public bool stackable;

            public bool sell;
        }
        */
        /*
        public struct StockNote
        {
            public ItemType type;

            public Ident eid;

            public int stock;

            public bool sell;
        }
        */
        /// <summary>
        /// List of free items.
        /// 
        /// Items that they are not inside container, stockpiles or inventories.
        /// </summary>
        //private readonly List<StockNote> m_freeItems = new();

        /// <summary>
        /// List of items in the stockpiles.
        /// </summary>
        //private readonly List<StockNote> m_stockpileItems = new();

        /// <summary>
        /// List with all the items in the map.
        /// </summary>
        private readonly List<StockNote> m_items = new();

        //private readonly Dictionary<string, int> m_stock = new();

        //private readonly MultiMap<ItemType, StockNote> m_stock = new();

        private readonly Broker m_broker = new();

        private readonly Dictionary<Ident, int> m_pendingJobs = new ();

        /// <summary>
        /// List of stockpiles.
        /// </summary>
        private readonly List<Stockpile> m_stockpiles = new();

        private readonly List<Ident> m_markets = new ();

        private readonly List<int> m_canBeSold = new ();

        /// <summary>
        /// Game map.
        /// </summary>
        private GameMap m_map;

        /// <summary>
        /// Game world.
        /// </summary>
        private GameWorld m_world;

        //public List<Note> StockpileItems => m_stockpileItems;

        public Broker Broker => m_broker;

        //public MultiMap<ItemType, StockNote> Stock => m_stock;

        

        //public int FreeItemsCount => m_freeItems.Count;

        public void Setup(GameWorld world, GameMap map)
        {
            m_world                = world;
            m_map                  = map;
            m_map.OnEntityAdded   += OnMapEntityAdded;
            m_map.OnEntityRemoved += OnMapEntityRemoved;

        }

        public void Quit()
        {
            m_map.OnEntityAdded   -= OnMapEntityAdded;
            m_map.OnEntityRemoved -= OnMapEntityRemoved;
        }

        public void OnMarketCreated(Ident eid)
        {
            m_markets.Add(eid);
        }

        public void OnMarketRemoved(Ident eid)
        {
            m_markets.Remove(eid);
        }

        public void OnMapEntityAdded(GameMap map, Ident eid, Vec2i where)
        {
            GameEntity entity = m_world.Find(eid);
            if (entity == null)
            {
                return;
            }

            /*
            var tag =  entity.FindFirstComponent<Comp.Tag>();
            if (tag == null)
            {
                return;
            }

            if (tag.ContainsOne(TagType.Item) == false)
            {
                return;
            }
            */

            var type = entity.FindFirstAny<Comp.ItemDecl>();
            if (type == null)
            {
                return;
            }

            if (map.IsZone(where))
            {
                //AddItem(m_stockpileItems, true, entity);
                AddItem(null, true, entity);
            }
            else
            {
                AddItem(null, false, entity);
                //AddItem(m_freeItems, false, entity);
            }
        }



        private void AddItem(List<StockNote> list, bool stock, GameEntity entity)
        {
            var stack = entity.FindFirstAny<Comp.Stack>();
            var decl  = entity.FindFirstAny<Comp.ItemDecl>();

            StockNote note = new ()
            {
                type      = decl.type,
                eid       = entity.Id,
                free      = !stock,
                stackable = stack != null,
                trading   = false,
                removed   = false,
            };
            
            if (stock)
            {
                m_broker.Add(note);
                /*
                if (note.stackable)
                {
                    if (m_stock.Count(decl.type) <= 0)
                    {
                        m_stock.Add(decl.type, new () { type = decl.type, stock = stack.amount });
                    }
                    else
                    {
                        var allnote = m_stock.Get(decl.type, 0);
                        allnote.stock += stack.amount;

                        m_stock.Replace(decl.type, 0, allnote);
                    }
                }
                else
                {
                    m_stock.Add(decl.type, new () { type = decl.type, stock = 1, eid = entity.Id});
                }
                */
            }

            m_items.Add(note);

            //list.Add(note);
        }

        public void OnMapEntityRemoved(GameMap map, Ident eid, Vec2i where)
        {
            /*
            if (map.IsZone(where))
            {
                m_stockpileItems.RemoveAll(note => note.eid == eid);
            }
            else
            {
                m_freeItems.RemoveAll(note => note.eid == eid);
            }
            */
            // NOTE: This is slow as hell with thousand of items. This should use remove and swap.
            

            int index = m_items.FindIndex(note => note.eid == eid);
            if (index < 0)
            {
                return;
            }

            StockNote note = m_items[index];

            if (!note.free)
            {
                m_broker.Remove(m_items[index]);
            }
            m_items.RemoveAt(index);

            //m_items.RemoveAll(note => note.eid == eid);
        }

        public void Sell(ItemType type, Ident eid)
        {
            if (!eid.IsZero)
            {
                SellOne(type, eid);
            }
            else
            {
                SellAll(type);
            }
        }

        public void SellOne(ItemType type, Ident eid)
        {
            /*
            for (int i = 0; i < m_stock.Count(type); i++)
            {
                var note = m_stock.Get(type, i);
                if (note.eid == eid)
                {
                    note.sell = true;
                    m_stock.Replace(type, i, note);
                    return;
                }
            }
            */
            /*
            int index = m_stockpileItems.FindIndex(note => note.eid == eid);
            if (index >= 0)
            {
                StockNote note = m_stockpileItems[index];
                note.trading = true;

                m_stockpileItems[index] = note;
            }
            */

            int index = m_items.FindIndex(note => note.eid == eid);
            if (index >= 0)
            {
                var note = m_items[index];
                note.trading = true;

                m_broker.Sell(note);
            }
        }

        public void SellAll(ItemType type)
        {
            foreach (var note in m_items)
            {
                if (note.type == type)
                {
                    note.trading = true;
                    m_broker.Sell(note);
                }
            }
            // Stackable only have one item.
            /*
            var note = m_stock.Get(type, 0);
            note.sell = true;
            m_stock.Replace(type, 0, note);

            for (int i = 0; i < m_stockpileItems.Count; i++)
            {
                if (m_stockpileItems[i].type == type)
                {
                    var snote  = m_stockpileItems[i];
                    snote.sell = true;

                    m_stockpileItems[i] = snote;
                }
            }
            */
        }
        /*
        public Ident GetFreeItem(int i)
        {
            return m_freeItems[i].eid;
        }
        */
        public Stockpile At(int id)
        {
            return m_stockpiles[id];
        }

        public int CreateStockpile(Rect2i bounds)
        {
            Stockpile stockpile = new (m_stockpiles.Count, m_map.CreateZone(), bounds);

            m_map.SetZone(bounds, stockpile.Zone);
            m_stockpiles.Add(stockpile);

            return m_stockpiles.Count - 1;
        }

        public void Update()
        {
            /*
            for (int i = 0; i < m_freeItems.Count; i++)
            {
                Ident eid = m_freeItems[i].eid;

                if (m_pendingJobs.ContainsKey(eid))
                {
                    continue;
                }

                foreach (Stockpile stockpile in m_stockpiles)
                {
                    if (stockpile.Accept(eid))
                    {
                        if (stockpile.TryReserve(out Vec2i where))
                        {
                            Jobs.JobStockpile job = new(eid, Query.GetPosition(eid).value, stockpile.Id, where);
                            job.onCompleted = OnJobCompleted;
                            Context.Jobs.AddJob(job);
                            m_pendingJobs[eid] = job;

                            break;
                        }
                    }
                }
            }
            */
            for (int i = 0; i < m_items.Count; i++)
            {
                StockNote note = m_items[i];
                // Check if there is already a job for the item.
                if (m_pendingJobs.ContainsKey(note.eid))
                {
                    continue;
                }

                if (note.free)
                {
                    ProcessFreeItem(m_items[i]);
                }
                else
                {
                    if (note.trading)
                    {
                        ProcessTradingItem(note);
                    }
                }
            }

            int total = 0;

            foreach (Ident marketId in m_markets)
            {
                total += m_broker.SellMarketContent(marketId);
            }

            if (total > 0)
            {
                Debug.Log("ITEMS SOLD FOR: " + total);
            }
        }

        public void ProcessFreeItem(StockNote note)
        {
            foreach (Stockpile stockpile in m_stockpiles)
            {
                if (stockpile.Accept(note.eid) && stockpile.TryReserve(out Vec2i where))
                {
                    var job = new Jobs.JobStockpile(note.eid, Query.GetPosition(note.eid).value, stockpile.Id, where)
                    {
                        onCompleted = OnJobCompleted
                    };

                    m_pendingJobs[note.eid] = Context.Jobs.AddJob(job);
                    break;
                }
            }
        }

        public void ProcessTradingItem(StockNote note)
        {
            // Check if there is at least one market.
            if (m_markets.Count <= 0)
            {
                return;
            }

            var building = Context.World.Find(m_markets[0]).FindFirstComponent<Comp.Building>();

            var job = new Jobs.JobTrade(
                note.eid,     Query.GetPosition(note.eid).value,
                m_markets[0], building.zone.Min
            );

            m_pendingJobs[note.eid] = Context.Jobs.AddJob(job);
        }

        #region @@@ JOBS @@@

        void OnJobCompleted(Jobs.Job job)
        {
            switch (job)
            {
                case Jobs.JobStockpile s: OnJobStockpileCompleted(s); break;
            }
        }

        void OnJobStockpileCompleted(Jobs.JobStockpile job)
        {
            m_stockpiles[job.stockpile].ReleaseReserve(job.stockpileLocation);
            m_pendingJobs.Remove(job.item);
        }

        #endregion
    }
}
