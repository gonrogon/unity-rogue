using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rogue.Game
{
    /// <summary>
    /// Defines a store to manage the item categories.
    /// </summary>
    public class ItemTypeStore
    {
        public struct Note
        {
            /// <summary>
            /// Unique string identifier of the type.
            /// </summary>
            public string name;

            /// <summary>
            /// Category.
            /// </summary>
            public CategoryId category;
        }

        /// <summary>
        /// Dictionary with the notes about the types.
        /// </summary>
        private readonly Dictionary<ItemType, Note> m_types = new();

        /// <summary>
        /// Dictionary with the pairs name/type.
        /// </summary>
        private readonly Dictionary<string, ItemType> m_names = new();

        private int m_nextTypeId = 0;

        public void Create(string name, CategoryId category)
        {
            ItemType type = new (m_nextTypeId++);
            m_types[type] = new Note{ name = name, category = category };
            m_names[name] = type;
        }

        public ItemType Find(string name)
        {
            if (!m_names.TryGetValue(name, out ItemType type))
            {
                return ItemType.None;
            }

            return type;
        }

        public string GenerateName(ItemType type)
        {
            if (!m_types.TryGetValue(type, out Note note))
            {
                return null;
            }

            return note.name;
        }

        public void LoadFromText(string text)
        {
            using var stream = new StringReader(text);

            Load(stream);
        }

        public void LoadFromFile(string file)
        {
            using var stream = new StreamReader(file);
            
            Load(stream);
        }

        private void Load(TextReader stream)
        {
            using var reader = new JsonTextReader(stream);

            var serializer = new JsonSerializer();
            var jarray     = serializer.Deserialize<JArray>(reader);

            foreach (JToken item in jarray)
            {
                if (item is not JArray jsub)
                {
                    continue;
                }

                string name = (string)jsub[0];
                string path = (string)jsub[1];
                if (name == null || path == null)
                {
                    continue;
                }

                Category category = Context.Categories.FindAny(path);
                if (category == null)
                {
                    Debug.LogWarning($"Unable to load item type \"{name}\", category \"{path}\" not found");
                    continue;
                }

                Create(name, category.Id);
            }
        }
    }
}
