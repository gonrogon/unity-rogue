using UnityEngine;
using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game
{
    public class AgentBHT : AgentBase
    {
        public class State
        {
            public int cost = 0;
        }

        Ident m_entity;

        Ident m_player;

        Node m_bht;



        private State m_state = new State();

        public AgentBHT(Ident entity, Ident player)
        {
            m_entity = entity;
            m_player = player;
        }

        public override void OnScheduleStart(SchedulerHandler handler)
        {
            base.OnScheduleStart(handler);

            //CreateFollowBHT();
            CreateBuilder();

            if (m_bht != null)
            {
                m_bht.SetGlobalVar("state", m_state);
            }
        }

        public override int OnScheduleTrigger()
        {
            if (m_bht == null)
            {
                return 0;
            }

            m_bht.Evaluate();

            int lastCost = m_state.cost;
            m_state.cost = 0;

            return lastCost;
        }

        public void CreateBuilder()
        {
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
            /*
            var sequence = new NodeSequence();
            var select   = new NodeSelect();

            sequence.Attach(new Nodes.NodeSelectJob(m_entity));
            sequence.Attach(select);
            select.Attach(CreatePlowTree());
            select.Attach(CreateSeedTree());
            select.Attach(CreateHarvestTree());
            select.Attach(CreateStockTree());
            select.Attach(CreateTradeTree());

            m_bht = sequence;
            */
        }

        public Node CreatePlowTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeJobPlow(),
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new NodeRepeat(
                    new Nodes.NodePlow(m_entity)
                )
            );
            /*
            var sequence = new NodeSequence();
            var job      = new Nodes.NodeJobPlow();
            var path     = new Nodes.NodeFindPath(m_entity);
            var follow   = CreateFollowTree();
            var repeat   = new NodeRepeat();
            var plow     = new Nodes.NodePlow(m_entity);

            repeat.Attach(plow);

            sequence.Attach(job);
            sequence.Attach(path);
            sequence.Attach(follow);
            sequence.Attach(repeat);

            return sequence;
            */
        }

        public Node CreateSeedTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeJobSeed(),
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new NodeRepeat(
                    new Nodes.NodeSeed(m_entity)
                )
            );
        }

        public Node CreateHarvestTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeJobHarvest(),
                //new NodeCondition(new Nodes.NodeFindPath(m_entity), new NodePrint("Path found"), new NodeIgnore(false, new NodePrint("No path"))),
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new NodeRepeat(
                    //new NodeCondition(new Nodes.NodeHarvest(m_entity), null, new NodeIgnore(false, new NodePrint("Harvest failure")))
                    new Nodes.NodeHarvest(m_entity)
                )
            );
        }

        public Node CreateStockTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeJobStockpile(),
                CreatePickUpTree(),
                new NodeVarCopy("stockpileLocation", "targetLocation"),
                CreateDropTree(),
                new Nodes.NodeJobStockpileDone()
            );
        }

        public Node CreateTradeTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeJobTrade(),
                CreatePickUpTree(),
                new NodeVarCopy("marketLocation", "targetLocation"),
                CreateMarketTree(),
                new Nodes.NodeJobTradeDone()
            );
        }

        public Node CreateDropTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new Nodes.NodeDrop(m_entity)
            );
        }

        public Node CreatePickUpTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new Nodes.NodePickUp(m_entity)
            );
        }

        public Node CreateMarketTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeFindPath(m_entity),
                CreateFollowTree(),
                new Nodes.NodeMarket(m_entity)
            );
        }

        public Node CreateGoTree()
        {
            return
            new NodeSequence(
                new Nodes.NodeFindPath(m_entity),
                new Nodes.NodeFollowPath2(
                    new Nodes.NodeMove2(m_entity)
                )
            );
        }

        public Node CreateFollowTree()
        {
            return
            new Nodes.NodeFollowPath2(
                new Nodes.NodeMove2(m_entity)
            );
            /*
            return 
            new NodeRepeat(
                new NodeSequence(
                    new Nodes.NodeFollowPath(m_entity),
                    new Nodes.NodeMove(m_entity)
                )
            );
            */
        }
    }
}
