using UnityEngine;
using Rogue.Core;
using Rogue.Core.Betree;
using Rogue.Game.Betree;
using GG.Mathe;

namespace Rogue.Game
{
    public class AgentBetree : AgentBase
    {
        public class State
        {
            public Ident eid = Ident.Zero;

            public int cost = 0;
        }

        private Core.Betree.Tree m_tree;

        private State m_state = new State();

        public AgentBetree(Ident eid)
        {
            m_state.eid  = eid;
            m_state.cost = 0;
        }

        public override void OnScheduleStart(SchedulerHandler handler)
        {
            base.OnScheduleStart(handler);

            CreateBuilder();

            if (m_tree != null)
            {
                m_tree.Blackboard.Set("state", m_state);
            }
        }

        public override int OnScheduleTrigger()
        {
            if (m_tree == null)
            {
                return 0;
            }

            m_tree.Tick();

            int lastCost = m_state.cost;
            m_state.cost = 0;

            return lastCost;
        }

        public void CreateBuilder()
        {
            m_tree = new Core.Betree.Tree(
                new NodeSequence(
                    new NodeSelectJob(),
                    new NodeSelector(
                        CreatePlowTree(),
                        CreateSeedTree(),
                        CreateHarvestTree(),
                        CreateStockTree(),
                        CreateTradeTree()
                    )
                )
            );
            /*
            m_bht =
            new NodeSequence(
                new Nodes.NodeSelectJob(m_entity),
                new NodeConditional(
                    new NodeSelect(
                        CreatePlowTree(),
                        CreateSeedTree(),
                        CreateHarvestTree(),
                        CreateStockTree(),
                        CreateTradeTree()
                    ),
                    null,
                    new Nodes.NodeSelectJobCheck()
                )
            );
            */
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

        public Node CreatePickUpTree(string what)
        {
            return new NodeSequence(
                CreateGoTree(null, what),
                new NodeActionPickUp(what)
            );
        }

        public Node CreateDropTree(string what, string where)
        {
            return new NodeSequence(
                CreateGoTree(null, where),
                new NodeActionDrop(what, where)
            );
        }

        public Node CreateStoreTree(string what, string container, string where)
        {
            return new NodeSequence(
                CreateGoTree(null, where),
                new NodeActionStore(what, container)
            );
        }

        public Node CreateGoTree(string origin, string target)
        {
            return new NodeSequence(
                new NodeFindPath(origin, target),
                new NodeFollowPath()
            );
        }
    }
}
