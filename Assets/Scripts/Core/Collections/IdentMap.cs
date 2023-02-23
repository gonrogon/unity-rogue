using System;
using System.Collections;
using System.Collections.Generic;

namespace Rogue.Core.Collections
{

    public class IdentMap<T> : IEnumerable<IdentMap<T>.Pair>
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
        /// Define a item of the list.
        /// </summary>
        public struct Pair
        {
            /// <summary>
            /// Identifier.
            /// </summary>
            public Ident id;

            /// <summary>
            /// Value.
            /// </summary>
            public T value;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="id">Identifier.</param>
            /// <param name="value">Value.</param>
            public Pair(Ident id, T value)
            {
                this.id    = id;
                this.value = value;
            }
        }

        /// <summary>
        /// Queue of identifier to reuse (FIFO).
        /// </summary>
        private Queue<Ident> m_reuse;

        /// <summary>
        /// List of items.
        /// </summary>
        private List<Pair> m_items;

        /// <summary>
        /// Minimum number of items before reusing identifiers.
        /// </summary>
        private int m_reuseLimit;

        /// <summary>
        /// Minimum perfectage of free identifiers before reusing them.
        /// </summary>
        private int m_reuseFactor;

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
            m_items       = new List<Pair>(capacity);
            m_reuse       = new Queue<Ident>();
            m_reuseLimit  = reuseLimit;
            m_reuseFactor = reuseFactor;
        }

        public bool Contains(Ident id)
        {
            if (id.Raw == 0 || id.Value >= m_items.Count)
            {
                return false;
            }

            return m_items[id.ValueAsIndex].id == id;
        }

        public T Get(Ident id)
        {
            return m_items[id.ValueAsIndex].value;
        }

        public bool TryFind(Ident id, out T value)
        {
            value = default(T);

            if (id.Raw == 0 || id.Value >= m_items.Count)
            {
                return false;
            }

            Pair pair = m_items[id.ValueAsIndex];

            if (pair.id != id)
            {
                return false;
            }

            value = pair.value;
            return  true;
        }

        public Ident Add(T value)
        {
            if (!Reuse(out Ident id))
            {
                id = new Ident((uint)m_items.Count, 1);
            }

            m_items.Insert(id.ValueAsIndex, new Pair(id, value));

            return id;
        }

        public void Overwrite(Ident id, T value)
        {
            if (id.Raw == 0 || id.Value >= m_items.Count)
            {
                throw new ArgumentOutOfRangeException("Ident not found", "id");
            }
            // TODO: Check if the ident is free.
            m_items[id.ValueAsIndex] = new Pair(id, value);
        }

        public void Release(Ident id)
        {
            if (!Contains(id))
            {
                return;
            }
            // Reset the item in the list with a empty id and a default value.
            m_items[id.ValueAsIndex] = new Pair(Ident.Zero, default(T));
            // Add id to the queue of free ids.
            m_reuse.Enqueue(id);
        }

        public void Clear()
        {
            m_items.Clear();
            m_reuse.Clear();
        }

        // ----------
        // ENUMERATOR
        // ----------

        public IEnumerator<Pair> GetEnumerator()
        {
            return Iterate(this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IEnumerable<Pair> Iterate(IdentMap<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!list.m_items[i].id.IsZero)
                {
                    yield return list.m_items[i];
                }
            }
        }

        // ---------
        // UTILITIES
        // ---------

        private bool Reuse(out Ident ident)
        {
            ident = Ident.Zero;

            if (m_reuse.Count < 0)
            {
                return false;
            }

            if (m_items.Count < m_reuseLimit || m_reuse.Count < (m_items.Count * m_reuseFactor) / 100)
            {
                return false;
            }

            ident = m_reuse.Dequeue().NextGenerationSkipZero();

            return true;
        }
    }
}
