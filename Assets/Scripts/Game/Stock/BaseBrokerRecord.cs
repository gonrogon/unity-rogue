namespace Rogue.Game.Stock
{
    internal abstract class BaseBrokerRecord : IBrokerRecord
    {
        public ItemType Type { get; protected set; } = ItemType.None;

        public bool Stackable { get; protected set; } = false;

        public int Total { get; protected set; } = 0;

        public int Trading { get; protected set; } = 0;

        public int Count => GetNoteCount();

        public abstract int GetNoteCount();

        public abstract StockNote GetNote(int index);

        public virtual void Add(StockNote note) => Total++;

        public virtual void Sell(StockNote note) => Trading++;

        public virtual void SellAll() => Trading = Total;

        public virtual void Remove(StockNote note)
        {
            Total--;

            if (note.trading)
            {
                Trading--;
            }
        }

        public virtual void Sync() {}
    }
}
