using System;
using System.Collections.Generic;
using Rogue.Core;

namespace Rogue.Game.Stock
{
    public class StockpileFilter
    {
        private List<CategoryId> m_allowed = null;

        public void Allow(string path)
        {
            var category = Context.Categories.Find(path);
            if (category == null)
            {
                return;
            }

            if (m_allowed == null)
            {
                m_allowed = new();
            }

            m_allowed.Add(category.Id);
        }

        public void ClearAllow()
        {
            m_allowed = null;
        }

        public bool Accept(Ident eid)
        {
            var type = Context.World.Find(eid).FindFirstAny<Comp.ItemDecl>();
            // Only items can be store in the stockpiles.
            if (type == null)
            {
                return false;
            }
            // Check the allowed categories.
            if (m_allowed != null && m_allowed.Count > 0)
            {
                bool allowed = false;

                for (int i = 0; i < m_allowed.Count && !allowed; i++)
                {
                    if (m_allowed[i].Contains(type.category))
                    {
                        allowed = true;
                    }
                }

                if (!allowed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
