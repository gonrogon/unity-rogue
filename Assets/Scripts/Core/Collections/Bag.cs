using System.Collections.Generic;
using System;

namespace Rogue.Core.Collections
{
    /// <summary>
    /// Defines a bag as a list of identifier/value pairs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Bag<T>
    {
        /// <summary>
        /// List with the items.
        /// </summary>
        private readonly List<IdentValuePair<T>> m_items = new();
        
        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count => m_items.Count;

        /// <summary>
        /// Flag indicating whether the bag is empty or not.
        /// </summary>
        public bool Empty => m_items.Count <= 0;

        /// <summary>
        /// Gets the identifier/value pair at an index.
        /// </summary>
        /// <param name="i">Index.</param>
        /// <returns>Identifier/value pair.</returns>
        public IdentValuePair<T> At(int i) => m_items[i];

        /// <summary>
        /// Checks if the bag contains an identifier.
        /// </summary>
        /// <param name="ident">Identifier.</param>
        /// <returns>True if the bag contains the identifier; otherwise, false.</returns>
        public bool Contains(Ident ident) => FindIndex(ident) >= 0;

        /// <summary>
        /// Finds the index of an indentifier.
        /// </summary>
        /// <param name="ident">Identifier.</param>
        /// <returns>Index of the indetifier if it exists; otherwise, -1.</returns>
        public int FindIndex(Ident ident)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i].ident == ident)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds an identifier/value pair.
        /// </summary>
        /// <remarks>Note that the bag does not check for repetitions.</remarks>
        /// <param name="ident">Identifier.</param>
        /// <param name="value">Value.</param>
        public void Add(Ident ident, T value) => m_items.Add(new IdentValuePair<T>(ident, value));

        /// <summary>
        /// Adds the items from another bag.
        /// </summary>
        /// <param name="bag">Bag to add.</param>
        public void AddRange(Bag<T> bag) => m_items.AddRange(bag.m_items);

        /// <summary>
        /// Removes a pair using the remove and swap technique.
        /// </summary>
        /// <param name="index">Index of the pair to remove.</param>
        public void Remove(int index) => ArrayUtil.RemoveAndSwap(m_items, index);

        /// <summary>
        /// Clears the bag.
        /// </summary>
        public void Clear() => m_items.Clear();
    }
}
