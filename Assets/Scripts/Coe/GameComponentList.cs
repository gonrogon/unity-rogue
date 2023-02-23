using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Coe
{
    /// <summary>
    /// Define a type for a list of components.
    /// </summary>
    public class GameComponentList : IEnumerable<IGameComponent>
    {
        /// <summary>
        /// List of components.
        /// </summary>
        private List<IGameComponent> m_list = new List<IGameComponent>();

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<IGameComponent> GetEnumerator()
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
        /// Check if the list contains a type of component.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains<T>() where T : IGameComponent
        {
            return ImplFindFirstIndex(typeof(T)) >= 0;
        }

        /// <summary>
        /// Find the nth ocurrence of a component of a type.
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

            return default(T);
        }

        /// <summary>
        /// Find the first component of a type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>Component if it exists; otherwise, default.</returns>
        public T FindFirst<T>() where T : IGameComponent
        {
            return Find<T>(0);
        }

        /// <summary>
        /// Find the last component of a type.
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

            return default(T);
        }

        /// <summary>
        /// Find the nth ocurrence of a component of a type.
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
        /// Find the first component of a type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <returns>Index of the component if it exits; otherwise, less than zero.</returns>
        private int ImplFindFirstIndex(Type type) => ImplFindIndex(type, 0);

        /// <summary>
        /// Find the last component of a type.
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
        /// Add a new component to the list.
        /// </summary>
        /// <typeparam name="T">Type of component to add.</typeparam>
        public void Add<T>() where T : IGameComponent, new()
        {
            Add(new T());
        }

        /// <summary>
        /// Add a component to the list.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public void Add(IGameComponent component)
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

            if (prev != null)
            {
                prev.LinkNext(cnew);
            }

            cnew.LinkPrev(prev);
            cnew.LinkNext(null);

            m_list.Add(cnew);
        }

        /// <summary>
        /// Remove the nth ocurrence of a component.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        public void Remove<T>(int nth)
        {
            int i  = ImplFindIndex(typeof(T), nth);
            if (i >= 0)
            {
                ImplRemove(i);
            }
        }

        /// <summary>
        /// Remove all components of a type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        public void RemoveAll<T>()
        {
            for (int i = 0; i < m_list.Count;)
            {
                if (m_list[i].GetType() == typeof(T))
                {
                    ImplRemove(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Implement the removal of a component.
        /// </summary>
        /// <param name="index">Index of the component to remove.</param>
        public void ImplRemove(int index)
        {
            IGameComponent component = m_list[index];

            if (component.Prev != null) { component.Prev.LinkNext(component.Next); }
            if (component.Next != null) { component.Next.LinkPrev(component.Prev); }

            m_list.RemoveAt(index);
        }
    }
}
