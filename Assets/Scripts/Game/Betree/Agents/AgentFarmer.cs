using Rogue.Core;
using Rogue.Core.Betree;

namespace Rogue.Game.Betree.Agents
{
    public class AgentFarmer : AgentBase
    {
        public AgentFarmer(Ident eid) : base(eid) {}

        public override void OnScheduleStart(SchedulerHandler handler)
        {
            base.OnScheduleStart(handler);
            CreateBuilder();
        }

        public void CreateBuilder()
        {
            m_tree.SetRoot(new NodeSequence(
                new NodeSelectJob(),
                new NodeSelector(
                    CreatePlowTree(),
                    CreateSeedTree(),
                    CreateHarvestTree(),
                    CreateStockTree(),
                    CreateTradeTree()
                )
            ));
        }

        public Node CreatePlowTree()
        {
            return new NodeJobPlow(
                new NodeSequence(
                    CreateGoTree(null, "cropLocation"),
                    new NodeActionPlow()
                ),
                null
            );
        }

        public Node CreateSeedTree()
        {
            return new NodeJobSeed(
                new NodeSequence(
                    CreateGoTree(null, "cropLocation"),
                    new NodeActionSeed()
                ),
                null
            );
        }

        public Node CreateHarvestTree()
        {
            return new NodeJobHarvest(
                new NodeSequence(
                    CreateGoTree(null, "cropLocation"),
                    new NodeActionHarvest()
                ),
                null
            );
        }

        public Node CreateStockTree()
        {
            return new NodeJobStockpile(
                new NodeSequence(
                    CreatePickUpTree("itemId"),
                    new NodeSetVar<bool>("picked", true),
                    CreateDropTree  ("itemId", "stockpileLocation")
                ),
                null
            );
        }

        public Node CreateTradeTree()
        {
            return new NodeJobTrade(
                new NodeSequence(
                    CreatePickUpTree("itemId"),
                    CreateStoreTree ("itemId", "marketId", "marketLocation")
                ),
                null
            );
        }
    }
}
