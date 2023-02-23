using Rogue.Core;
using GG.Mathe;

namespace Rogue.Map
{
    public interface IMulticell
    {
        Vec2i GetOrigin();

        bool ContainsCoord(Ident eid, Vec2i coord);
    }
}
