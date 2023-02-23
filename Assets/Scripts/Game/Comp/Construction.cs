using GG.Mathe;

namespace Rogue.Game.Comp
{
    public interface IConstruction
    {        
        /// <summary>
        /// Identifier of the job assigned to this construction.
        /// </summary>
        int Job { get; }

        /// <summary>
        /// Total amount of work to complete the construction.
        /// </summary>
        int TotalWork { get; }

        /// <summary>
        /// Percentage of the job already completed.
        /// </summary>
        int Progress { get; }

        /// <summary>
        /// Flag indicating whether the job is completed or not.
        /// </summary>
        bool Done { get; }

        /// <summary>
        /// Set the job assigned to this construction.
        /// </summary>
        /// <param name="jid">Job identifier.</param>
        public void SetJob(int jid);

        /// <summary>
        /// Advance the job by an amount of work.
        /// </summary>
        /// <param name="amount">Amount to advance.</param>
        /// <returns>True if the job is completed; otherwise, false.</returns>
        public bool Advance(int amount);

        /// <summary>
        /// Construct.
        /// </summary>
        /// <param name="coord">Location where to place the construction.</param>
        public void Construct(Vec2i coord);
    }
}
