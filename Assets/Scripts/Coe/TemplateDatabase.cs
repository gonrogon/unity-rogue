using System.Collections.Generic;

namespace Rogue.Coe
{
    public class TemplateDatabase
    {
        private readonly Dictionary<string, Template> m_templates = new ();

        /// <summary>
        /// Gets a list with the names of all the templates.
        /// </summary>
        /// <returns>List.</returns>
        public List<string> GetTemplateNames() => new (m_templates.Keys);

        public Template Find(string name)
        {
            if (m_templates.TryGetValue(name, out Template template))
            {
                return template;
            }

            return null;
        }

        public bool TryGet(string name, out Template template)
        {
            return m_templates.TryGetValue(name, out template);
        }

        public bool Add(Template template)
        {
            if (template == null)
            {
                return false;
            }

            m_templates[template.Name] = template;

            return true;
        }
        /*
        public bool Load(string file)
        {
            Template template = TemplateSerializer.LoadFromFile(file);

            if (template == null)
            {
                Debug.LogError($"Unable to load template from \"{file}\"");
                return false;
            }

            if (m_templates.ContainsKey(template.Name))
            {
                Debug.LogWarning($"Unable to load template, template name \"{template.Name}\" already exists");
                return false;
            }

            if (template != null)
            {
                m_templates.Add(template.Name, template);
            }

            return true;
        }
        */
        public bool LoadFromText(string text)
        {
            if (Serialization.TemplateSerializer.LoadDatabaseFromText(text, this) == null)
            {
                return false;
            }

            return true;
        }

        public void Save(string file)
        {
            Serialization.TemplateSerializer.SaveDatabaseToFile(file, this);
        }

        public void Compile()
        {
            foreach (var pair in m_templates)
            {
                pair.Value.Compile(this);
            }
        }
    }
}
