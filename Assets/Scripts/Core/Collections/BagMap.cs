using System;
using System.Collections.Generic;

namespace Rogue.Core.Collections 
{
    /// <summary>
    /// Defines a dictionary that map integers to bags.
    /// </summary>
    /// <typeparam name="K">The type of the keys of the bags.</typeparam>
    /// <typeparam name="V">The type of the values of the bags.</typeparam>
    public class BagMap<K, V>
    {
        /// <summary>
        /// List of bags.
        /// </summary>
        private readonly List<Bag<K, V>> m_bags = new ();

        /// <summary>
        /// List of free bags (bag ready to be reused).
        /// </summary>
        private readonly Queue<int> m_free = new ();

        /// <summary>
        /// Creates a new bag.
        /// </summary>
        /// <returns>Index of the bag.</returns>
        public int Create()
        {
            int index;

            if (m_free.Count > 0)
            {
                index = m_free.Dequeue();
            }
            else
            {
                index = m_bags.Count;
                        m_bags.Add(new Bag<K, V>());
            }

            return index;
        }

        /// <summary>
        /// Releases a bag.
        /// 
        /// After the call to this function the bag is ready to be reused by a call to "create".
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        public void Release(int bag)
        {
            m_bags[bag].Clear();
            m_free.Enqueue(bag);
        }

        /// <summary>
        /// Checks if there are no elements inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <returns>True if there are not elements in a bag; otherwise, false.</returns>
        public bool Empty(int bag) => m_bags[bag].Empty;
        

        /// <summary>
        /// Gets the number of elements in a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <returns>Number of elements in the bag.</returns>
        public int Count(int bag) => m_bags[bag].Count;

        /// <summary>
        /// Checks if a bag contains an element.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="key">Key.</param>
        /// <returns>True if the element is in the bag; otherwise, false.</returns>
        public bool Contains(int bag, K key) => m_bags[bag].Contains(key);

        /// <summary>
        /// Gets the first element inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <returns>True if there is at least one element in the bag; otherwise, false.</returns>
        public bool TryGetFirst(int bag, out KeyValuePair<K, V> pair) => TryGetNth(bag, 0, out pair);

        /// <summary>
        /// Gets nth element inside a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="nth">Number.</param>
        /// <returns>True if there is at least nth elements in the bag; otherwise, false.</returns>
        public bool TryGetNth(int bag, int nth, out KeyValuePair<K, V> pair)
        {
            pair = new KeyValuePair<K, V>(default, default);

            if (nth >= Count(bag))
            {
                return false;
            }

            pair = m_bags[bag].At(nth);

            return true;
        }

        /// <summary>
        /// Performs the specified operation on each element of a bag.
        /// </summary>
        /// <param name="bag">Bag.</param>
        /// <param name="action">Action.</param>
        public void ForEach(int bag, Action<K, V> action)
        {
            for (int i = 0; TryGetNth(bag, i, out var pair); i++)
            {
                action(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Adds a key/value pair to a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(int bag, K key, V value) => m_bags[bag].Add(key, value);

        /// <summary>
        /// Adds a key/value pair to a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="pair">Pair.</param>
        public void Add(int bag, KeyValuePair<K, V> pair) => m_bags[bag].Add(pair);

        /// <summary>
        /// Moves an element from a bag to another one.
        /// </summary>
        /// <param name="origin">Index of the origin bag.</param>
        /// <param name="target">Index of the target bag.</param>
        /// <param name="key">Key.</param>
        public void Move(int origin, int target, K key)
        {
            if (origin == target)
            {
                return;
            }

            int index = m_bags[origin].FindFirst(key);
            if (index < 0)
            {
                return;
            }

            var pair = m_bags[origin].At(index);

            m_bags[origin].Remove(index);
            m_bags[target].Add   (pair);
        }

        /// <summary>
        /// Moves all elements from a bag to another one.
        /// </summary>
        /// <param name="origin">Index of the origin bag.</param>
        /// <param name="target">Index of the target bag.</param>
        public void MoveAll(int origin, int target)
        {
            if (origin == target)
            {
                return;
            }

            if (Empty(origin))
            {
                return;
            }

            Bag<K, V> originBag = m_bags[origin];
            Bag<K, V> targetBag = m_bags[target];

            if (Empty(target))
            {
                m_bags[origin] = targetBag;
                m_bags[target] = originBag;
            }
            else
            {
                targetBag.AddRange(originBag);
                originBag.Clear();
            }
        }

        /// <summary>
        /// Removes an element from a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        /// <param name="key">Key.</param>
        /// <returns>Number of pairs removed from the bag.</returns>
        public int Remove(int bag, K key)
        {
            if (Empty(bag))
            {
                return 0;
            }

            var list  = m_bags[bag];
            int index = m_bags[bag].FindFirst(key);

            if (index < 0)
            {
                return 0;
            }

            list.Remove(index);

            return 1;
        }

        /// <summary>
        /// Removes all the elements from a bag.
        /// </summary>
        /// <param name="bag">Index of the bag.</param>
        public void RemoveAll(int bag) => m_bags[bag].Clear();
    }
}