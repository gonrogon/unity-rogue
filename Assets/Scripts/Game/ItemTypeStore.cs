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
        /// <summary>
        /// Defines a structure to store the metadata about a type.
        /// </summary>
        public struct Meta
        {
            /// <summary>
            /// Unique string with the name of the type.
            /// </summary>
            public string name;

            /// <summary>
            /// Category identifier.
            /// </summary>
            public CategoryId category;
        }

        /// <summary>
        /// Dictionary with the pairs type/metadata.
        /// </summary>
        private readonly Dictionary<ItemType, Meta> m_types = new();

        /// <summary>
        /// Dictionary with the pairs name/type.
        /// </summary>
        private readonly Dictionary<string, ItemType> m_names = new();

        /// <summary>
        /// Identifier for the next type.
        /// </summary>
        private int m_nextTypeId = 0;

        /// <summary>
        /// Gets the type for a type name.
        /// </summary>
        /// <param name="name">Type name.</param>
        /// <returns>Type if the type name exists; otherwise, none.</returns>
        public ItemType GetType(string name)
        {
            if (!m_names.TryGetValue(name, out ItemType type))
            {
                return ItemType.None;
            }

            return type;
        }

        /// <summary>
        /// Gets the name of a item type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Name if the type exists; otherwise, null.</returns>
        public string GetName(ItemType type)
        {
            if (!m_types.TryGetValue(type, out Meta meta))
            {
                return null;
            }

            return meta.name;
        }

        /// <summary>
        /// Gets the category of a type.
        /// </summary>
        /// <param name="name">Type.</param>
        /// <returns>Category identifier it the type exists; otherwise, none.</returns>
        public CategoryId GetCategory(string name)
        {
            ItemType type = GetType(name);

            if (!type.Valid)
            {
                return CategoryId.None;
            }

            return GetCategory(type);
        }

        /// <summary>
        /// <summary>
        /// Gets the category of a type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Category identifier if the type exists; otherwise, none.</returns>
        public CategoryId GetCategory(ItemType type)
        {
            if (!m_types.TryGetValue(type, out Meta meta))
            {
                return CategoryId.None;
            }

            return meta.category;
        }

        /// <summary>
        /// Creates a new type.
        /// </summary>
        /// <param name="name">Name of the type.</param>
        /// <param name="category">Category identifier.</param>
        public void Create(string name, CategoryId category)
        {
            ItemType type = new (m_nextTypeId++);
            m_types[type] = new Meta{ name = name, category = category };
            m_names[name] = type;
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
