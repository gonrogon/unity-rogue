using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Name : GameComponent<Name>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string name;

        /// <summary>
        /// Description.
        /// </summary>
        public string description;

        public Name() {}

        public Name(string name, string description)
        {
            this.name        = name;
            this.description = description;
        }
    }
}
