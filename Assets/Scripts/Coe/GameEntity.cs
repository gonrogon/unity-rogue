using Rogue.Core;

namespace Rogue.Coe
{
    public class GameEntity
    {
        /// <summary>
        /// World.
        /// </summary>
        public GameWorld World { get; private set; }

        /// <summary>
        /// Template used to create this entity.
        /// </summary>
        public Template Template { get; private set; }

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public Ident Id { get; private set; } = Ident.Zero;

        /// <summary>
        /// Flag indicating whether the entity is running or not.
        /// </summary>
        private bool m_running = false;

        /// <summary>
        /// Flag indicating whether the entity is alive or not.
        /// </summary>
        private bool m_alive = true;

        /// <summary>
        /// Flag indicating whether the entity is running or not.
        /// </summary>
        public bool IsRunning => m_running;

        /// <summary>
        /// Flag indicating whether the entity is alive or not.
        /// </summary>
        public bool IsAlive => m_alive;

        /// <summary>
        /// List of components.
        /// </summary>
        private readonly GameComponentList m_components = new ();

        /// <summary>
        /// List of behaviours.
        /// </summary>
        private readonly GameBehaviourList m_behaviours = new ();

        /// <summary>
        /// View.
        /// </summary>
        private IGameView m_view = null;

        #region @@@ CREATION @@@

        internal GameEntity(GameWorld world)
        {
            World = world;
        }

        internal GameEntity(GameWorld world, Template template)
        {
            World = world;

            Populate(template);
        }

        internal void Assign(Ident eid)
        {
            Id = eid;
        }

        public bool Setup()
        {
            m_running = true;

            foreach (var behavior in m_behaviours)
            {
                behavior.OnSetup(World, this);
            }

            if (m_view != null)
            {
                m_view.OnSetup(World, this);
            }

            return true;
        }

        #endregion

        // ----------
        // Life cycle
        // ----------

        public void Init()
        {
            foreach (var behaviour in m_behaviours)
            {
                behaviour.OnInit();
            }

            if (m_view != null)
            {
                m_view.OnInit();
            }
        }

        public void Quit()
        {
            if (m_view != null)
            {
                m_view.OnQuit();
            }

            foreach (var behaviour in m_behaviours)
            {
                behaviour.OnQuit();
            }
        }

        public void Step(float time)
        { }

        public void Kill()
        {
            m_alive = false;
        }

        // --------
        // Messages
        // --------

        public T Send<T>(T message) where T : IGameMessage
        {
            foreach (IGameBehaviour behaviour in m_behaviours)
            {
                if (behaviour.OnMessage(message) == GameMessageState.Consumed)
                {
                    break;
                }
            }

            if (m_view != null)
            {
                m_view.OnMessage(message);
            }

            return message;
        }

        #region @@@ COMPONENTS @@@

        public bool ContainsComponent<T>() where T : IGameComponent => m_components.Contains<T>();

        public T FindComponent<T>(int nth) where T : IGameComponent => m_components.Find<T>(nth);

        public T FindFirstComponent<T>() where T : IGameComponent => m_components.FindFirst<T>();

        public T FindLastComponent<T>() where T : IGameComponent => m_components.FindLast<T>();

        public void AddComponent<T>() where T : IGameComponent, new() => m_components.Add<T>();

        public void AddComponent(IGameComponent component) => m_components.Add(component);

        public void RemoveComponent<T>(int nth) => m_components.Remove<T>(nth);

        public void RemoveAllComponents<T>() => m_components.RemoveAll<T>();

        #endregion

        #region @@@ FLYWEIGHTS @@@

        public bool ContainsFlyweight<T>() where T : IGameComponent
        {
            return Template == null ? false : Template.FindFlyweightIndex<T>(0) >= 0;
        }

        public T FindFlyweight<T>(int nth) where T : IGameComponent
        {
            return Template == null ? default : Template.FindFlyweight<T>(nth);
        }

        public T FindFirstFlyweight<T>() where T : IGameComponent
        {
            return Template == null ? default : Template.FindFirstFlyweight<T>();
        }

        #endregion

        #region @@@ COMMON INTERFACE FOR COMPONENTS AND FLYWEIGHTS @@@

        public T FindFirstAny<T>() where T : IGameComponent
        {
            T c;

            if ((c = FindFirstComponent<T>()) != null) { return c; }
            if ((c = FindFirstFlyweight<T>()) != null) { return c; }

            return c;
        }

        #endregion

        // ----------
        // Behaviours
        // ----------

        public bool ContainsBehaviour<T>() where T : IGameBehaviour => m_behaviours.Contains<T>();

        public T FindBehaviour<T>() where T : IGameBehaviour => m_behaviours.Find<T>();

        public void AddBehaviour<T>() where T : IGameBehaviour, new() => m_behaviours.Add<T>();

        public void AddBehaviour(IGameBehaviour behaviour) => m_behaviours.Add(behaviour);

        public void RemoveBehaviour<T>() => m_behaviours.Remove<T>();

        // ----
        // View
        // ----

        public bool ContainsView() => m_view != null;

        public void SetView(IGameView view)
        {
            m_view = view;

            if (m_view != null && m_running)
            {
                m_view.OnSetup(World, this);
            }
        }

        // TODO: Remove
        public void SetView(string type)
        {
            IGameView view = GameViewUtil.Create(type, null);

            SetView(view);
        }

        // TODO: Remove
        public void SetView(string type, string name)
        {
            IGameView view = GameViewUtil.Create(type, name);

            SetView(view);
        }

        public void RemoveView()
        {
            if (m_view != null)
            {
                m_view.OnQuit();
            }

            m_view = null;
        }

        // ---------
        // Templates
        // ---------

        public void Populate(Template template)
        {
            Template = template;

            for (int i = 0; i < template.ComponentCount; i++)
            {
                IGameComponent componet = template.CloneComponent(i);
                if (componet == null)
                {
                    continue;
                }

                AddComponent(componet);
            }

            for (int i = 0; i < template.BehaviourCount; i++)
            {
                AddBehaviour(template.CloneBehaviour(i));
            }

            if (template.GetViewInfo() != null)
            {
                SetView(template.CloneView());
            }
        }
    }
}
