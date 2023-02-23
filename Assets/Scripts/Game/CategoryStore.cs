using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Game
{
    /// <summary>
    /// Defines a store to manage the item categories.
    /// </summary>
    public class CategoryStore
    {
        /// <summary>
        /// Dictionary with all the categories.
        /// </summary>
        private readonly Dictionary<CategoryId, Category> m_categories = new();

        /// <summary>
        /// List of categories at the root level.
        /// </summary>
        private readonly List<Category> m_root = new();

        /// <summary>
        /// Number of categories at root level.
        /// </summary>
        public int Count => m_root.Count;

        /// <summary>
        /// Gets a category at the root level by its index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Category.</returns>
        public Category Get(int index) => m_root[index];

        /// <summary>
        /// Finds a category by its name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Reference to the category it is found; otherwise, null.</returns>
        public Category Find(string name) => m_root.Find(category => { return category.Name == name; });

        /// <summary>
        /// Finds a category at any level by its identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Reference to the category if it exists; otherwise, null.</returns>
        public Category FindAny(CategoryId id)
        {
            if (!m_categories.TryGetValue(id, out Category category))
            {
                return null;
            }

            return category;
        }

        /// <summary>
        /// Finds a category at any level by its path.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Reference to the category if it exists; otherwise, null.</returns>
        public Category FindAny(string path)
        {
            string[] chunks = path.Split('/');
            Category cursor = null;

            for (int i = 0; i < chunks.Length; i++)
            {
                if (i == 0)
                {
                    cursor = Find(chunks[i]);
                }
                else
                {
                    cursor = cursor.Find(chunks[i]);

                    if (cursor == null)
                    {
                        break;
                    }
                }
            }

            return cursor;
        }

        /// <summary>
        /// Generates the path to a category.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>String with the path if the category exists; otherwise, null.</returns>
        public string GeneratePath(CategoryId id)
        {
            if (!m_categories.TryGetValue(id, out Category category))
            {
                return null;
            }
            
            StringBuilder builder = new();
            Category      cursor  = category;
            int           index   = 0;

            while (cursor != null)
            {
                if (index > 0)
                {
                    builder.Insert(0, '/');    
                }
                builder.Insert(0, cursor.Name);

                cursor = cursor.Parent;
                index++;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Adds a category by its category path.
        /// </summary>
        /// <param name="path">Category path.</param>
        public CategoryId Create(string path)
        {
            CategoryId lastId = new ();
            string[]   chunks = path.Split('/');

            for (int i = 0; i < chunks.Length; i++)
            {
                if (i == 0)
                {
                    lastId = AddImpl(chunks[i]);
                }
                else
                {
                    lastId = AddImpl(chunks[i], lastId);
                }
            }

            return lastId;
        }

        /// <summary>
        /// Adds a category at the root level.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Identifier.</returns>
        private CategoryId AddImpl(string name)
        {
            Category category = Find(name);
            if (category != null)
            {
                return category.Id;
            }

            var ncid = CategoryId.Create((uint)m_root.Count + 1);
            category = new(ncid, name);

            m_categories[ncid] = category;

                   m_root.Add(category);
            return m_root.Last().Id;
        }

        /// <summary>
        /// Adds a category as a subcategory of other category.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="parent">Identifier of the parent category.</param>
        /// <returns>Identifier.</returns>
        private CategoryId AddImpl(string name, CategoryId parent)
        {
            Category category         = m_categories[parent].AddImpl(name);
            m_categories[category.Id] = category;

            return category.Id;
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
                var path = (string)item;
                if (path == null)
                {
                    continue;
                }

                Create(path);
            }
        }
    }
}
