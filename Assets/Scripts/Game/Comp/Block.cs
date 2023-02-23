using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Block : GameComponent<Block>
    {
        /// <summary>
        /// Blocked tags.
        /// </summary>
        public TagType tags = TagType.None;

        /// <summary>
        /// Flag indicating whether the block is enabled or not.
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Block() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tags">Tags to block.</param>
        public Block(TagType tags) : this(tags, true) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tags">Tags to block.</param>
        /// <param name="enabled">True if the block is enabled; otherwise, false.</param>
        public Block(TagType tags, bool enabled)
        {
            this.tags    = tags;
            this.enabled = enabled;
        }

        /// <summary>
        /// Checks if a list of tags is blocked.
        /// 
        /// A list of tags is blocked if one its tags if in the list of blocked tags.
        /// </summary>
        /// <param name="tags">Tags.</param>
        /// <returns>True if there is at least one tag blocked; otherwise, false.</returns>
        public bool Blocked(TagType tags)
        {
            return (this.tags & tags) != TagType.None;
        }

        /// <summary>
        /// Adds tags to the list of blocked tags.
        /// </summary>
        /// <param name="tags">Tags to block.</param>
        public void Add(TagType tags)
        {
            this.tags |= tags;
        }

        /// <summary>
        /// Removes tags from the list of blocked tags.
        /// </summary>
        /// <param name="tags">Tags to remove.</param>
        public void Remove(TagType tags)
        {
            this.tags ^= tags;
        }
    }
}
