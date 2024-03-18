using System;
using Newtonsoft.Json;

namespace Rogue.Game
{
    /// <summary>
    /// Defines a category identifier.
    /// </summary>
    [JsonConverter(typeof(CategoryIdConverter))]
    public struct CategoryId : IEquatable<CategoryId>
    {
        /// <summary>
        /// Define a category id for no category.
        /// </summary>
        public static readonly CategoryId None = new (0, 0);

        public const int MinLevel = 0;

        public const int MaxLevel = 3;

        public const int MinValue = 0;

        public const int MaxValue = byte.MaxValue;

        /// <summary>
        /// Level.
        /// </summary>
        private readonly uint m_level;

        /// <summary>
        /// Value.
        /// </summary>
        private readonly uint m_value;

        /// <summary>
        /// Flag indicating whether the category allow subcategories or not.
        /// </summary>
        public bool AllowSubcategories => m_level < MaxLevel;

        /// <summary>
        /// Gets the level.
        /// </summary>
        public uint Level => m_level;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public uint Value => m_value & GetValueMask(m_level);

        /// <summary>
        /// Creates a category at the root level.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Category.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Value is out of the valid range.</exception>
        public static CategoryId Create(uint value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", value, "invalid value"); 
            }

            return new CategoryId(MinLevel, value);
        }

        /// <summary>
        /// Creates a category as a subcategory of other category.
        /// </summary>
        /// <param name="parent">Parent category.</param>
        /// <param name="value">Value.</param>
        /// <returns>Category.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Value is out of the valid range, parent is not valid or
        /// does not allow subcategories.</exception>
        public static CategoryId CreateSubcategory(uint value, CategoryId parent)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", value, "invalid value"); 
            }

            if (!parent.AllowSubcategories)
            {
                throw new ArgumentOutOfRangeException("parent", "parent does not allow subcategories");
            }

            uint level = parent.m_level + 1;

            return new CategoryId(level, value << (int)GetValueShift(level) | parent.m_value);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="value">Value</param>
        private CategoryId(uint level, uint value)
        {
            m_level = level;
            m_value = value;
        }

        /// <summary>
        /// Creates a subcategory.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Category.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Parent is at the maximum level.</exception>
        public CategoryId CreateSubcategory(uint value) => CreateSubcategory(value, this);

        /// <summary>
        /// Checks if a category is contained within other.
        /// </summary>
        /// <param name="other">Category.</param>
        /// <returns>True if the category is contained; otherwise, false.</returns>
        public bool Contains(CategoryId other)
        {
            // Invalid categories contains nothing.
            if (other.m_level < 0 || m_level < 0) { return false; }
            // If the other category is at a lower level, it cannot be contained within this one.
            if (other.m_level < m_level) { return false; }
            // If the other category is at the same level or at a greater one, it can only be contained if the
            // value part corresponding to this level has the same value.
            return (other.m_value & GetLevelMask(m_level)) == m_value;
        }

        #region @@@ EQUATABLE @@@

        public override int GetHashCode() => m_level.GetHashCode() ^ m_value.GetHashCode() << 2;

        public override bool Equals(object obj)
        {
            if (obj is not CategoryId other)
            {
                return false;
            }

            return (this == other);
        }

        public bool Equals(CategoryId other) => (this == other);

        public static bool operator==(CategoryId lhs, CategoryId rhs) => lhs.m_level == rhs.m_level && lhs.m_value == rhs.m_value;

        public static bool operator!=(CategoryId lhs, CategoryId rhs) => lhs.m_level != rhs.m_level || lhs.m_value != rhs.m_value;

        #endregion

        #region @@@ HELPERS @@@

        /// <summary>
        /// Calculates the bit mask to check if a category is contained inside another.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Mask.</returns>
        private static uint GetLevelMask(uint level)
        {
            return 0xffffffffu >> (int)(8u * (MaxLevel - level));
        }

        /// <summary>
        /// Calculates the bit mask to get the value of a category.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Mask.</returns>
        private static uint GetValueMask(uint level)
        {
            return 0x000000ffu << (int)level;
        }

        /// <summary>
        /// Calculates the shift to apply to encode the value of a category.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Shift.</returns>
        private static uint GetValueShift(uint level)
        {
            return 8u * level;
        }

        #endregion

        public override string ToString()
        {
            if (this == None)
            {
                return "none";
            }

            if (Context.Categories != null)
            {
                var path = Context.Categories.GeneratePath(this);
                if (path != null) 
                {
                    return path;
                }
            }

            return "unregistered_category";
        }
    }
}
