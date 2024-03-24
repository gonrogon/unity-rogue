using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;
using System.Collections.Generic;

namespace Rogue.Game.Betree
{
    public class NodeSensorAreaRandomPoint : NodeActionBase
    {
        /// <summary>
        /// Name of the variable to get as origin.
        /// </summary>
        private readonly string m_originVar = "";

        private int m_length = 4;

        public override NodeState OnUpdate()
        {
            if (!TryGetOrigin(out Vec2i origin))
            {
                return NodeState.Failure;
            }

            Rect2i rect = new Rect2i(origin - new Vec2i(m_length, m_length), m_length * 2, m_length * 2);

            int x = UnityEngine.Random.Range(0, rect.Width);
            int y = UnityEngine.Random.Range(0, rect.Height);

            int fx = origin.x - m_length + x;
            int fy = origin.y - m_length + y;

            Vec2i target = new Vec2i(fx, fy);

            if (target == origin)
            {
                return NodeState.Failure;
            }

            UnityEngine.Debug.Log($"Set random target {target}");

            Blackboard.Set("sensorTarget", target);

            return NodeState.Success;
        }

        private bool TryGetOrigin(out Vec2i origin)
        {
            if (string.IsNullOrEmpty(m_originVar) || !Blackboard.TryGet(m_originVar, out object obj))
            {
                obj = AgentState.eid;
            }

            return TryGetPosition(obj, out origin);
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
