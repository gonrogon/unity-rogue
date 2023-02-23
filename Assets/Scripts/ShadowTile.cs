using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GG.Mathe;

namespace Rogue 
{
    [CreateAssetMenu(menuName = "Map/Shadow Tile")]
    public class ShadowTile : TileBase
    {
        [HideInInspector]
        public Map.GameMap map = null;

        public Color color = Color.white;

        public Sprite tileHole = null;

        public Sprite tileHorizontalEnd = null;

        public Sprite tileVerticalEnd = null;

        public Sprite tileInternalCorner = null;

        public Sprite tileExternalCorner = null;

        public Sprite tileVertical = null;

        public Sprite tileHorizontal = null;

        public Sprite tileDebug = null;

        public Sprite tileRemarkHorizontalSide = null;

        public bool debug = false;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Vector3Int neighbour = position + new Vector3Int(x, y);

                    if (HasShadow(tilemap, neighbour))
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

            if (CheckRuleInternalCorner(position)) { tileData.sprite = tileInternalCorner; return; }
            if (CheckRuleExternalCorner(position)) { tileData.sprite = tileExternalCorner; return; }
            if (CheckRuleHorizontalEnd (position)) { tileData.sprite = tileHorizontalEnd; return; }
            if (CheckRuleVerticalEnd   (position)) { tileData.sprite = tileVerticalEnd; return; }
            if (CheckRuleHorizontalDown(position)) { tileData.sprite = tileHorizontal; return; }
            if (CheckRuleVerticalRight (position)) { tileData.sprite = tileVertical; return; }
        }

        public bool CheckRuleHole(Vector3Int position)
        {
            return HasWall(position, -1, 0) && HasWall(position, 0, 1) && HasWall(position, 1, 0) && HasWall(position, 0, -1);
        }

        public bool CheckRuleInternalCorner(Vector3Int position)
        {
            return HasWall(position, -1, 0) && HasWall(position, 0, 1);
        }

        public bool CheckRuleExternalCorner(Vector3Int position)
        {
            return HasWall(position, -1,  1) && !HasWall(position, -1, 0) && !HasWall(position, 0, 1);
        }

        public bool CheckRuleHorizontalEnd(Vector3Int position)
        {
            return HasWall(position, 0, 1) && !HasWall(position, -1, 1);
        }

        public bool CheckRuleVerticalEnd(Vector3Int position)
        {
            return HasWall(position, -1, 0) && !HasWall(position, -1, 1);
        }

        public bool CheckRuleHorizontalDown(Vector3Int position)
        {
            return HasWall(position, 0, 1);
        }

        public bool CheckRuleVerticalRight(Vector3Int position)
        { 
            return HasWall(position, -1, 0);
        }

        public bool HasShadow(ITilemap tilemap, Vector3Int position) => HasShadow(tilemap, position, 0, 0);

        public bool HasShadow(ITilemap tilemap, Vector3Int position, int xOffset, int yOffset)
        {
            return tilemap.GetTile(position + new Vector3Int(xOffset, yOffset)) == this;
        }

        public bool HasWall(Vector3Int position) => HasWall(position, 0, 0);

        public bool HasWall(Vector3Int position, int xOffset, int yOffset)
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
