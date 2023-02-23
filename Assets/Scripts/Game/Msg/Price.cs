using Rogue.Coe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to get the name of an entity.
    /// </summary>
    public class Price : GameMessage<Price>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public int price = 0;
    }
}