using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Core.BHT
{
    public class Tree
    {
        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        private Node m_root = null;

        private Node m_active = null;

        public virtual void Evaluate() {}
    }
}
