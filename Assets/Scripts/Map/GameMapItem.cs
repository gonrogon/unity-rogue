using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Map
{
    public struct GameMapItem
    {
        [Flags]
        public enum Flags
        {
            None      = 0,
            Multicell = 1,
            Movable   = 2
        }

        public Flags flags;

        public byte width;

        public byte height;

        public byte ox;

        public byte oy;

        public static GameMapItem CreateMovable()
        {
            GameMapItem item = new GameMapItem();
            item.flags = Flags.Movable;

            return item;
        }

        public bool IsMulticell => (flags & Flags.Multicell) != Flags.None;

        public bool IsMovable => (flags & Flags.Movable) != Flags.None;
    }
}
