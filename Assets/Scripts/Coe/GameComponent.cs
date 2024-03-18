using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    /// <summary>
    /// Define an interface for components.
    /// </summary>
    public interface IGameComponent
    {
        /// <summary>
        /// Get the previous component of the same type or null if it does not exists.
        /// </summary>
        IGameComponent Prev { get; }

        /// <summary>
        /// Get the next component of the same type or null if it does not exists.
        /// </summary>
        IGameComponent Next { get; }

        /// <summary>
        /// Get an unlinked copy of the component.
        /// </summary>
        /// <returns>Clone.</returns>
        IGameComponent Clone();

        /// <summary>
        /// Link a component with the next component of tha same type.
        /// </summary>
        /// <param name="prev">Previous component of the same type or null if it does not exists.</param>
        void LinkPrev(IGameComponent prev);

        /// <summary>
        /// Link a component with the previous component of tha same type.
        /// </summary>
        /// <param name="next">Next component of the same type or null if it does not exists.</param>
        void LinkNext(IGameComponent next);

        /// <summary>
        /// Check if two component has the same type.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>True if both components have the same type; otherwise, false.</returns>
        bool EqualType(IGameComponent component);
    }

    /// <summary>
    /// Define a base component.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GameComponent<T> : IGameComponent where T : GameComponent<T>, new()
    {
        [JsonIgnore]
        public IGameComponent Prev { get; private set; } = null;

        [JsonIgnore]
        public IGameComponent Next { get; private set; } = null;

        [JsonIgnore]
        public T PrevThis => (T)Prev;

        [JsonIgnore]
        public T NextThis => (T)Next;

        public virtual IGameComponent Clone()
        {
            T clone = (T)MemberwiseClone();

            clone.Prev = null;
            clone.Next = null;

            return clone;
        }

        public void LinkPrev(IGameComponent prev)
        {
            if (prev != null && !EqualType(prev)) { throw new System.ArgumentException("Unable to link, invalid component type", "prev"); }

            Prev = prev;
        }

        public void LinkNext(IGameComponent next)
        {
            if (next != null && !EqualType(next)) { throw new System.ArgumentException("Unable to link, invalid component type", "next"); }

            Next = next;
        }

        public bool EqualType(IGameComponent component)
        {
            return GetType() == component.GetType();
        }
    }
}
