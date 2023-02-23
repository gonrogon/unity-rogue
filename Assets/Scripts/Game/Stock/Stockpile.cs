using System.Collections.Generic;
using UnityEngine;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Stock
{
    public class Stockpile
    {
        private readonly int m_id = -1;

        /// <summary>
        /// Zone identifier.
        /// </summary>
        private readonly int m_zone = -1;

        /// <summary>
        /// Dictionary with the reserved positions in the stockpile.
        /// </summary>
        private readonly Dictionary<Vec2i, bool> m_reserved = new();

        /// <summary>
        /// Bounds.
        /// </summary>
        private Rect2i m_bounds = Rect2i.Zero;

        private StockpileFilter m_filter = new ();

        public int Id => m_id;

        public int Zone => m_zone;

        public Rect2i Bounds => m_bounds;

        public Stockpile(int id, int zone)
        {
            m_id   = id;
            m_zone = zone;
        }

        public Stockpile(int id, int zone, Rect2i bounds)
        {
            m_id     = id;
            m_zone   = zone;
            m_bounds = bounds;
        }

        # region @@@ ZONE MANAGEMENT @@@

        public void SetBounds(Vec2i coord)
        {
            m_bounds.SetFromPointsEncompassed(coord, coord);
        }

        public void SetBounds(Rect2i bounds)
        {
            m_bounds = bounds;
        }

        public void Extend(Vec2i coord)
        {
            m_bounds.Extend(coord);
        }

        public void Extend(Rect2i rect)
        {
            m_bounds.Extend(rect);
        }

        public void Shrink(Map.GameMap map)
        {
            Vec2i min = Vec2i.MaxValue;
            Vec2i max = Vec2i.MinValue;

            foreach (Vec2i coord in m_bounds)
            {
                if (map.IsZone(coord, m_zone))
                {
                    min = Vec2i.Min(min, coord);
                    max = Vec2i.Max(max, coord);
                }
            }

            m_bounds.SetFromPointsEncompassed(min, max);
        }

        #endregion

        #region @@@ ITEMS MANAGEMENT @@@

        public void SetFilter(StockpileFilter filter)
        {
            m_filter = filter;
        }

        /// <summary>
        /// Checks if a item is accepted by the stockpile.
        /// </summary>
        /// <param name="eid">Item identifier.</param>
        /// <returns>True if the item is accepted; otherwise, false.</returns>
        public bool Accept(Ident eid)
        {
            if (m_filter != null)
            {
                return m_filter.Accept(eid);    
            }
            
            return true;
        }

        /// <summary>
        /// Notifies the stockpile that an item has been added.
        /// </summary>
        /// <param name="eid">Item identifier.</param>
        /// <param name="where">Location.</param>
        public void OnItemAdded(Ident eid, Vec2i where)
        {
            ReleaseReserve(where);
        }

        /// <summary>
        /// Notifies the stockpile that an item has been removed.
        /// </summary>
        /// <param name="eid"></param>
        public void OnItemRemoved(Ident eid)
        {}

        #endregion

        #region @@@ RESERVATION @@@

        /// <summary>
        /// Tries to reserve a location.
        /// </summary>
        /// <param name="where">Reserved location.</param>
        /// <returns>True if a location was reserved; otherwise, false.</returns>
        public bool TryReserve(out Vec2i where)
        {
            where = Vec2i.Zero;

            foreach (Vec2i coord in m_bounds)
            {
                if (!Context.Map.IsZone(coord, m_zone))
                {
                    continue;
                }

                if (Context.Map.IsSolid(coord) || !Context.Map.Empty(coord))
                { 
                    continue;
                }

                if (m_reserved.TryGetValue(coord, out bool value) && value)
                {
                    continue;
                }

                where = coord;
                m_reserved[coord] = true;
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Release a reserved location.
        /// </summary>
        /// <param name="where">Location to release.</param>
        public void ReleaseReserve(Vec2i where)
        {
            m_reserved.Remove(where);
        }

        #endregion
    }
}
