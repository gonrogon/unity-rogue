using System;
using System.Collections;
using System.Collections.Generic;

namespace Rogue.Core.Collections
{
    /// <summary>
    /// Defines a list that maps identifiers to values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IdentMap<T> : IEnumerable<KeyValuePair<Ident, T>>
    {
        /// <summary>
        /// Default minimum number of items before reusing the identifiers.
        /// </summary>
        public const int DefaultReuseLimit = 10000;

        /// <summary>
        /// Default minimum perfectage of free identifiers before reusing them.
        /// </summary>
        public const int DefaultReuseFactor = 10;

        /// <summary>
        /// List of items.
        /// </summary>
        private readonly List<KeyValuePair<Ident, T>> m_items;

        /// <summary>
        /// Queue of identifier to reuse (FIFO).
        /// </summary>
        private readonly Queue<Ident> m_reuse;

        /// <summary>
        /// Minimum number of items before reusing identifiers.
        /// </summary>
        private readonly int m_reuseLimit;

        /// <summary>
        /// Minimum perfectage of free identifiers before reusing them.
        /// </summary>
        private readonly int m_reuseFactor;

        /// <summary>
        /// Flag indicating whether the list is empty or not.
        /// </summary>
        public bool Empty => m_items.Count == 0;

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count => m_items.Count;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IdentMap() : this(0, DefaultReuseLimit, DefaultReuseFactor) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        /// <param name="reuseLimit">Minimum number of items before reusing identifiers.</param>
        /// <param name="reuseFactor">Minimum perfectage of free identifiers before reusing them.</param>
        public IdentMap(int capacity, int reuseLimit, int reuseFactor)
        {
            m_items       = new (capacity);
            m_reuse       = new ();
            m_reuseLimit  = reuseLimit;
            m_reuseFactor = reuseFactor;
        }

        /// <summary>
        /// Checks whether or not the map contains an element.
        /// </summary>
        /// <param name="key">Identifier.</param>
        /// <returns>True if the map contains the element; otherwise, false.</returns>
        public bool Contains(Ident key)
        {
            if (key.Raw == 0 || key.Value >= m_items.Count)
            {
                return false;
            }

            return m_items[key.ValueAsIndex].Key == key;
        }

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <param name="key">Identifier.</param>
        /// <returns>Value.</returns>
        public T Get(Ident key) => m_items[key.ValueAsIndex].Value;

        /// <summary>
        /// Tries to find an element.
        /// </summary>
        /// <param name="key">Identifier of the element.</param>
        /// <param name="value">Value.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool TryFind(Ident key, out T value)
        {
            value = default;
            // Check if the key is valid.
            if (key.Raw == 0 || key.Value >= m_items.Count)
            {
                return false;
            }
            // Check if both keys are equal.
            var pair = m_items[key.ValueAsIndex];
            if (pair.Key != key)
            {
                return false;
            }
            // Done.
            value = pair.Value;
            return true;
        }

        /// <summary>
        /// Adds a new element to the map.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>Identifier assigned to the element.</returns>
        public Ident Add(T value)
        {
            if (!Reuse(out Ident id))
            {
                id = new Ident((uint)m_items.Count, 1);
            }

            m_items.Insert(id.ValueAsIndex, new (id, value));

            return id;
        }

        /// <summary>
        /// Overwrites an element.
        /// </summary>
        /// <param name="key">Identifier of the element to be overwritten.</param>
        /// <param name="value">Value to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">Identifier not found.</exception>
        public void Overwrite(Ident key, T value)
        {
            if (key.Raw == 0 || key.Value >= m_items.Count || m_items[key.ValueAsIndex].Key == Ident.Zero)
            {
                throw new ArgumentOutOfRangeException("Ident not found", "id");
            }

            m_items[key.ValueAsIndex] = new (key, value);
        }

        /// <summary>
        /// Releases an element.
        /// </summary>
        /// <param name="key">Identifier of the element to be released.</param>
        public void Release(Ident key)
        {
            if (!Contains(key))
            {
                return;
            }
            // Reset the item in the list with a empty id and a default value.
            m_items[key.ValueAsIndex] = new (Ident.Zero, default);
            // Add id to the queue of free ids.
            m_reuse.Enqueue(key);
        }

        /// <summary>
        /// Clears the map.
        /// </summary>
        public void Clear()
        {
            m_items.Clear();
            m_reuse.Clear();
        }

        /// <summary>
        /// Gets a forward iterator.
        /// </summary>
        /// <returns>Iterator.</returns>
        public IEnumerator<KeyValuePair<Ident, T>> GetEnumerator() => Iterate(this).GetEnumerator();

        /// <summary>
        /// Gets a forward iterator.
        /// </summary>
        /// <returns>Iterator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Implements a forward iterator.
        /// </summary>
        /// <param name="map">Map to iterate.</param>
        /// <returns>Iterator.</returns>
        private static IEnumerable<KeyValuePair<Ident, T>> Iterate(IdentMap<T> map)
        {
            for (int i = 0; i < map.Count; i++)
            {
                if (!map.m_items[i].Key.IsZero)
                {
                    yield return map.m_items[i];
                }
            }
        }

        /// <summary>
        /// Gets an identifier to reuse.
        /// </summary>
        /// <param name="key">Identifier to reuse.</param>
        /// <returns>True if there is a identifier to reuse; otherwise, false.</returns>
        private bool Reuse(out Ident key)
        {
            key = Ident.Zero;

            if (m_reuse.Count < 0)
            {
                return false;
            }

            if (m_items.Count < m_reuseLimit || m_reuse.Count < (m_items.Count * m_reuseFactor) / 100)
            {
                return false;
            }

            key = m_reuse.Dequeue().NextGenerationSkipZero();

            return true;
        }
    }
}
