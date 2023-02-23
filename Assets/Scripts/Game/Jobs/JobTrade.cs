using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    [System.Serializable]
    public class JobTrade : Job
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
        public Ident market;

        /// <summary>
        /// Where to store the item.
        /// </summary>
        public Vec2i marketLocation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location.</param>
        public JobTrade(Ident item, Vec2i itemLocation, Ident market, Vec2i marketLocation)
            :
            base(1, itemLocation)
        {
            this.item           = item;
            this.itemLocation   = itemLocation;
            this.market         = market;
            this.marketLocation = marketLocation;
        }
    }
}
