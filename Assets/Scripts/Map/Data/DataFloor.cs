using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Map.Data {

/// <summary>
/// Define a type of floor.
/// </summary>
[CreateAssetMenu(menuName = "Map/Floor")]
public class DataFloor : ScriptableObject
{
    /// <summary>
    /// Name.
    /// </summary>
    public string title = string.Empty;

    /// <summary>
    /// Description.
    /// </summary>
    public string description = string.Empty;

    /// <summary>
    /// Tile.
    /// </summary>
    public Tile tile = null;

    public TileBase tileBase = null;

    /// <summary>
    /// Tile group.
    /// </summary>
    public TileGroup tileGroup = null;

    public TileBase GetTileByIndex(int index)
    {
        if (tileGroup != null)
        {
            return tileGroup.GetTile(index);
        }

        if (tileBase != null)
        {
            return tileBase;
        }

        return tile;
    }

    /// <summary>
    /// Get a random tile index.
    /// </summary>
    /// <param name="random">Randomizer.</param>
    /// <returns>Random tile index.</returns>
    public int GetRandomTileIndex(System.Random random)
    {
        if (tileGroup != null)
        {
            return tileGroup.GetRandomIndex(random);
        }

        return 0;
    }

    /// <summary>
    /// Get a random tile.
    /// </summary>
    /// <returns>Tile.</returns>
    public Tile GetRandomTile(System.Random random)
    {
        if (tileGroup != null)
        {
            return tileGroup.GetRandomTile(random);
        }

        return tile;
    }
}

}
