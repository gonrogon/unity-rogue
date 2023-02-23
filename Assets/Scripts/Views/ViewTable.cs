using UnityEngine;

namespace Rogue.Views
{
    [CreateAssetMenu(menuName = "Views/View Table")]
    public class ViewTable : ScriptableObject
    {
        /// <summary>
        /// List of views.
        /// </summary>
        [SerializeField]
        private ViewBase[] m_views;

        /// <summary>
        /// Checks if the table constains a view.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains(string type) => FindView(type, null) != null;
        
        /// <summary>
        /// Checks if the table constains a view.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <param name="name">Name.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains(string type, string name) => FindView(type, name) != null;

        /// <summary>
        /// Create a new instance of a view by its type.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <returns>Reference to the new instance if it exists; otherwise, null.</returns>
        public ViewBase Create(string type)
        {
            ViewBase view = FindView(type, null);
            if (view != null)
            {
                return Instantiate(view);
            }

            return null;
        }

        /// <summary>
        /// Create a new instance of a view by its type and name.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <param name="name">Name.</param>
        /// <returns>Reference to the new instance if it exists; otherwise, null.</returns>
        public ViewBase Create(string type, string name)
        {
            ViewBase view = FindView(type, name);
            if (view != null)
            {
                return Instantiate(view);
            }

            return null;
        }

        /// <summary>
        /// Finds a view by its type and name.
        /// </summary>
        /// <param name="type">Type of view.</param>
        /// <param name="name">Name.</param>
        /// <returns>Reference to the view if it exists; otherwise, null.</returns>
        private ViewBase FindView(string type, string name)
        {
            foreach (var view in m_views)
            {
                if (view.GetType().Name == type)
                {
                    if (string.IsNullOrEmpty(name) || (!string.IsNullOrEmpty(name) && view.name == name))
                    {
                        return view;
                    }
                }
            }

            return null;
        }
    }
}
