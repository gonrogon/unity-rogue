namespace Rogue.Coe
{
    public enum GameMessageState
    {
        Continue, // Continue with the chain.
        Consumed  // Message has been consumed, stop the chain.
    }

    public interface IGameMessage
    {
        /// <summary>
        /// Create a deep copy of the message.
        /// </summary>
        /// <returns>Deep copy of the message.</returns>
        IGameMessage Clone();
    }

    public abstract class GameMessage<T> : IGameMessage where T : GameMessage<T>
    {
        public virtual IGameMessage Clone()
        {
            return CloneThis();
        }

        /// <summary>
        /// Create a deep copy of this message.
        /// 
        /// By default this method does a memberwise clone.
        /// </summary>
        /// <returns>Deep copy of the message.</returns>
        public virtual T CloneThis()
        {
            return (T)MemberwiseClone();
        }
    }
}
