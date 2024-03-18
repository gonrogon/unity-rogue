using UnityEngine;
using Rogue.Coe;

namespace Rogue.Views
{
    public abstract class ViewBase : MonoBehaviour, IGameView
    {
        /// <summary>
        /// View manager.
        /// </summary>
        protected ViewManager m_manager;

        /// <summary>
        /// Game world.
        /// </summary>
        protected GameWorld m_world;

        /// <summary>
        /// Entity linked to this view.
        /// </summary>
        protected GameEntity m_entity;

        /// <summary>
        /// View component.
        /// </summary>
        protected Game.Comp.View m_view = null;

        /// <summary>
        /// Location component.
        /// </summary>
        protected Game.Comp.Location m_location = null;

        /// <summary>
        /// Sets up the view.
        /// 
        /// Note that this method is called just after the instantiation of the view to set its references.
        /// </summary>
        /// <param name="manager">Manager.</param>
        public virtual void SetupView(ViewManager manager)
        {
            m_manager = manager;
        }

        #region @@@ GAME VIEW @@@

        public virtual bool OnSetup(GameWorld world, GameEntity entity)
        {
            m_world  = world;
            m_entity = entity;

            var name = GetEntityName();
            if (name != null)
            {
                gameObject.name = name;
            }

            m_view     = GetEntityViewComponent();
            m_location = GetEntityLocationComponent();

            return true;
        }

        public virtual void OnInit()
        {
            UpdateWorldPosition();
        }

        public virtual void OnQuit()
        {
            Destroy(gameObject);
        }

        public virtual void OnMessage(IGameMessage message) {}

        /// <summary>
        /// Updates the view position.
        /// </summary>
        public virtual void UpdateWorldPosition() {}

        #endregion

        #region @@@ UTILITIES @@@

        /// <summary>
        /// Get the name assigned to the entity in the name component.
        /// </summary>
        /// <returns>Name if the name component is present; otherwise, null.</returns>
        protected string GetEntityName()
        {
            var comp = m_entity.FindFirstComponent<Game.Comp.Name>();
            if (comp == null)
            {
                return null;
            }

            return comp.name;
        }

        /// <summary>
        /// Gets the view component.
        /// </summary>
        /// <returns>View component.</returns>
        protected Game.Comp.View GetEntityViewComponent()
        {
            return m_entity.FindFirstComponent<Game.Comp.View>();
        }

        /// <summary>
        /// Gets the location component.
        /// </summary>
        /// <returns>Location component.</returns>
        protected Game.Comp.Location GetEntityLocationComponent()
        {
            return m_entity.FindFirstComponent<Game.Comp.Location>();
        }

        #endregion
    }
}
