using System.Collections.Generic;
using System;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a database for the templates.
    /// </summary>
    public class TemplateDatabase
    {
        /// <summary>
        /// Dictionary with the templates.
        /// </summary>
        private readonly Dictionary<string, Template> m_templates = new ();

        /// <summary>
        /// Gets a list with the names of all loaded templates.
        /// </summary>
        /// <returns>List.</returns>
        public List<string> GetTemplateNames() => new (m_templates.Keys);

        /// <summary>
        /// Finds a template.
        /// </summary>
        /// <param name="name">Name of the template.</param>
        /// <returns>Reference to the template if it exists; otherwise, null.</returns>
        public Template Find(string name) => m_templates.TryGetValue(name, out var template) ? template : null;

        /// <summary>
        /// Tries to get a template.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="template">Template.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool TryGet(string name, out Template template) => m_templates.TryGetValue(name, out template);

        /// <summary>
        /// Adds a template.
        /// </summary>
        /// <param name="template">Template to add.</param>
        public void Add(Template template)
        {
            if (template == null)
            {
                throw new System.ArgumentNullException("template", "template to add can not be null");
            }

            m_templates[template.Name] = template;
        }

        /// <summary>
        /// Load the database from a file.
        /// </summary>
        /// <param name="file">File to load.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool LoadFromFile(string file) => Serialization.TemplateSerializer.LoadDatabaseFromFile(file, this) != null;

        /// <summary>
        /// Loads the database from a text.
        /// </summary>
        /// <param name="text">Text to load.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool LoadFromText(string text) => Serialization.TemplateSerializer.LoadDatabaseFromText(text, this) != null;

        public int Compile(Action<Template> onCompiled = null)
        {
            int count = 0;

            foreach (var pair in m_templates)
            {
                if (pair.Value.CompileNew(this, onCompiled))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
