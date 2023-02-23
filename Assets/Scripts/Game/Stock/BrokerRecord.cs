using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Game.Stock
{
    internal interface IBrokerRecord
    {
        ItemType Type { get; }

        bool Stackable { get; }

        int Total { get; }

        int Trading { get; }

        int Count { get; }

        StockNote GetNote(int index);

        void Add(StockNote note);

        void Sell(StockNote note);

        void SellAll();

        void Remove(StockNote note);

        void Sync();
    }
}
