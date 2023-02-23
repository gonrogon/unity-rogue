using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Coe
{
    /// <summary>
    /// Define a type for a list of behaviours.
    /// </summary>
    public class GameBehaviourList : IEnumerable<IGameBehaviour>
    {
        /// <summary>
        /// List of behaviours.
        /// </summary>
        private List<IGameBehaviour> m_list = new List<IGameBehaviour>();

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<IGameBehaviour> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        /// <summary>
        /// Check if the list contains a type of behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour.</typeparam>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains<T>() where T : IGameBehaviour
        {
            return ImplFindIndex(typeof(T)) >= 0;
        }

        /// <summary>
        /// Find the nth ocurrence of a behaviour of a type.
        /// </summary>
        /// <typeparam name="T">Type of behaviour.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Nth-behaviour if it exits; otherwise, default.</returns>
        public T Find<T>() where T : IGameBehaviour
        {
            int i  = ImplFindIndex(typeof(T));
            if (i >= 0)
            {
                return (T)m_list[i];
            }

            return default(T);
        }

        /// <summary>
        /// Find the nth ocurrence of a behaviour of a type.
        /// </summary>
        /// <param name="type">Type of behaviour.</param>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the behaviour if it exits; otherwise, less than zero.</returns>
        private int ImplFindIndex(Type type)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                if (m_list[i].GetType() == type)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Add a new behaviour to the list.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to add.</typeparam>
        public void Add<T>() where T : IGameBehaviour, new()
        {
            Add(new T());
        }

        /// <summary>
        /// Add a behaviour to the list.
        /// </summary>
        /// <param name="behaviour">Behaviour to add.</param>
        public void Add(IGameBehaviour behaviour)
        {
            int i  = ImplFindIndex(behaviour.GetType());
            if (i >= 0)
            {
                Debug.LogWarning($"List already contains a behavior of type {behaviour.GetType()}");
                return;
            }

            ImplInsert(behaviour);
        }

        /// <summary>
        /// Insert a new behavior ordered by priority.
        /// </summary>
        /// <param name="behavior">Behaviour to insert.</param>
        private void ImplInsert(IGameBehaviour behavior)
        {
            int found = -1;

            for (int i = 0; i < m_list.Count && found < 0; i++)
            {
                if (m_list[i].Priority < behavior.Priority)
                {
                    found = i;
                }
            }

            if (found < 0)
            {
                found = m_list.Count;
            }

            m_list.Insert(found, behavior);
        }

        /// <summary>
        /// Remove a type of behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour.</typeparam>
        public void Remove<T>()
        {
            int i  = ImplFindIndex(typeof(T));
            if (i >= 0)
            {
                ImplRemove(i);
            }
        }

        /// <summary>
        /// Implement the removal of a behaviour.
        /// </summary>
        /// <param name="index">Index of the behaviour to remove.</param>
        public void ImplRemove(int index)
        {
            m_list.RemoveAt(index);
        }
    }
}
