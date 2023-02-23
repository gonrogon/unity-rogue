using UnityEngine;

namespace Rogue.Views
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField]
        private Grid m_grid = null;

        [SerializeField]
        private ViewTable[] m_views = null;

        [SerializeField]
        private SpriteTable[] m_sprites = null;

        private ViewFactory m_factory = null;

        /// <summary>
        /// Gets the grid used to place the sprites.
        /// </summary>
        public Grid Grid => m_grid;

        public void Listen(Coe.GameWorld world, Map.GameMap map)
        {
            world.OnEntityAdded   += OnWorldEntityAdded;
            world.OnEntityRemoved += OnWorldEntityRemoved;
            map.OnEntityAdded     += OnMapEntityAdded;
            map.OnEntityRemoved   += OnMapEntityRemoved;
        }

        private void OnWorldEntityAdded(Coe.GameWorld world, Coe.GameEntity entity)
        {
            if (entity.ContainsView())
            {
                return;
            }

            var view = entity.FindFirstComponent<Game.Comp.View>();
            if (view == null)
            {
                return;
            }

            if (view.mapOnly)
            {
                return;
            }

            entity.SetView(m_factory.Create(view.type, view.name));
        }
        
        private void OnWorldEntityRemoved(Coe.GameWorld world, Coe.GameEntity entity)
        {}

        private void OnMapEntityAdded(Map.GameMap map, Core.Ident eid, GG.Mathe.Vec2i where)
        {
            Coe.GameEntity entity = Context.World.Find(eid);
            if (entity.ContainsView())
            {
                return;
            }

            var view = entity.FindFirstComponent<Game.Comp.View>();
            if (view == null)
            {
                return;
            }

            entity.SetView(m_factory.Create(view.type, view.name));
        }

        private void OnMapEntityRemoved(Map.GameMap map, Core.Ident eid, GG.Mathe.Vec2i where)
        {
            Coe.GameEntity entity = Context.World.Find(eid);
            entity.RemoveView();
        }

        #region @@@ SPRITES @@@

        /// <summary>
        /// Try to get a sprite.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="sprite">Sprite.</param>
        /// <returns>True if the sprite exists; otherwise, null.</returns>
        public bool TryGetSprite(string name, out Sprite sprite)
        {
            sprite = null;
            foreach (SpriteTable table in m_sprites)
            {
                if (table.TryGet(name, out sprite))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region @@@ UNITY LIFE CYCLE @@@

        private void Awake()
        {
            m_factory = new ViewFactory(this, m_views);
            // Link the factory to the entity system.
            Coe.GameViewUtil.AddFactory(m_factory);
        }

        #endregion
    }
}
