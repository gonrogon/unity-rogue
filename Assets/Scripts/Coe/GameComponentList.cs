using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a list of components.
    /// </summary>
    public class GameComponentList : IEnumerable<IGameComponent>
    {
        /// <summary>
        /// List of components.
        /// </summary>
        private readonly List<IGameComponent> m_list = new ();

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<IGameComponent> GetEnumerator() => m_list.GetEnumerator();

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => m_list.GetEnumerator();

        /// <summary>
        /// Checks if the list contains a type of component.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains<T>() where T : IGameComponent => ImplFindFirstIndex(typeof(T)) >= 0;

        /// <summary>
        /// Finds the nth ocurrence of a component of a type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Nth-component if it exits; otherwise, default.</returns>
        public T Find<T>(int nth) where T : IGameComponent
        {
            int i  = ImplFindIndex(typeof(T), nth);
            if (i >= 0)
            {
                return (T)m_list[i];
            }

            return default;
        }

        /// <summary>
        /// Finds the first component of a type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>Component if it exists; otherwise, default.</returns>
        public T FindFirst<T>() where T : IGameComponent => Find<T>(0);

        /// <summary>
        /// Finds the last component of a type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>Component if it exists; otherwise, default.</returns>
        public T FindLast<T>() where T : IGameComponent
        {
            int i  = ImplFindLastIndex(typeof(T));
            if (i >= 0)
            {
                return (T)m_list[i];
            }

            return default;
        }

        /// <summary>
        /// Finds the nth ocurrence of a component of a type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the component if it exits; otherwise, less than zero.</returns>
        private int ImplFindIndex(Type type, int nth)
        {
            int count =  0;

            for (int i = 0; i < m_list.Count; i++)
            {
                if (m_list[i].GetType() == type && nth == count++)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds the first component of a type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <returns>Index of the component if it exits; otherwise, less than zero.</returns>
        private int ImplFindFirstIndex(Type type) => ImplFindIndex(type, 0);

        /// <summary>
        /// Finds the last component of a type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <returns>Index of the component if it exits; otherwise, less than zero.</returns>
        private int ImplFindLastIndex(Type type)
        {
            for (int i = m_list.Count -1; i >= 0; i--)
            {
                if (m_list[i].GetType() == type)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds a component to the list.
        /// </summary>
        /// <param name="component">Component to add.</param>
        /// <returns>Reference to the component on success; otherwise, null.</returns>
        public IGameComponent Add(IGameComponent component)
        {
            IGameComponent prev = null;
            IGameComponent cnew = component;

            int i  = ImplFindLastIndex(component.GetType());
            if (i >= 0)
            {
                prev = m_list[i];
            }

            #if DEBUG
                if (prev != null)
                {
                    Debug.Assert(prev.Next == null, "The next component of the last one is not null");
                }
            #endif

            prev?.LinkNext(cnew);
            cnew .LinkPrev(prev);
            cnew .LinkNext(null);

            m_list.Add(cnew);

            return component;
        }

        /// <summary>
        /// Removes the nth ocurrence of a component.
        /// </summary>
        /// <typeparam name="T">Type of component to remove.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Reference to the removed component or null if no component was removed.</returns>
        public IGameComponent Remove<T>(int nth)
        {
            int i = ImplFindIndex(typeof(T), nth);
            if (i < 0)
            {
                return null;
            }

            return ImplRemove(i);
        }

        /// <summary>
        /// Removes all components of a type.
        /// </summary>
        /// <typeparam name="T">Type of component to remove.</typeparam>
        /// <param name="onRemoved">Action to invoke when a component is removed.</param>
        /// <returns>True if at least one component was removed; otherwise, false.</returns>
        public void RemoveAll<T>(Action<IGameComponent> onRemoved)
        {
            for (int i = 0; i < m_list.Count;)
            {
                if (m_list[i].GetType() == typeof(T))
                {
                    IGameComponent c = ImplRemove(i);
                    if (c != null)
                    {
                        onRemoved?.Invoke(c);
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Implements the removal of a component.
        /// </summary>
        /// <param name="index">Index of the component to remove.</param>
        /// <returns>Reference to the removed component or null if no component was removed.</returns>
        public IGameComponent ImplRemove(int index)
        {
            IGameComponent c = m_list[index];

            c.Prev?.LinkNext(c.Next);
            c.Next?.LinkPrev(c.Prev);

            m_list.RemoveAt(index);
            return c;
        }
    }
}
