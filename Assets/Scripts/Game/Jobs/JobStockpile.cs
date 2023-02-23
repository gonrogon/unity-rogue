using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    [System.Serializable]
    public class JobStockpile : Job
    {
        /// <summary>
        /// Item to store.
        /// </summary>
        public Ident item;

        /// <summary>
        /// Where is the item to store.
        /// </summary>
        public Vec2i itemLocation;

        /// <summary>
        /// Stockpile.
        /// </summary>
        public int stockpile;

        /// <summary>
        /// Where to store the item.
        /// </summary>
        public Vec2i stockpileLocation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location.</param>
        public JobStockpile(Ident item, Vec2i itemLocation, int stockpile, Vec2i stockpileLocation)
            :
            base(1, itemLocation)
        {
            this.item              = item;
            this.itemLocation      = itemLocation;
            this.stockpile         = stockpile;
            this.stockpileLocation = stockpileLocation;
        }
    }
}
