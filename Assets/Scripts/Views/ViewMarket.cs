using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Views
{
    public class ViewMarket : ViewBase
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

        /// <summary>
        /// Crop component.
        /// </summary>
        protected Game.Comp.Building m_building;

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

            m_building = entity.FindFirstComponent<Game.Comp.Building>();
            if (m_building == null)
            {
                return false;
            }

            return true;
        }

        public override void OnMessage(IGameMessage message)
        {
            UpdateWorldPosition();

            switch (message)
            {
                case Game.Msg.Plow pl: 
                {}
                break;
            }
        }

        public override void UpdateWorldPosition()
        {
            Game.Comp.Building building = m_entity.FindFirstAny<Game.Comp.Building>();
            if (building == null)
            {
                return;
            }

            foreach (Vec2i coord in building.zone)
            {
                Context.Map.SetCell(coord, "buildings", "market", null);
            }
        }

        #endregion
    }
}
