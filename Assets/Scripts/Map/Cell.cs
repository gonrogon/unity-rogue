namespace Rogue.Map {

[System.Serializable]
public struct Cell
{
    public int floor;

    public int floorIndex;

    public int wall;

    public int wallIndex;

    public bool solid;

    public int durability;

    public int zone;

    /// <summary>
    /// Flag indicating if the wall is solid or not.
    /// </summary>
    public bool IsSolid => solid;

    public void SetFloor(Floor floor, System.Random random)
    {
        floorIndex = 0;

        if (floor is null)
        {
            this.floor = -1;
            return;
        }

        this.floor = floor.id;
        floorIndex = floor.floorData.GetRandomTileIndex(random);
    }

    public void SetWall(Wall wall, System.Random random)
    {
        wallIndex = 0;

        if (wall == null)
        {
            this.wall = -1;
            return;
        }

        this.wall  = wall.id;
        wallIndex  = wall.wallData.GetRandomTileIndex(random);
        solid      = wall.Solid;
        durability = wall.Durability;
    }

    public void Reset(Floor floor, Wall wall, System.Random random)
    {
        if (floor != null) { SetFloor(floor, random); } else { this.floor = -1; floorIndex = 0; }
        if (wall  != null) { SetWall (wall,  random); } else { this.wall  = -1; wallIndex  = 0; }
    }

    public void Clear()
    {
        floor      = -1;
        floorIndex =  0;
        wall       = -1;
        wallIndex  =  0;
        zone       = -1;
    }
}

}
