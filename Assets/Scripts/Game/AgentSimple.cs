using UnityEngine;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game
{
    public class AgentSimple : AgentBase
    {
        Ident m_entity;

        Ident m_player;

        enum State
        {
            Normal,
            WaitingForPath,
            Moving,
        }

        State m_state = State.Normal;

        Vec2i m_target;

        public AgentSimple(Ident entity, Ident player)
        {
            m_entity = entity;
            m_player = player;
        }

        public override int OnScheduleTrigger()
        {
            switch (m_state)
            {
                case State.Normal:         return OnNormal();
                case State.WaitingForPath: return 0;
                case State.Moving:         return OnMove();
            }

            return 0;
        }

        private int OnNormal()
        {
            var entityPos = Query.GetPosition(m_entity);
            var playerPos = Query.GetPosition(m_player);

            if (entityPos && playerPos)
            {
                int dx = Mathf.Abs(playerPos.value.x - entityPos.value.x);
                int dy = Mathf.Abs(playerPos.value.y - entityPos.value.y);

                if (dx > 1 ||dy > 1)
                {
                    if (Query.MapIsVisible(entityPos.value, playerPos.value, m_entity))
                    {
                        m_state = State.WaitingForPath;

                        Map.PathRequest req = new Map.PathRequest(entityPos.value, playerPos.value, OnPathReceived);

                        req.parameters = new Map.PathFinder.Parameters();

                        req.parameters.solid = (Map.GameMap map, Vec2i coord) =>
                        {
                            return !Query.MapIsPassable(coord, m_entity);
                        };

                        //Context.Map.RequestPath(new Map.PathRequest(entityPos.value, playerPos.value, OnPathReceived));
                        //Context.Map.RequestPath(req);
                        Context.PathManager.Enqueue(req);
                        
                        return  0;
                    }
                }
                else
                {
                    return DoAttack(m_entity, m_player);
                }
            }

            return 0;
        }

        private int OnMove()
        {
            m_state = State.Normal;

            return DoMove(m_entity, m_target - Query.GetPosition(m_entity).value);
        }

        private void OnPathReceived(bool success, Path2i path)
        {
            if (!success || path.Count < 2)
            {
                m_state = State.Normal;
                return;
            }

            m_state  = State.Moving;
            m_target = path.At(1);
        }

        private int DoMove(Ident entity, Vec2i dir)
        {
            Msg.ActionMove move = new Msg.ActionMove(dir);

            Context.World.Send(entity, move);

            return move.cost;
        }

        private int DoAttack(Ident entity, Ident target)
        {
            Msg.ActionAttackBlow msg = new Msg.ActionAttackBlow();

            msg.origin = entity;
            msg.target = target;
            msg.weapon = Ident.Zero;

            Context.World.Send(entity, msg);

            return msg.cost;
        }
    }
}
