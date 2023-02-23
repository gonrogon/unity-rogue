using Rogue.Coe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to get the name of an entity.
    /// </summary>
    public class Name : GameMessage<Name>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// Description.
        /// </summary>
        public string description = string.Empty;
    }
}