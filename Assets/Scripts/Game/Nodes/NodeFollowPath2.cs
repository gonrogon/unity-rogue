using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    /// <summary>
    /// Defines a node to traverse a path.
    /// 
    /// The node traveses the path saving at each iteration the next waypoint as "pathWaypoint". The child node is
    /// resposible for doing something meaningful with the waypoints.
    /// 
    /// Return:
    ///  - SUCCESS: The path has been fully traversed.
    ///  - FAILURE: The path cannot be traversed.
    ///  - RUNNING: The path is being traversed.
    /// </summary>
    public class NodeFollowPath2 : NodeDecorator
    {
        /// <summary>
        /// Define an enumeration with the states of the node.
        /// </summary>
        private enum State
        {
            Initial,
            Seeking,
            Walking,
            Success,
            Failure,
        }

        /// <summary>
        /// State.
        /// </summary>
        private State m_state = State.Initial;

        /// <summary>
        /// Cursor pointing to the next waypoint.
        /// </summary>
        private int m_cursor = 0;

        /// <summary>
        /// Path to walk.
        /// </summary>
        private Path2i m_path = null;

        public NodeFollowPath2() : this(null) {}

        public NodeFollowPath2(Node child) : base(child) {}

        public override NodeState Evaluate()
        {
            switch (m_state)
            {
                case State.Initial:
                {
                    if (FindVar("path") is not Path2i path)
                    {
                        // Path not found.
                        m_state = State.Failure;
                    }
                    else
                    {
                        // Start traversing the path.
                        m_path  = path;
                        m_state = State.Seeking;
                    }
                }
                break;

                case State.Seeking:
                {
                    if (m_cursor < m_path.Count)
                    {
                        SetVar("pathWaypoint", m_path.At(m_cursor));
                        m_cursor++;
                        // Move to the waypoint.
                        m_state = State.Walking;
                    }
                    else
                    {
                        // The path has been fully traversed.
                        m_state = State.Success;
                    }
                }
                break;

                case State.Walking:
                {
                    m_state = m_child.Evaluate() switch
                    {
                        NodeState.Success => State.Seeking,
                        NodeState.Failure => State.Failure,
                        _                 => State.Walking
                    };
                }
                break;
            }

            switch (m_state)
            {
                case State.Success:
                {
                    Reset();
                    return NodeState.Success;
                }

                case State.Initial:
                case State.Failure:
                {
                    Reset();
                    return NodeState.Failure;
                }

                default:
                {
                    return NodeState.Running;
                }
            }
        }

        public override void Reset()
        {
            m_state  = State.Initial;
            m_cursor = 0;
            m_path   = null;
        }
    }
}
