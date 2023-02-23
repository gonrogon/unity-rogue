using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Map.Data
{
    public abstract class TileGroup : ScriptableObject
    {
        public abstract int Count { get; }

        public abstract Tile GetTile(int i);

        public abstract int GetRandomIndex(System.Random random);

        public abstract Tile GetRandomTile(System.Random random);
    }
}
