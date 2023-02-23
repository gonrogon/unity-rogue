using Rogue.Core;

namespace Rogue.Game.Stock
{
    public class StockNote
    {
        /// <summary>
        /// Type of item.
        /// </summary>
        public ItemType type;

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public Ident eid;
        
        /// <summary>
        /// Flag indicating whether the item is a free item or it belongs to a stockpile.
        /// </summary>
        public bool free;

        /// <summary>
        /// Flag indicating whether the item is stackable or not.
        /// </summary>
        public bool stackable;

        /// <summary>
        /// Flag indicating whether the item is marked for trading or not.
        /// </summary>
        public bool trading;

        /// <summary>
        /// Flag indicating whether the item was removed or not.
        /// </summary>
        public bool removed;
    }
}
