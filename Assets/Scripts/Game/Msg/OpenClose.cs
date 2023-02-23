using Rogue.Coe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity to open or close.
    /// </summary>
    public class OpenClose : GameMessage<OpenClose>
    {
        /// <summary>
        /// Type of switch to perform.
        /// </summary>
        public SwitchType type = SwitchType.Toggle;

        /// <summary>
        /// Flag indicinating whether the entity is closed or not.
        /// </summary>
        public bool closed = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OpenClose() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">Type of switch to perform.</param>
        public OpenClose(SwitchType type)
        {
            this.type = type;
        }
    }
}