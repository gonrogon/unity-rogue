using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rogue.Core;

namespace Rogue.Coe
{
    /// <summary>
    /// Define an interface for components.
    /// </summary>
    public interface IGameBehaviour
    {
        /// <summary>
        /// Get the priority of this type of behavior.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Get the entity identifier.
        /// </summary>
        Ident Eid { get; }

        /// <summary>
        /// Get the world.
        /// </summary>
        GameWorld World { get; }

        /// <summary>
        /// Get the entity.
        /// </summary>
        GameEntity Entity { get; }

        /// <summary>
        /// Get a disconnected copy of the behavior.
        /// </summary>
        /// <returns>Clone.</returns>
        IGameBehaviour Clone();

        /// <summary>
        /// Set up the behavior.
        /// </summary>
        /// <param name="world">World.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>True on success; otherwise false.</returns>
        bool OnSetup(GameWorld world, GameEntity entity);

        /// <summary>
        /// Initiates the behaviour.
        /// </summary>
        void OnInit();

        /// <summary>
        /// Finalizes the behaviour.
        /// </summary>
        void OnQuit();

        /// <summary>
        /// Handles a game message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Message state.</returns>
        GameMessageState OnMessage(IGameMessage message);

        /// <summary>
        /// Check if two behaviours has the same type.
        /// </summary>
        /// <param name="component">Behaviour.</param>
        /// <returns>True if both behaviours have the same type; otherwise, false.</returns>
        bool EqualType(IGameBehaviour behaviour);
    }

    
    public abstract class GameBehaviour<T> : IGameBehaviour where T : GameBehaviour<T>, new()
    {
        /// <summary>
        /// Get the behaviour type (serialization).
        /// </summary>
        [JsonProperty(PropertyName = "class", Order = int.MinValue)]
        private string Class => GetType().Name;

        [JsonIgnore]
        public int Priority { get; private set; }

        [JsonIgnore]
        public Ident Eid { get; private set; }
        
        [JsonIgnore]
        public GameWorld World { get; private set; }

        [JsonIgnore]
        public GameEntity Entity { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected GameBehaviour()
        {
            Priority = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="priority">Priority.</param>
        protected GameBehaviour(int priority)
        {
            Priority = priority;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="component">Component to copy.</param>
        protected GameBehaviour(GameBehaviour<T> behaviour)
        {
            Priority = behaviour.Priority;
        }

        public virtual IGameBehaviour Clone()
        {
            T clone = (T)MemberwiseClone();

            clone.World  = null;
            clone.Entity = null;
            clone.Eid    = Ident.Zero;

            return clone;
        }


        /// <summary>
        /// Default implementation save sthe world, entity and entity identifier for later usege.
        /// </summary>
        /// <param name="world">World.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>True on success; toerhwise, false.</returns>
        public virtual bool OnSetup(GameWorld world, GameEntity entity)
        {
            World  = world;
            Entity = entity;
            Eid    = entity.Id;

            return true;
        }

        /// <summary>
        /// Default implementation does nothing.
        /// </summary>
        public virtual void OnInit() {}

        /// <summary>
        /// Default implementation does nothing.
        /// </summary>
        public virtual void OnQuit() {}
        
        /// <summary>
        /// Default implementation does nothing.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Continue.</returns>
        public virtual GameMessageState OnMessage(IGameMessage message) => GameMessageState.Continue;

        public bool EqualType(IGameBehaviour behaviour)
        {
            return GetType() == behaviour.GetType();

        }
    }
}
