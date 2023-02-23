using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Map.Data
{
    /// <summary>
    /// Define a type of floor.
    /// </summary>
    [CreateAssetMenu(menuName = "Map/Tile/Variation")]
    public class TileVariation : TileGroup
    {
        [System.Serializable]
        public struct Item
        {
            public Tile tile;

            public int weight;
        }

        public Item[] items;

        public override int Count => items.Length;

        public override Tile GetTile(int i)
        {
            return items[i].tile;
        }

        public override int GetRandomIndex(System.Random random)
        {
            int totalWeight = 0;

            for (int i = 0; i < items.Length; i++)
            {
                totalWeight += items[i].weight;
            }
            
            int weight = random.Next(0, totalWeight);
            int accum  = 0;

            for (int i = 0; i < items.Length; i++)
            {
                if ((accum += items[i].weight) >= weight)
                {
                    return i;
                }
            }

            return items.Length - 1;
        }

        public override Tile GetRandomTile(System.Random random)
        {
            return items[GetRandomIndex(random)].tile;
        }
    }
}
