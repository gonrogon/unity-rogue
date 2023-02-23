using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class Blackboard
    {
        /// <summary>
        /// Dictionary with the vars.
        /// </summary>
        private readonly Dictionary<string, object> m_vars = new();

        /// <summary>
        /// Checks whether a variable is defined or not.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>True if the variable is defined; otherwise, false.</returns>
        public bool Contains(string name) => m_vars.ContainsKey(name);

        /// <summary>
        /// Gets a variable.
        /// </summary>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="name">Name.</param>
        /// <returns>Value.</returns>
        public T Get<T>(string name)
        {
            if (!m_vars.TryGetValue(name, out object obj) || obj is not T value)
            {
                return default;
            }

            return value;
        }

        /// <summary>
        /// Gets a variable or default value if it does not exists.
        /// </summary>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="name">Name.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value.</returns>
        public T GetOrDefault<T>(string name, T defaultValue)
        {
            if (!m_vars.TryGetValue(name, out object obj) || obj is not T value)
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Gets a variable.
        /// </summary>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <returns>True if the variable is defined and it has the right type; otherwise, false.</returns>
        public bool TryGet<T>(string name, out T value)
        {
            if (!m_vars.TryGetValue(name, out object obj) || obj is not T v)
            {
                value = default;
                return false;
            }

            value = v;
            return true;
        }

        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <remarks>
        /// Overrides any previous value.
        /// </remarks>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public void Set<T>(string name, T value)
        {
            m_vars[name] = value;
        }

        /// <summary>
        /// Removes a variable.
        /// </summary>
        /// <param name="name">Name.</param>
        public void Remove(string name)
        {
            m_vars.Remove(name);
        }
    }
}
