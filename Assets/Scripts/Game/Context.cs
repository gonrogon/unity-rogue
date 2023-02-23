using Rogue.Coe;
using Rogue.Map;

namespace Rogue
{
    public class Context
    {
        public static Input Input { get; private set; } = null;

        public static GameWorld World { get; private set; } = null;

        public static GameMap Map { get; private set; } = null;

        public static PathManager PathManager { get; private set; } = null;

        public static Game.Scheduler Scheduler { get; private set; } = null;

        public static Game.CategoryStore Categories { get; private set; } = null;

        public static Game.ItemTypeStore ItemTypes { get; private set; } = null;

        public static Game.Jobs.JobManager Jobs { get; private set; } = null;

        public static Game.Stock.StockSystem Stock { get; private set; } = null;

        public static Game.BodySystem Bodies { get; private set; } = null;
        
        public static Game.InventorySystem Inventories { get; private set; } = null;

        public static Game.Crops.CropSystem Crops { get; private set; } = null;

        public static void Provide(Input input)
        {
            Input = input;
        }

        public static void Provide(GameWorld world)
        {
            World = world;
        }

        public static void Provide(GameMap map)
        {
            Map = map;
        }

        public static void Provide(PathManager manager)
        {
            PathManager = manager;
        }

        public static void Provide(Game.Scheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public static void Provide(Game.CategoryStore categories)
        {
            Categories = categories;
        }

        public static void Provide(Game.ItemTypeStore itemTypes)
        {
            ItemTypes = itemTypes;
        }

        public static void Provide(Game.Jobs.JobManager jobs)
        {
            Jobs = jobs;
        }

        public static void Provide(Game.Stock.StockSystem stock)
        {
            Stock = stock;
        }

        public static void Provide(Game.BodySystem bodies)
        {
            Bodies = bodies;
        }

        public static void Provide(Game.InventorySystem inventories)
        {
            Inventories = inventories;
        }

        public static void Provide(Game.Crops.CropSystem crops)
        {
            Crops = crops;
        }
    }
}
