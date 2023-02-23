using System;
using System.Collections.Generic;

namespace Rogue.Core.Collections
{
    public class MultiMap<Key, Value>
    {
        /// <summary>
        /// Items.
        /// </summary>
        private readonly Dictionary<Key, List<Value>> m_items = new();

        /// <summary>
        /// Gets a collection with the keys.
        /// </summary>
        public Dictionary<Key, List<Value>>.KeyCollection Keys => m_items.Keys;

        /// <summary>
        /// Gets a collection with the values.
        /// </summary>
        public Dictionary<Key, List<Value>>.ValueCollection Values => m_items.Values;

        public int Count(Key key)
        {
            if (!m_items.TryGetValue(key, out var list))
            {
                return 0;
            }

            return list.Count;
        }

        public Value Get(Key key, int index)
        {
            if (!m_items.TryGetValue(key, out var list))
            {
                return default;
            }

            return list[index];
        }

        public void Replace(Key key, int index, Value value)
        {
            if (!m_items.TryGetValue(key, out var list))
            {
                return;
            }

            if (list.Count <= index)
            {
                return;
            }

            list[index] = value;
        }

        public void Add(Key key, Value value)
        {
            FindOrCreate(key).Add(value);
        }

        public void Remove(Key key)
        {
            m_items.Remove(key);
        }

        public void Remove(Key key, Value item)
        {
            if (!m_items.TryGetValue(key, out var list))
            {
                return;
            }

            if (list.Count <= 1)
            {
                Remove(key);
                return;
            }

            list.Remove(item);
        }

        private List<Value> FindOrCreate(Key key)
        {
            if (!m_items.TryGetValue(key, out var list))
            {
                return m_items[key] = new ();
            }
            
            return list;
        }
    }
}
