using Rogue.Core;
using Rogue.Coe;

namespace Rogue.Game
{
    public class BodyWorldListener : IGameWorldListerner
    {
        private BodySystem m_bodies;

        public BodyWorldListener(BodySystem bodies) 
        {
            m_bodies = bodies;
        }

        #region @@@ WORLD LISTENER @@@

        public void OnEntityAdded(GameWorld world, GameEntity entity) 
        {
            if (!entity.ContainsComponent<Comp.Body>())
            {
                return;
            }

            SetBody(entity);
        }

        public void OnEntityRemoved(GameWorld world, GameEntity entity) 
        {
            if (!entity.ContainsComponent<Comp.Body>())
            {
                return;
            }

            RemoveBody(entity);
        }

        public void OnComponentAdded(GameWorld world, GameEntity entity, IGameComponent component)
        {
            if (component is not Comp.Body body) 
            {
                return;
            }

            SetBody(entity, body);
        }

        public void OnComponentRemoved(GameWorld world, GameEntity entity, IGameComponent component)
        {
            if (component is not Comp.Body body)
            {
                return;
            }

            RemoveBody(entity, body);
        }

        public void OnBehaviourAdded(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        public void OnBehaviourRemoved(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        #endregion

        private void SetBody(GameEntity entity) => SetBody(entity, entity.FindFirstComponent<Comp.Body>());

        private void SetBody(GameEntity entity, Comp.Body body)
        {
            if (!entity.IsRunning)
            {
                return;
            }

            if (body.bid.IsZero)
            {
                if (string.IsNullOrEmpty(body.template))
                {
                    return;
                }

                body.bid = m_bodies.Add(body.template);
            }
            else
            {
                UnityEngine.Debug.Log(entity.Id.Value + " - " + Query.GetName(entity.Id).value + ": Body already asigned");
            }
        }

        private void RemoveBody(GameEntity entity) => RemoveBody(entity, entity.FindFirstComponent<Comp.Body>());

        private void RemoveBody(GameEntity entity, Comp.Body body)
        {
            if (!entity.IsRunning)
            {
                return;
            }

            if (body.bid.IsZero)
            {
                return;
            }

            m_bodies.Remove(body.bid);
            body.bid = Ident.Zero;
        }
    }
}
