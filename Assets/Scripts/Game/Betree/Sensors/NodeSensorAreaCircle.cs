using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;
using System.Collections.Generic;

namespace Rogue.Game.Betree
{
    public class NodeSensorAreaCircle : NodeActionBase
    {
        /// <summary>
        /// Name of the variable to get as origin.
        /// </summary>
        private readonly string m_originVar = "";

        public override NodeState OnUpdate()
        {
            if (!TryGetOrigin(out Vec2i origin))
            {
                return NodeState.Failure;
            }

            Blackboard.Set("sensorArea", new Circle2i(origin, Blackboard.Get<int>("sensorRadius")));

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
