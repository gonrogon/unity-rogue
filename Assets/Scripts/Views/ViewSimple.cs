using Rogue.Coe;
using UnityEngine;

namespace Rogue.Views
{
    public class ViewSimple : ViewBase
    {
        /// <summary>
        /// Grid to place the sprite.
        /// </summary>
        protected Grid m_grid = null;

        /// <summary>
        /// Transformation.
        /// </summary>
        protected Transform m_transform = null;

        /// <summary>
        /// Renderer.
        /// </summary>
        protected SpriteRenderer m_renderer = null;

        public override void SetupView(ViewManager manager)
        {
            base.SetupView(manager);
            m_grid = m_manager.Grid;
        }

        #region @@@ UNITY LIFECYCLE @@@

        private void Awake()
        {
            m_transform = transform;
            m_renderer  = GetComponent<SpriteRenderer>();
        }

        #endregion

        #region @@@ VIEW IMPL @@@

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            if (m_renderer.sprite == null && m_view != null)
            {
                if (m_manager.TryGetSprite(m_view.sprite, out Sprite sprite))
                {
                    m_renderer.sprite = sprite;
                }
            }

            return true;
        }

        public override void OnMessage(IGameMessage message)
        {
            UpdateWorldPosition();

            switch (message)
            {
                // NOTE: Game object should be destroy if the entity is picked and created again when it is drop. So
                // a system to recognise this should be needed.
                case Game.Msg.PickUp pk: 
                {
                    if (pk.done && pk.what == m_entity.Id)
                    {
                        m_renderer.enabled = false;
                    }
                }
                break;

                case Game.Msg.Drop dr:
                {
                    if (dr.done && dr.what == m_entity.Id)
                    {
                        m_renderer.enabled = true;
                    }
                }
                break;
            }
        }

        public override void UpdateWorldPosition()
        {
            if (m_location == null)
            {
                return;
            }

            m_transform.position   = m_grid.GetCellCenterWorld(new Vector3Int(m_location.position.x, m_location.position.y, 0));
            m_transform.localScale = m_grid.transform.lossyScale;
        }

        #endregion
    }
}
