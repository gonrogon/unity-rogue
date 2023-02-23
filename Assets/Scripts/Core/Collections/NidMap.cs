using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rogue.Core.Collections
{

    public class NidMap<T> : IEnumerable<NidMap<T>.Pair>
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
            public bool valid;

            /// <summary>
            /// Value.
            /// </summary>
            public T value;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="valid">Flag indicating whether the pair is valid or not.</param>
            /// <param name="value">Value.</param>
            public Pair(bool valid, T value)
            {
                this.valid = valid;
                this.value = value;
            }
        }

        /// <summary>
        /// Queue of identifier to reuse (FIFO).
        /// </summary>
        private Queue<int> m_reuse;

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
        public NidMap() : this(0, DefaultReuseLimit, DefaultReuseFactor) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        /// <param name="reuseLimit">Minimum number of items before reusing identifiers.</param>
        /// <param name="reuseFactor">Minimum perfectage of free identifiers before reusing them.</param>
        public NidMap(int capacity, int reuseLimit, int reuseFactor)
        {
            m_items       = new List<Pair>(capacity);
            m_reuse       = new Queue<int>();
            m_reuseLimit  = reuseLimit;
            m_reuseFactor = reuseFactor;
        }

        public bool Contains(int nid)
        {
            if (nid < 0 || nid >= m_items.Count)
            {
                return false;
            }

            return m_items[nid].valid;
        }

        public T Get(int nid)
        {
            return m_items[nid].value;
        }

        public bool TryFind(int nid, out T value)
        {
            value = default(T);

            if (nid < 0 || nid >= m_items.Count)
            {
                return false;
            }

            Pair pair = m_items[nid];

            if (!pair.valid)
            {
                return false;
            }

            value = pair.value;
            return true;
        }

        public int Add(T value)
        {
            if (!Reuse(out int nid))
            {
                nid = m_items.Count;
            }

            m_items.Insert(nid, new Pair(true, value));

            return nid;
        }

        public void Overwrite(int nid, T value)
        {
            if (nid < 0 || nid >= m_items.Count)
            {
                throw new ArgumentOutOfRangeException("Ident not found", "id");
            }
            // TODO: Check if the nid is free.
            m_items[nid] = new Pair(true, value);
        }

        public void Release(int nid)
        {
            if (!Contains(nid))
            {
                return;
            }
            // Reset the item in the list with a empty id and a default value.
            m_items[nid] = new Pair(false, default(T));
            // Add id to the queue of free ids.
            m_reuse.Enqueue(nid);
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

        private static IEnumerable<Pair> Iterate(NidMap<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.m_items[i].valid)
                {
                    yield return list.m_items[i];
                }
            }
        }

        // ---------
        // UTILITIES
        // ---------

        private bool Reuse(out int nid)
        {
            nid = -1;

            if (m_reuse.Count < 0)
            {
                return false;
            }

            if (m_items.Count < m_reuseLimit || m_reuse.Count < (m_items.Count * m_reuseFactor) / 100)
            {
                return false;
            }

            nid = m_reuse.Dequeue();

            return true;
        }
    }
}
