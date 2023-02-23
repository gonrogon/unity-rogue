using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeFindPath : NodeAction
    {
        private enum State
        {
            None,
            Waiting,
            Received,
            Done,
        }

        private Ident m_entity;

        private State m_state = State.None;

        private bool m_success = false;

        private Path2i m_result = null;

        public NodeFindPath(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            switch (m_state)
            {
                case State.None:    
                {
                    switch (RequestPath())
                    {
                        case < 0:
                        {
                            m_state = State.None;
                            return NodeState.Failure;
                        }

                        case > 0:
                        {
                            m_state = State.None;
                            SetVar("path", m_result);
                            return NodeState.Success;
                        }

                        case 0:
                        {
                            m_state = State.Waiting;
                            return NodeState.Running;
                        }
                    }
                }

                case State.Waiting:
                {
                    return NodeState.Running;
                }

                case State.Received:
                {
                    m_state = State.None;
                    
                    if (m_result != null)
                    {
                        SetVar("path", m_result);
                    }

                    return m_success ? NodeState.Success : NodeState.Failure;
                }
            }

            return NodeState.Failure;
        }

        private int RequestPath()
        {
            var entityPos = Query.GetPosition(m_entity);
            //var varTarget = GetVar("targetPos");

            if (!entityPos)
            {
                return -1;
            }

            if (!TryFindVar("targetLocation", out Vec2i targetLocation))
            {
                return -1;
            }

            if (entityPos.value == targetLocation)
            {
                m_result = new Path2i
                {
                    targetLocation
                };

                return 1;
            }

            // Create the path request.
            Map.PathRequest request = new (entityPos.value, targetLocation, OnPathReceived)
            {
                includeOrigin = false,
                includeTarget = false,
                parameters    = new ()
                {
                    solid = (Map.GameMap map, Vec2i coord) =>
                    {
                        return !Query.MapIsPassable(coord, m_entity);
                    }
                }
            };

            Context.PathManager.Enqueue(request);

            //m_state = State.Waiting;

            return 0;
        }

        private void OnPathReceived(bool success, Path2i path)
        {
            m_state   = State.Received;
            m_success = success;
            m_result  = path;
        }
    }
}
