using System;
using System.Reflection;
using System.Collections.Generic;

namespace Rogue.Coe
{
    public static class GameViewUtil
    {
        /// <summary>
        /// List of searchers.
        /// </summary>
        private static readonly List<IGameViewFactory> m_factories = new ();

        /// <summary>
        /// Adds a view factory.
        /// </summary>
        /// <param name="factory"></param>
        public static void AddFactory(IGameViewFactory factory) => m_factories.Add(factory);

        /// <summary>
        /// Creates a view from it type and name.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <param name="name">Name.</param>
        /// <returns>Reference to the view if it exists; otherwise, null.</returns>
        public static IGameView Create(string type, string name)
        {
            foreach (IGameViewFactory factory in m_factories)
            {
                IGameView view = factory.Create(type, name);
                if (view != null)
                {
                    return view;
                }
            }
            // Views have to be created using factories because views could be MonoBehaviours. MonoBehaviour have to
            // be created using the Instance() methods of Unity.
            return null;
        }
    }
}
