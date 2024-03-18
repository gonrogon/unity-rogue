using System.Collections.Generic;
using System.IO;
using Rogue.Core.Collections;
using Rogue.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Rogue.Game
{
    public class BodySystem
    {
        private readonly Dictionary<string, Body> m_templates = new ();

        private readonly IdentMap<Body> m_bodies = new ();

        public void AddTemplate(string name, Body body)
        {
            if (m_templates.ContainsKey(name))
            { 
                Debug.LogWarning($"Body system already contains the template [{name}]");
                return;
            }

            m_templates.Add(name, body);
        }

        public void LoadTemplatesFromText(string text)
        {
            using StringReader stream = new (text);
            LoadTemplates(stream);
        }

        public void LoadTemplatesFromFile(string file)
        {
            using StreamReader stream = new (file);
            LoadTemplates(stream);
        }

        public void LoadTemplates(TextReader stream)
        {
                  JsonSerializer serializer = new ();
            using JsonTextReader reader     = new (stream);

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new Serialization.BodyConverter());
            serializer.Converters.Add(new Serialization.BodyMemberConverter());

            var dic = serializer.Deserialize<Dictionary<string, Body>>(reader);
            foreach (var pair in dic)
            {
                if (m_templates.ContainsKey(pair.Key))
                {
                    Debug.LogWarning($"Body system already contains the template [{pair.Key}]");
                    continue;
                }

                m_templates.Add(pair.Key, pair.Value);
            }
        }

        public void SaveTemplatesToFile(string file)
        {
            using StreamWriter stream = new (file);

                  JsonSerializer serializer = new ();
            using JsonTextWriter writer     = new (stream);

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting        = Formatting.Indented;
            serializer.Converters.Add(new Serialization.BodyConverter());
            serializer.Converters.Add(new Serialization.BodyMemberConverter());
            serializer.Serialize(writer, m_templates);
        }

        public Body Get(Ident bid)
        {
            return m_bodies.Get(bid);
        }

        public Ident Add()
        {
            return m_bodies.Add(new Body());
        }

        public Ident Add(string template)
        {
            if (!m_templates.TryGetValue(template, out Body body))
            {
                Debug.LogWarning($"Body template [{template}] not found");
                return Ident.Zero;
            }

            return m_bodies.Add(Body.CreateFromTemplate(body));
        }

        public Ident Add(Body body)
        {
            return m_bodies.Add(body);
        }

        public void Remove(Ident bid)
        {
            m_bodies.Release(bid);
        }
    }
}
