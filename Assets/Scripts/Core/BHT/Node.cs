using System.Collections.Generic;

namespace Rogue.Core.BHT
{
    public enum NodeState
    {
        Failure, Running, Success
    }

    public abstract class Node
    {
        /// <summary>
        /// Root node.
        /// </summary>
        protected Node m_root = null;

        /// <summary>
        /// Parent node.
        /// </summary>
        protected Node m_parent = null;

        /// <summary>
        /// Dictionary with the vars.
        /// </summary>
        private Dictionary<string, object> m_vars = null;

        /*
        public Node GetRoot()
        {
            Node root = this;

            while (root.m_parent != null)
            {
                root = root.m_parent;
            }

            return root;
        }
        */

        public Node GetRoot()
        {
            if (m_root != null)
            {
                return m_root;
            }

            return this;
        }

        #region @@@ VARS @@@

        public object FindVar(string name)
        {
            for (Node node = this; node != null; node = node.m_parent)
            {
                var obj  = node.GetNodeVar(name);
                if (obj != null)
                {
                    return obj;
                }
            }

            return null;
        }

        public bool TryFindVar(string name, out object var)
        {
            var = default;

            for (Node node = this; node != null; node = node.m_parent)
            {
                var = node.GetNodeVar(name);
                if (var != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryFindVar<T>(string name, out T var)
        {
            var = default;

            for (Node node = this; node != null; node = node.m_parent)
            {
                if (node.GetNodeVar(name) is T t)
                {
                    var = t;

                    return true;
                }
            }

            return false;
        }

        public bool HasGlobalVar(string name) => GetRoot().HasNodeVar(name);

        public object GetGlobalVar(string name) => GetRoot().GetNodeVar(name);

        public void SetGlobalVar(string name, object value) => GetRoot().SetNodeVar(name, value);

        public virtual bool HasVar(string name)
        {
            if (m_parent != null)
            {
                return m_parent.HasNodeVar(name);
            }

            return HasNodeVar(name);
        }

        public virtual object GetVar(string name)
        {
            if (m_parent != null)
            {
                return m_parent.GetNodeVar(name);
            }

            return GetNodeVar(name);
        }

        public virtual void SetVar(string name, object value)
        {
            if (m_parent != null)
            {
                m_parent.SetNodeVar(name, value);
                return;
            }

            SetNodeVar(name, value);
        }

        public virtual void DelVar(string name)
        {
            if (m_parent != null)
            {
                m_parent.DelNodeVar(name);
                return;
            }

            DelNodeVar(name);
        }

        protected virtual bool HasNodeVar(string name)
        {
            if (m_vars == null)
            {
                return false;
            }

            return m_vars.ContainsKey(name);
        }

        protected virtual object GetNodeVar(string name)
        {
            if (m_vars == null)
            {
                return null;
            }

            if (!m_vars.TryGetValue(name, out object result))
            {
                return null;
            }

            return result;
        }

        protected virtual void SetNodeVar(string name, object value)
        {
            if (m_vars == null)
            {
                m_vars = new Dictionary<string, object>();
            }

            m_vars[name] = value;
        }

        protected virtual void DelNodeVar(string name)
        {
            if (m_vars == null)
            {
                return;
            }

            m_vars.Remove(name);
        }

        #endregion

        #region @@@ LIFE CYCLE @@@

        public virtual void Reset() {}

        public virtual NodeState Evaluate() => NodeState.Success;

        #endregion

        #region @@@ EVENTS @@@

        /// <summary>
        /// Notifies the node that it has been attached to another node.
        /// </summary>
        /// <param name="parent"></param>
        public virtual void OnAttached(Node parent)
        {
            m_parent = parent;
            m_root   = parent.GetRoot();
        }

        #endregion
    }
}
