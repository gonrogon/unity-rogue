using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Views
{
    public class ViewStockpile : ViewBase
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
        protected Game.Comp.Stockpile m_stockpile;

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
            m_stockpile = entity.FindFirstComponent<Game.Comp.Stockpile>();
            if (m_stockpile == null)
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
            Game.Stock.Stockpile stockpile = Context.Stock.At(m_stockpile.id);
            if (stockpile == null)
            {
                return;
            }
            
            foreach (Vec2i coord in stockpile.Bounds)
            {
                if (Context.Map.IsSolid(coord))
                {
                    continue;
                }

                if (Context.Map.IsZone(coord, stockpile.Zone) == false)
                {
                    continue;
                }

                Context.Map.SetCell(coord, "stockpiles", "default", null);

                /*
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
                */
            }
        }

        #endregion
    }
}
