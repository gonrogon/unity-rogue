using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a list of behaviours.
    /// </summary>
    public class GameBehaviourList : IEnumerable<IGameBehaviour>
    {
        /// <summary>
        /// List of behaviours.
        /// </summary>
        private readonly List<IGameBehaviour> m_list = new ();

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<IGameBehaviour> GetEnumerator() => m_list.GetEnumerator();

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => m_list.GetEnumerator();

        /// <summary>
        /// Checks if the list contains a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour.</typeparam>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains<T>() where T : IGameBehaviour => ImplFindIndex(typeof(T)) >= 0;

        /// <summary>
        /// Finds a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour.</typeparam>
        /// <returns>Reference to the behaviour if it exits; otherwise, default.</returns>
        public T Find<T>() where T : IGameBehaviour
        {
            int i  = ImplFindIndex(typeof(T));
            if (i >= 0)
            {
                return (T)m_list[i];
            }

            return default;
        }

        /// <summary>
        /// Finds the nth ocurrence of a behaviour of a type.
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
        /// Adds a behaviour to the list.
        /// </summary>
        /// <remarks>Behaviours are ordered by priority.</remarks>
        /// <param name="behaviour">Behaviour to add.</param>
        /// <returns>Reference to the behaviour on success; otherwise, null.</returns>
        public IGameBehaviour Add(IGameBehaviour behaviour)
        {
            int i  = ImplFindIndex(behaviour.GetType());
            if (i >= 0)
            {
                Debug.LogWarning($"List already contains a behavior of type {behaviour.GetType()}");
                return null;
            }

            return ImplInsert(behaviour);
        }

        /// <summary>
        /// Inserts a behavior.
        /// </summary>
        /// <remarks>Behaviours are ordered by priority.</remarks>
        /// <param name="behavior">Behaviour to insert.</param>
        /// <returns>Reference to the behaviour on success; otherwise, null.</returns>
        private IGameBehaviour ImplInsert(IGameBehaviour behavior)
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
            return m_list[found];
        }

        /// <summary>
        /// Removes a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to remove.</typeparam>
        /// <returns>Reference to the removed behaviour or null if no behaviour was removed.</returns>
        public IGameBehaviour Remove<T>()
        {
            int i  = ImplFindIndex(typeof(T));
            if (i < 0)
            {
                return null;
            }

            return ImplRemove(i);
        }

        /// <summary>
        /// Implements the removal of a behaviour.
        /// </summary>
        /// <param name="index">Index of the behaviour to remove.</param>
        /// <returns>Removed behaviour.</returns>
        public IGameBehaviour ImplRemove(int index)
        {
            IGameBehaviour b = m_list[index];

            m_list.RemoveAt(index);
            return b;
        }
    }
}
