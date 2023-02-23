using UnityEngine;
using System.Collections.Generic;

namespace Rogue.Views
{
    public class ViewFactory : Coe.IGameViewFactory
    {
        private readonly List<ViewTable> m_tables = new();

        private ViewManager m_manager;

        public ViewFactory(ViewManager manager)
        {
            m_manager = manager;
        }

        public ViewFactory(ViewManager manager, IEnumerable<ViewTable> tables)
        {
            m_manager = manager;
            m_tables.AddRange(tables);
        }

        public void AddTable(ViewTable table)
        {
            m_tables.Add(table);
        }

        public Coe.IGameView Create(string type, string name)
        {
            foreach (ViewTable table in m_tables)
            {
                var view = table.Create(type, name);
                if (view != null)
                {
                    view.SetupView(m_manager);

                    return view;
                }
            }

            return null;
        }
    }
}
