using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rogue.Core
{
    /// <summary>
    /// Defines an identifier.
    /// </summary>
    public struct Ident : IEquatable<Ident>
    {
        /// <summary>
        /// Number of bits used in the value area.
        /// </summary>
        public const int ValBits = 24;

        /// <summary>
        /// Number of bits used in the generation area.
        /// </summary>
        public const int GenBits =  8;

        /// <summary>
        /// Mask for the value area.
        /// </summary>
        public const uint ValMask = 0xffffff00;

        /// <summary>
        /// Mask for the generation area.
        /// </summary>
        public const uint GenMask = 0x000000ff;

        /// <summary>
        /// Maximum value for the value area.
        /// </summary>
        public const uint MaxVal  = 0x00ffffff;

        /// <summary>
        /// Maximum value for the generation area.
        /// </summary>
        public const uint MaxGen  = 0x000000ff;

        /// <summary>
        /// Empty identifier.
        /// </summary>
        public static readonly Ident Zero = new ();

        /// <summary>
        /// Internal identifier (generation + value).
        /// </summary>
        private uint m_id;

        /// <summary>
        /// Get the raw identifier.
        /// </summary>
        public uint Raw => m_id;

        /// <summary>
        /// Get the value.
        /// </summary>
        public uint Value => m_id >> GenBits;

        /// <summary>
        /// Get the values as an array index.
        /// </summary>
        public int ValueAsIndex => (int)(m_id >> GenBits);

        /// <summary>
        /// Get the generation.
        /// </summary>
        public uint Generation => m_id & GenMask;

        /// <summary>
        /// Flag indicating if the identifier is zero or not.
        /// </summary>
        public bool IsZero => m_id == 0;

        /// <summary>
        /// Constructor.
        /// 
        /// Create an identifier with value and generation zero.
        /// </summary>
        /// <param name="value">Value.</param>
        public Ident(uint value) : this(value, 0) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="generation">Generation.</param>
        public Ident(uint value, uint generation)
        {
            m_id = ((value << GenBits) | generation);

            Debug.Assert(value      < MaxVal, "Invalid identifier value");
            Debug.Assert(generation < MaxGen, "Invalid identifier generation");
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="ident">Identifier to copy.</param>
        public Ident(Ident ident) => m_id = ident.m_id;

        /// <summary>
        /// Creates an identifier from a raw value.
        /// </summary>
        /// <param name="raw">Raw value.</param>
        /// <returns>Identifier.</returns>
        public static Ident CreateFromRaw(uint raw)
        {
            Ident id;
            id.m_id = raw;

            return id;
        }

        /// <summary>
        /// Generates a new identifier with the same generation and the next value.
        /// </summary>
        /// <returns>Identifier.</returns>
        public Ident NextValue() => new (Value + 1, Generation);

        /// <summary>
        /// Generates a new identifier with the next generation and the same value.
        /// 
        /// Note when the generation exceeds its maximum value its restarts from zero.
        /// </summary>
        /// <returns>Identifier.</returns>
        public Ident NextGeneration() => new (Value, Generation >= MaxGen ? 0 : Generation + 1);

        /// <summary>
        /// Generates a new identifier with the next generation and the same value.
        /// 
        /// Note that this method avoids using zero as generation, so when the generation exceeds its maximum value
        /// its restarts from one.
        /// </summary>
        /// <returns>Identifier.</returns>
        public Ident NextGenerationSkipZero() => new (Value, Generation >= MaxGen ? 1 : Generation + 1);

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode() => -1269635712 + m_id.GetHashCode();

        /// <summary>
        /// Checks if two identifiers are equal.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>True if they are equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is not Ident)
            {
                return false;
            }

            return Equals((Ident)obj);
        }

        /// <summary>
        /// Checks if two identifiers are equal.
        /// </summary>
        /// <param name="ident">Identifier.</param>
        /// <returns>True if they are equal; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ident ident) => m_id == ident.m_id;

        /// <summary>
        /// Checks if two identifiers are equal.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator==(Ident lhs, Ident rhs) => lhs.m_id == rhs.m_id;

        /// <summary>
        /// Checks if two identifiers are not equal.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!=(Ident lhs, Ident rhs) => lhs.m_id != rhs.m_id;

        /// <summary>
        /// Checks if a identifier is less than or equal to another.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator<=(Ident lhs, Ident rhs) => lhs.m_id <= rhs.m_id;

        /// <summary>
        /// Checks if a identifier is greater than or equal to another.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator>=(Ident lhs, Ident rhs) => lhs.m_id >= rhs.m_id;

        /// <summary>
        /// Checks if a identifier is less than another.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator<(Ident lhs, Ident rhs) => lhs.m_id < rhs.m_id;

        /// <summary>
        /// Checks if a identifier is greater than another.
        /// </summary>
        /// <param name="lhs">Identifier.</param>
        /// <param name="rhs">Identifier.</param>
        /// <returns>Result of the comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator>(Ident lhs, Ident rhs) => lhs.m_id > rhs.m_id;

        /// <summary>
        /// Get a nicely formatted string for the identifier.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString() => $"({Generation}, {Value})";
    }
}
