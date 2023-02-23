using UnityEngine;
using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeFindPath : NodeActionBase
    {
        /// <summary>
        /// Defines an enumeration with the states of the follow algorithm.
        /// </summary>
        private enum FindState
        {
            Initial, // Initial state.
            Waiting, // Waiting for the path manager to complete the request.
            Success, // A path was successful found.
            Failure  // No path was found.
        }

        /// <summary>
        /// Follow state.
        /// </summary>
        private FindState m_find = FindState.Initial;

        /// <summary>
        /// Path.
        /// </summary>
        private Path2i m_path = null;

        /// <summary>
        /// Name of the variable to get as origin.
        /// </summary>
        private readonly string m_originVar = "pathOrigin";

        /// <summary>
        /// Name of the variable to get as target.
        /// </summary>
        private readonly string m_targetVar = "pathTarget";

        /// <summary>
        /// Flag indicating whether or not the position of the origin have to be included in the path.
        /// </summary>
        private readonly bool m_includeOrigin = false;

        /// <summary>
        /// Flag indicating whether or not the position of the target have to be included in the path.
        /// </summary>
        private readonly bool m_includeTarget = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeFindPath() {}

        public NodeFindPath(string origin, string target)
        {
            m_originVar = origin;
            m_targetVar = target;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="includeOrigin">True if the position of the origin have to be included in the
        /// path; otherwise, false.</param>
        /// <param name="includeTarget">True if the position of the target have to be included in the
        /// path; otherwise, false.</param>
        public NodeFindPath(bool includeOrigin, bool includeTarget)
        {
            m_includeOrigin = includeOrigin;
            m_includeTarget = includeTarget;
        }

        public override void OnInit()
        {
            m_find = FindState.Initial;
        }

        public override void OnQuit(NodeState state)
        {
            // TODO: If there is an active request, it should be cancelled in case of abortion.
            m_path = null;
        }

        public override NodeState OnUpdate() => Step();

        /// <summary>
        /// Executes a step of the find algorithm.
        /// 
        /// Note that more than one step can be executed per tick.
        /// </summary>
        /// <returns>Node state.</returns>
        public NodeState Step()
        {
            switch (m_find)
            {
                case FindState.Initial:
                {
                    switch (RequestPath())
                    {
                        case < 0: m_find = FindState.Failure; break;
                        case   0: m_find = FindState.Waiting; break;
                        case > 0:
                        {
                            m_find = FindState.Success;
                            Step();
                        }
                        break;
                    }
                }
                break;

                case FindState.Success:
                {
                    Blackboard.Set("path", m_path);
                }
                break;
            }

            var state = m_find switch
            {
                FindState.Success => NodeState.Success,
                FindState.Initial => NodeState.Failure,
                FindState.Failure => NodeState.Failure,
                _                 => NodeState.Running
            };

            return state;
        }

        private void OnPathReceived(bool success, Path2i path)
        {
            if (m_find != FindState.Waiting)
            {
                return;
            }

            m_find = success ? FindState.Success : FindState.Failure;
            m_path = path;
        }

        private int RequestPath()
        {
            Ident eid = AgentState.eid;
            // Get the origin and target of the path.
            if (!TryGetOrigin(out Vec2i origin)) { return -1; }
            if (!TryGetTarget(out Vec2i target)) { return -1; }
            // Base case, we are already at the target.
            if (origin == target)
            {
                /*
                if (Query.MapTryGetRandomPassableSpot(origin, eid, out Vec2i spot))
                {
                    m_path = new Path2i();
                    if (m_includeOrigin) { m_path.Add(origin); }
                                           m_path.Add(spot);
                    return 1;   
                }
                */
                m_path = null;
                return 1;
            }
            // Make the request.
            Context.PathManager.Enqueue(new Map.PathRequest(origin, target, OnPathReceived)
            {
                includeOrigin = m_includeOrigin,
                includeTarget = m_includeTarget,
                parameters    = new ()
                {
                    solid = (Map.GameMap map, Vec2i coord) => !Query.MapIsPassable(coord, eid)
                }
            });
            // Done.
            return 0;
        }

        private bool TryGetOrigin(out Vec2i origin)
        {
            if (string.IsNullOrEmpty(m_originVar) || !Blackboard.TryGet(m_originVar, out object obj))
            {
                obj = AgentState.eid;
            }

            return TryGetPosition(obj, out origin);
        }

        private bool TryGetTarget(out Vec2i target)
        {
            if (string.IsNullOrEmpty(m_targetVar) || !Blackboard.TryGet(m_targetVar, out object obj))
            {
                obj = null;
            }

            return TryGetPosition(obj, out target);
        }

        private bool TryGetPosition(object obj, out Vec2i position)
        {
            switch (obj)
            {
                case Vec2i:
                {
                    position = (Vec2i)obj;
                    return true;
                }

                case Ident:
                {
                    var  query = Query.GetPosition((Ident)obj);
                    if (!query)
                    {
                        position = Vec2i.Zero;
                        return false;
                    }

                    position = query.value;
                    return true;
                }
            }

            position = Vec2i.Zero;
            return false;
        }
    }
}
