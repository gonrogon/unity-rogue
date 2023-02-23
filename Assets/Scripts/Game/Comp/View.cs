using UnityEngine;
using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class View : GameComponent<View>
    {
        public string type;

        public string name;

        public string sprite;

        public bool mapOnly = true;

        public View() {}

        public View(string sprite)
            :
            this(null, null, sprite)
        {}

        public View(string type, string name, string sprite, bool mapOnly = true)
        {
            this.type    = type;
            this.name    = name;
            this.sprite  = sprite;
            this.mapOnly = mapOnly;
        }
    }
}
