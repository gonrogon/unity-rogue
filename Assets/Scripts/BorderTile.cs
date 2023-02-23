using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GG.Mathe;

namespace Rogue 
{
    [CreateAssetMenu(menuName = "Map/Border Tile")]
    public class BorderTile : TileBase
    {
        [HideInInspector]
        public Map.GameMap map = null;

        public Color color = Color.white;

        public Sprite tileHorizontalEnd = null;

        public Sprite tileVerticalEnd = null;

        public Sprite tileInternalCorner = null;

        public Sprite tileExternalCorner = null;

        public Sprite tileVertical = null;

        public Sprite tileHorizontal = null;

        public Sprite tileDebug = null;

        public bool debug = false;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Vector3Int neighbour = position + new Vector3Int(x, y);
                    // Check if the neighbour tile is a border.
                    if (tilemap.GetTile(position) == this)
                    {
                        tilemap.RefreshTile(neighbour);
                    }
                }
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.color        = color;
            tileData.transform    = Matrix4x4.identity;
            tileData.flags        = TileFlags.LockTransform;
            tileData.colliderType = Tile.ColliderType.None;
            tileData.sprite       = tileDebug;

            if (debug)
            {
                tileData.sprite = tileDebug;
                return;
            }

            var p = new Vec2i(position.x, position.y);
            if (CheckRuleInternalCorner(map, p)) { tileData.sprite = tileInternalCorner; return; }
            if (CheckRuleExternalCorner(map, p)) { tileData.sprite = tileExternalCorner; return; }
            if (CheckRuleHorizontalEnd (map, p)) { tileData.sprite = tileHorizontalEnd; return; }
            if (CheckRuleVerticalEnd   (map, p)) { tileData.sprite = tileVerticalEnd; return; }
            if (CheckRuleHorizontal    (map, p)) { tileData.sprite = tileHorizontal; return; }
            if (CheckRuleVertical      (map, p)) { tileData.sprite = tileVertical; return; }
        }

        public static bool NeedBorderTile(Map.GameMap map, Vec2i position)
        {
            if (CheckRuleInternalCorner(map, position)) { return true; }
            if (CheckRuleExternalCorner(map, position)) { return true; }
            if (CheckRuleHorizontalEnd (map, position)) { return true; }
            if (CheckRuleVerticalEnd   (map, position)) { return true; }
            if (CheckRuleHorizontal    (map, position)) { return true; }
            if (CheckRuleVertical      (map, position)) { return true; }

            return false;
        }

        private static bool CheckRuleInternalCorner(Map.GameMap map, Vec2i position)
        {
            return HasWall(map, position, 1, 0) && HasWall(map, position, 0, -1);
        }

        private static bool CheckRuleExternalCorner(Map.GameMap map, Vec2i position)
        {
            return HasWall(map, position, 1, -1) && !HasWall(map, position, 1, 0) && !HasWall(map, position, 0, -1);
        }

        private static bool CheckRuleHorizontalEnd(Map.GameMap map, Vec2i position)
        {
            return HasWall(map, position, -1, -1) && !HasWall(map, position, -1, 0) && !HasWall(map, position, 0, -1);
        }

        private static bool CheckRuleVerticalEnd(Map.GameMap map, Vec2i position)
        {
            return HasWall(map, position, 1, 1) && !HasWall(map, position, 1, 0) && !HasWall(map, position, 0, 1);
        }

        private static bool CheckRuleHorizontal(Map.GameMap map, Vec2i position)
        {
            return HasWall(map, position, 0, -1);
        }

        private static bool CheckRuleVertical(Map.GameMap map, Vec2i position)
        { 
            return HasWall(map, position, 1, 0);
        }

        private static bool HasWall(Map.GameMap map, Vec2i position, int xOffset, int yOffset)
        {
            var pos = new Vec2i(position.x + xOffset, position.y + yOffset);

            if (!map.HasCoord(pos))
            {
                return false;
            }

            return map.HasWall(pos);
        }
    }
}
