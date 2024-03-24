using System.Collections.Generic;
using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;


namespace Rogue.Game.Betree
{
    public class NodeFollowPath : NodeActionBase
    {
        /// <summary>
        /// Defines an enumeration with the states of the follow algorithm.
        /// </summary>
        private enum FollowState
        {
            Initial, // Initial state.
            Seeking, // Seeking the next waypoint.
            Walking, // Walking to the waypoint.
            Success, // The path has been full covered.
            Failure, // A problem occurs while following the path.
        }

        /// <summary>
        /// Follow state.
        /// </summary>
        private FollowState m_follow = FollowState.Initial;

        /// <summary>
        /// Iterator.
        /// </summary>
        private IEnumerator<Vec2i> m_cursor = null;

        public override void OnInit()
        {
            m_follow = FollowState.Initial;
        }

        public override void OnQuit(NodeState state)
        {
            m_cursor = null;
        }

        public override NodeState OnUpdate() => Step();

        /// <summary>
        /// Executes a step of the follow algorithm.
        /// 
        /// Note that more than one step can be executed per tick.
        /// </summary>
        /// <returns>Node state.</returns>
        private NodeState Step()
        {
            switch (m_follow)
            {
                case FollowState.Initial:
                {
                    if (!Blackboard.Contains("path"))
                    {
                        m_follow = FollowState.Failure;
                    }
                    else
                    {
                        var path = Blackboard.GetOrDefault<Path2i>("path", null);
                        if (path == null || path.Count == 0)
                        {
                            m_follow = FollowState.Success;
                        }
                        else
                        {
                            m_follow = FollowState.Seeking;
                            m_cursor = path.GetEnumerator();
                            // Execute another step to ensure that the tick resulted in an movement.
                            Step();
                        }
                    }
                }
                break;

                case FollowState.Seeking: 
                {
                    if (m_cursor.MoveNext())
                    {
                        m_follow = FollowState.Walking;
                        // Execute another step to do the movement.
                        Step();
                    }
                    else
                    {
                        m_follow = FollowState.Success;
                    }
                }
                break;

                case FollowState.Walking:
                {
                    m_follow = Walk() switch
                    {
                        > 0 => FollowState.Seeking,
                        < 0 => FollowState.Failure,
                        _   => FollowState.Walking,
                    };
                }
                break;
            }

            var s = m_follow switch
            {
                FollowState.Success => NodeState.Success,
                FollowState.Initial => NodeState.Failure,
                FollowState.Failure => NodeState.Failure,
                _                   => NodeState.Running
            };

            return s;
        }

        /// <summary>
        /// Moves the entity toward the waypoint.
        /// </summary>
        /// <returns>Less than zero if there was a problem, zero if the movement need another tick, greater than zero
        /// on success.</returns>
        private int Walk()
        {
            Ident eid = AgentState.eid;
            var   loc = Context.World.Find(AgentState.eid).FindFirstComponent<Comp.Location>();
            // Check if the entity has a location componetn.
            if (loc == null)
            {
                return -1;
            }
            // Save the direction of the movement.
            var  msg = Context.World.Send(eid, new Msg.ActionMove(m_cursor.Current - loc.position));
            if (!msg.done)
            {
                return -1;
            }
            // Add the cost of the movement.
            AddActionCost(msg.cost);
            // Check if the entity has reached the destination.
            if (loc.position != m_cursor.Current)
            {
                return  0;
            }
            // Done.
            return 1;
        }
    }
}
