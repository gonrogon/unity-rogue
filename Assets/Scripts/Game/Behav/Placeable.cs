using Rogue.Coe;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Placeable : GameBehaviour<Placeable>, Map.IMulticell
    {
        private Comp.Building m_building;

        private Comp.Location m_location;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            m_building = entity.FindFirstComponent<Comp.Building>();
            m_location = entity.FindFirstComponent<Comp.Location>();

            return true;
        }

        public override void OnInit()
        {
            Context.Map.AddMulticell(Entity.Id, this);
        }

        public override void OnQuit()
        {
            Context.Map.RemoveMulticell(Entity.Id);
        }

        public Vec2i GetOrigin()
        {
            if (m_building != null)
            {
                return m_building.zone.Min;
            }
            
            if (m_location != null)
            {
                return m_location.position;
            }

            return Vec2i.Zero;
        }

        public bool ContainsCoord(Ident eid, Vec2i coord)
        {
            if (m_building != null)
            {
                return m_building.zone.Contains(coord);
            }
            
            if (m_location != null)
            { 
                return m_location.position == coord;
            }

            return false;
        }
    }
}
