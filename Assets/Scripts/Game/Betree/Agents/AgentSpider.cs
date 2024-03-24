using Rogue.Core;
using Rogue.Core.Betree;

namespace Rogue.Game.Betree.Agents
{
    public class AgentSpider : AgentBase
    {
        public AgentSpider(Ident eid) : base(eid) {}

        public override void OnScheduleStart(SchedulerHandler handler)
        {
            base.OnScheduleStart(handler);
            CreateSpider();
        }

        public void CreateSpider()
        {
            m_tree.SetRoot(new NodeSequence(
                new NodeSelector(
                    CreateFollow(),
                    CreateWander()
                )
            ));
        }

        public Node CreateWander()
        {
            return new NodeSequence(
                new NodeSensorAreaRandomPoint(),
                CreateGoTree(null, "sensorTarget")
            );
        }

        public Node CreateFollow()
        {
            m_tree.Blackboard.Set<int>("sensorRadius", 5);

            return new NodeSequence(
                new NodeSensorAreaCircle(),
                new NodeSensorPlayer(),
                CreateGoTree(null, "sensorTarget")
            );
        }
    }
}
