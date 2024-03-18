using System;
using Newtonsoft.Json;

namespace Rogue.Game
{
    /// <summary>
    /// Defines a item type.
    /// </summary>
    [JsonConverter(typeof(ItemTypeConverter))]
    public struct ItemType : IEquatable<ItemType>
    {
        public static readonly ItemType None = new (-1);

        /// <summary>
        /// Value.
        /// </summary>
        private readonly int m_value;

        /// <summary>
        /// Flag indicating whether it is a valid item type or not.
        /// </summary>
        public bool Valid => m_value >= 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value.</param>
        public ItemType(int value)
        {
            m_value = value;
        }

        public override int GetHashCode() => m_value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is not ItemType other)
            {
                return false;
            }

            return (this == other);
        }

        public bool Equals(ItemType other) => (this == other);

        public static bool operator==(ItemType lhs, ItemType rhs) => lhs.m_value == rhs.m_value;

        public static bool operator!=(ItemType lhs, ItemType rhs) => lhs.m_value != rhs.m_value;

        public override string ToString()
        {
            if (Context.ItemTypes != null)
            {
                return Context.ItemTypes.GetName(this);
            }

            return "unregisterd_type";
        }
    }
}
