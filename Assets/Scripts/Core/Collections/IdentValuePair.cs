namespace Rogue.Core.Collections
{
    /// <summary>
    /// Defines a identifier/value pair.
    /// </summary>
    /// <typeparam name="T">Type for the values.</typeparam>
    public readonly struct IdentValuePair<T>
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public readonly Ident ident;

        /// <summary>
        /// Value.
        /// </summary>
        public readonly T value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ident">Identifier.</param>
        /// <param name="value">Value.</param>
        public IdentValuePair(Ident ident, T value)
        {
            this.ident = ident;
            this.value = value;
        }
    }
}
