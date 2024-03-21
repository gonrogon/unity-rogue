using Rogue.Coe;
using UnityEngine;

namespace Rogue.Views
{
    public class ViewManager : MonoBehaviour, IGameWorldListerner
    {
        /// <summary>
        /// Grid.
        /// </summary>
        [SerializeField]
        private Grid m_grid = null;

        /// <summary>
        /// List of views.
        /// </summary>
        [SerializeField]
        private ViewTable[] m_views = null;

        /// <summary>
        /// List of sprites.
        /// </summary>
        [SerializeField]
        private SpriteTable[] m_sprites = null;

        /// <summary>
        /// View factory.
        /// </summary>
        private ViewFactory m_factory = null;

        /// <summary>
        /// Gets the grid used to place the sprites.
        /// </summary>
        public Grid Grid => m_grid;

        private void Awake()
        {
            m_factory = new ViewFactory(this, m_views);
            // Link the factory to the entity system.
            GameViewUtil.AddFactory(m_factory);
        }

        public void Listen(GameWorld world, Map.GameMap map)
        {
            world.AddListener(this);

            map.OnEntityAdded   += OnMapEntityAdded;
            map.OnEntityRemoved += OnMapEntityRemoved;
        }

        #region @@@ LISTENERS @@@

        public void OnEntityAdded(GameWorld world, GameEntity entity) => SetView(entity);

        public void OnEntityRemoved(GameWorld world, GameEntity entity) => RemoveView(entity);

        public void OnComponentAdded(GameWorld world, GameEntity entity, IGameComponent component) {}

        public void OnComponentRemoved(GameWorld world, GameEntity entity, IGameComponent component) {}

        public void OnBehaviourAdded(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        public void OnBehaviourRemoved(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        public void OnMapEntityAdded(Map.GameMap map, Core.Ident eid, GG.Mathe.Vec2i where) => SetView(Context.World.Find(eid));

        public void OnMapEntityRemoved(Map.GameMap map, Core.Ident eid, GG.Mathe.Vec2i where) => RemoveView(Context.World.Find(eid));

        private void SetView(GameEntity entity)
        {
            if (entity.ContainsView())
            {
                return;
            }

            var view =  entity.FindFirstComponent<Game.Comp.View>();
            if (view == null)
            {
                return;
            }

            string type = string.IsNullOrEmpty(view.type) ? null : char.ToUpper(view.type[0]) + view.type[1..];
            string name = string.IsNullOrEmpty(view.name) ? null : char.ToUpper(view.name[0]) + view.name[1..];

            entity.SetView(type, name);
        }

        private void RemoveView(GameEntity entity)
        {
            entity.RemoveView();
        }

        #endregion

        #region @@@ SPRITES @@@

        /// <summary>
        /// Tries to get a sprite.
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
    }
}
