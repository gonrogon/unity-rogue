using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Rogue.Map;
using GG.Mathe;

namespace Rogue 
{
    public class MapView : MonoBehaviour
    {
        public Tilemap m_ground;

        public Tilemap m_shadow;

        public Tilemap m_boder;

        public Tilemap m_debug;

        [SerializeField]
        public TileBase m_shadowTile;

        [SerializeField]
        public TileBase m_boderTile;

        public Tile m_debugPathTile;

        public Tile m_selectionTile;

        public Vec2i GetMouseCoord()
        {
            Vector2 mouse = Mouse.current.position.ReadValue();

            return GetMouseCoord(mouse.x, mouse.y);
        }

        public Vec2i GetCursorCell(Vector2 cursor)
        {
            Vector3Int cell = m_ground.WorldToCell(cursor);

            return new Vec2i(cell.x, cell.y);
        }

        public Vec2i GetMouseCoord(float mx, float my)
        {
            Vector3    world = Camera.main.ScreenToWorldPoint(new Vector3(mx, my, -10.0f));
            Vector3Int cell  = m_ground.WorldToCell(world);

            return new Vec2i(cell.x, cell.y);;
        }

        public void Update()
        {
            GetMouseCoord();    
        }

        public void Sync(GameMap map)
        {
            var rect = new Rect2i(Vec2i.Zero, map.Width, map.Height);

            foreach (Vec2i coord in rect)
            {
                var floor = map.GetFloorData(coord);
                var wall  = map.GetWallData(coord);
                // Show the wall.
                if (wall != null)
                {
                    m_ground.SetTile(new Vector3Int(coord.x, coord.y, 0), wall.GetTileByIndex(map.GetWallTileIndex(coord)));
                    continue;
                }
                // No wall, show the floor.
                if (floor != null)
                {
                    m_ground.SetTile(new Vector3Int(coord.x, coord.y, 0), floor.GetTileByIndex(map.GetFloorTileIndex(coord)));
                    continue;
                }

                //m_ground.SetTile(new Vector3Int(coord.x, coord.y, 0), terrain.GetDefaultFloor().tile);
            }
            // Shadows
            /*
            foreach (Vec2i coord in rect)
            {
                if (map.HasWall(coord))
                {
                    continue;
                }
            
                var up     = coord + Vec2i.Up;
                var left   = coord + Vec2i.Left;
                var corner = coord + Vec2i.Left + Vec2i.Up;
            
                if (CheckWall(map, up) || CheckWall(map, left) || CheckWall(map, corner))
                {
                    var tilePos = new Vector3Int(coord.x, coord.y, 0);

                    if (m_shadowTile is ShadowTile shadow)
                    {
                        shadow.map = map;
                    }

                    m_shadow.SetTile(tilePos, m_shadowTile);
                }
            }
            */
            // Borders
            /*
            foreach (Vec2i coord in rect)
            {
                if (map.HasWall(coord))
                {
                    continue;
                }

                if (BorderTile.NeedBorderTile(map, coord))
                {
                    var tilePos = new Vector3Int(coord.x, coord.y, 0);

                    if (m_boderTile is BorderTile border)
                    {
                        border.map = map;
                    }

                    m_boder.SetTile(tilePos, m_boderTile);
                }
            }
            */
        }

        #region @@@ DEBUG @@@

        public void SetSelection(Rect2i rect)
        {
            foreach (Vec2i coord in rect)
            {
                m_debug.SetTile(new Vector3Int(coord.x, coord.y, 0), m_selectionTile);
            }
        }

        public void SetDebugPath(Vec2i coord)
        {
            m_debug.SetTile(new Vector3Int(coord.x, coord.y, 0), m_debugPathTile);
        }

        public void ClearDebug()
        {
            m_debug.ClearAllTiles(); 
        }

        #endregion

        #region @@@ UTILITIES @@@

        public static bool CheckWall(GameMap map, Vec2i position)
        {
            return map.HasCoord(position) && map.HasWall(position);
        }

        #endregion
    }

}
