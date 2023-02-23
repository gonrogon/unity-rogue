using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Rogue.Game
{
    public class Category
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        private readonly CategoryId m_id;

        /// <summary>
        /// Name.
        /// </summary>
        private readonly string m_name;

        /// <summary>
        /// Parent category.
        /// </summary>
        private readonly Category m_parent;

        /// <summary>
        /// Child categories.
        /// </summary>
        private List<Category> m_children;

        /// <summary>
        /// Gets the category id.
        /// </summary>
        public CategoryId Id => m_id;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// Gets the parent category.
        /// </summary>
        public Category Parent => m_parent;

        /// <summary>
        /// Number of subcategories.
        /// </summary>
        public int Count => m_children == null ? 0 : m_children.Count;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="parent">Parent category.</param>
        public Category(CategoryId id, string name, Category parent)
        {
            m_id     = id;
            m_name   = name;
            m_parent = parent;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        public Category(CategoryId id, string name)
        {
            m_id     = id;
            m_name   = name;
            m_parent = null;
        }

        /// <summary>
        /// Gets a subcategory by its index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Category.</returns>
        public Category Get(int index) => m_children[index];

        /// <summary>
        /// Finds a subcategory.
        /// </summary>
        /// <param name="name">Name of the subcategory.</param>
        /// <returns>Reference to the subcategory if it is found; otherwise, null.</returns>
        public Category Find(string name)
        {
            if (m_children == null)
            {
                return null;
            }

            return m_children.Find(c => { return c.m_name == name; });
        }

        /// <summary>
        /// Adds a subcategory.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Added category.</returns>
        public Category AddImpl(string name)
        {
            if (m_children == null)
            {
                m_children = new();
            }

                   m_children.Add(new Category(m_id.CreateSubcategory((uint)m_children.Count), name, this));
            return m_children.Last();
        }
    }
}
