using System.Collections.Generic;
using System.Collections;
using System;

namespace Rogue.Core.Collections
{
    /// <summary>
    /// Defines a bag as a list of key/value pairs.
    /// </summary>
    /// <typeparam name="K">The type of the keys.</typeparam>
    /// <typeparam name="V">The type of the values.</typeparam>
    public class Bag<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        /// <summary>
        /// List with the items.
        /// </summary>
        private readonly List<KeyValuePair<K, V>> m_items = new ();
        
        /// <summary>
        /// Comparer.
        /// </summary>
        private readonly IEqualityComparer<K> m_comparer;

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count => m_items.Count;

        /// <summary>
        /// Flag indicating whether the bag is empty or not.
        /// </summary>
        public bool Empty => m_items.Count <= 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Bag() => m_comparer = EqualityComparer<K>.Default;

        /// <summary>
        /// Gets an identifier.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Identifier.</returns>
        public K GetKey(int index) => m_items[index].Key;

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Value.</returns>
        public V GetValue(int index) => m_items[index].Value;

        /// <summary>
        /// Gets a key/value pair.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Identifier/value pair.</returns>
        public KeyValuePair<K, V> At(int index) => m_items[index];

        /// <summary>
        /// Checks if the bag contains an item with the specified key.
        /// </summary>
        /// <param name="ident">Identifier.</param>
        /// <returns>True if the bag contains the identifier; otherwise, false.</returns>
        public bool Contains(K key) => FindFirst(key) >= 0;

        /// <summary>
        /// Finds the first item with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Index of the first item with the identifier if it exists; otherwise, less than zero.</returns>
        public int FindFirst(K key)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_comparer.Equals(m_items[i].Key, key))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds a key/value pair.
        /// </summary>
        /// <remarks>Note that the bag does not check for repetitions.</remarks>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(K key, V value) => m_items.Add(new KeyValuePair<K, V>(key, value));

        /// <summary>
        /// Adds a key/value pair.
        /// </summary>
        /// <remarks>Note that the bag does not check for repetitions.</remarks>
        /// <param name="item">Pair to add.</param>
        public void Add(KeyValuePair<K, V> pair) => m_items.Add(pair);

        /// <summary>
        /// Adds the items from another bag.
        /// </summary>
        /// <param name="bag">Bag to add.</param>
        public void AddRange(Bag<K, V> bag) => m_items.AddRange(bag.m_items);

        /// <summary>
        /// Removes the first item with the identifier.
        /// </summary>
        /// <param name="key">Key of the item to remove.</param>
        public void Remove(K key)
        {
            int index = FindFirst(key);
            if (index < 0)
            {
                return;
            }

            ArrayUtil.RemoveAndSwap(m_items, index);
        }

        /// <summary>
        /// Removes a item.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        public void Remove(int index) => ArrayUtil.RemoveAndSwap(m_items, index);

        /// <summary>
        /// Removes all the items with the specified key.
        /// </summary>
        /// <param name="key">Key of the items to remove.</param>
        /// <returns>The number of items removed from the bag.</returns>
        public int RemoveAll(K key)
        {
            int count = 0;

            for (int i = 0; i < m_items.Count;)
            {
                if (m_comparer.Equals(m_items[i].Key, key))
                {
                    ArrayUtil.RemoveAndSwap(m_items, i);
                    count++;

                    continue;
                }

                ++i;
            }

            return count;
        }

        /// <summary>
        /// Removes all the items that match the conditions defined by the specific predicate.
        /// </summary>
        /// <param name="match">Predicate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of items removed from the bag.</returns>
        public int RemoveAll(Predicate<KeyValuePair<K, V>> match)
        {
            int count = 0;

            for (int i = 0; i < m_items.Count;)
            {
                if (match(m_items[i]))
                {
                    ArrayUtil.RemoveAndSwap(m_items, i);
                    count++;

                    continue;
                }

                ++i;
            }

            return count;
        }

        /// <summary>
        /// Clears the bag.
        /// </summary>
        public void Clear() => m_items.Clear();

        /// <summary>
        /// Gets a forward iterator.
        /// </summary>
        /// <returns>Iterator.</returns>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => Iterate(this).GetEnumerator();

        /// <summary>
        /// Gets a forward iterator.
        /// </summary>
        /// <returns>Iterator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Implements a forward iterator.
        /// </summary>
        /// <param name="bag">Bag to iterate.</param>
        /// <returns>Iterator.</returns>
        private static IEnumerable<KeyValuePair<K, V>> Iterate(Bag<K, V> bag)
        {
            for (int i = 0; i <= bag.m_items.Count; ++i)
            {
                yield return bag.m_items[i];
            }
        }
    }
}
