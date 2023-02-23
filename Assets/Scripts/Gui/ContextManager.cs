using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rogue.Gui
{
    /// <summary>
    /// Defines a manager for UI contexts.
    /// </summary>
    public class ContextManager
    {
        /// <summary>
        /// Defines a context activation.
        /// </summary>
        private struct ContextActivation
        {
            /// <summary>
            /// Context that was activated.
            /// </summary>
            public Context context;

            /// <summary>
            /// Settings for the context.
            /// </summary>
            public ContextSettings settings;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="context">Context that was activated.</param>
            public ContextActivation(Context context) : this(context, new ContextSettings()) {}

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="context">Context that was activated.</param>
            /// <param name="settings">Settings for the context.</param>
            public ContextActivation(Context context, ContextSettings settings)
            {
                this.context  = context;
                this.settings = settings;
            }
        }

        /// <summary>
        /// Dictionary with the contexts.
        /// </summary>
        private Dictionary<string, Context> m_contexts = new Dictionary<string, Context>();

        /// <summary>
        /// List of active contexts.
        /// </summary>
        private List<ContextActivation> m_active = new List<ContextActivation>();

        /// <summary>
        /// Queue of action to execute.
        /// </summary>
        private Queue<string> m_actions = new Queue<string>();

        /// <summary>
        /// Return the active context if there is one; otherwise, return null.
        /// </summary>
        public Context ActiveContext
        {
            get
            {
                if (m_active.Count <= 0)
                {
                    return null;
                }

                return m_active[m_active.Count - 1].context;
            }
        }

        /// <summary>
        /// Return the settings of the active context if there is one; otherwise, empty settings.
        /// </summary>
        public ContextSettings ActiveSettings
        {
            get
            {
                if (m_active.Count > 0)
                {
                    return m_active[m_active.Count - 1].settings;
                }

                return ContextSettings.Empty;
            }
        }

        public void Enqueue(string action)
        {
            m_actions.Enqueue(action);
        }

        public Context Find(string name)
        {
            Context context;

            if (m_contexts.TryGetValue(name, out context))
            {
                return context;
            }

            return null;
        }

        public void Add(string name, Context context)
        {
            if (m_contexts.ContainsKey(name))
            {
                return;
            }

            if (!context.Setup(this))
            {
                return;
            }

            m_contexts.Add(name, context);
        }

        public void Push(string name)
        {
            Context context;

            if (!m_contexts.TryGetValue(name, out context))
            {
                return;
            }

            m_active.Add(new ContextActivation(context));
            context.Start();
        }

        public void Push(string name, ContextSettings settings)
        {
            Context context;

            if (!m_contexts.TryGetValue(name, out context))
            {
                return;
            }

            m_active.Add(new ContextActivation(context, settings));
            context.Start();
        }

        public void Pop()
        {
            if (m_active.Count <= 0)
            {
                return;
            }

            ContextActivation active = m_active[m_active.Count - 1];

            active.context.Finish();
            //active.settings.end?.Invoke(active.context);

            m_active.RemoveAt(m_active.Count -1);
        }

        public void Update(float time)
        {
            Context current = ActiveContext;

            if (current == null)
            {
                m_actions.Clear();

                return;
            }

            while (m_actions.Count > 0)
            {
                current.Action(m_actions.Dequeue());
            }

            bool focus = false;

            for (int i = m_active.Count - 1; i >= 0; i--)
            {
                if (focus)
                {
                    m_active[i].context.Focus();

                    focus = false;
                }

                if (m_active[i].context.Update(time))
                {
                    for (int j = i + 1; j < m_active.Count; j++)
                    {
                        ContextActivation temp;

                        temp            = m_active[j - 1];
                        m_active[j - 1] = m_active[j];
                        m_active[j]     = temp;
                    }

                    Pop();

                    focus = true;
                }
            }
        }
    }
}
