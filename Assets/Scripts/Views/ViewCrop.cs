using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Views
{
    public class ViewCrop : ViewBase
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
        protected Game.Comp.Crop m_crop;

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
            /*
            if (m_renderer.sprite == null && m_view != null)
            {
                if (m_manager.TryGetSprite(m_view.sprite, out Sprite sprite))
                {
                    m_renderer.sprite = sprite;
                }
            }
            */
            m_crop = entity.FindFirstComponent<Game.Comp.Crop>();
            if (m_crop == null)
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
            Game.Crops.Crop crop = Context.Crops.At(m_crop.cropId);
            if (crop == null)
            {
                return;
            }
            
            foreach (Vec2i coord in crop.Zone)
            {
                Game.Crops.Soil  soil  = crop.GetSoil(coord);
                Game.Crops.Plant plant = crop.GetPlant(coord);
                
                if (plant == null || crop.GetPlantType().GetView(plant.growth) == null)
                {
                    if (soil.plowed)
                    {
                        Context.Map.SetCell(coord, "crops", "soil_plowed", null);
                    }
                    else
                    {
                        Context.Map.SetCell(coord, "crops", "soil", null);
                    }
                }
                else
                {
                    Game.Crops.PlantView view = crop.GetPlantType().GetView(plant.growth);

                    Context.Map.SetCell(coord, view.biome, view.floor, view.wall);
                }
            }
        }

        #endregion
    }
}
