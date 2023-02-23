using System;
using System.Collections.Generic;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Stock
{
    public class StockpileFactory
    {
        public static int CreateGenericStockpile(Rect2i bounds)
        {
            return Context.Stock.CreateStockpile(bounds);
        }

        public static int CreateWeaponStockpile(Rect2i bounds)
        {
            int id = CreateGenericStockpile(bounds);
            var stockpile = Context.Stock.At(id);

            StockpileFilter filter = new ();
            filter.Allow("weapon");

            stockpile.SetFilter(filter);

            return id;
        }
    }
}
