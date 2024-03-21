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
        /// List of flyweights.
        /// </summary>
        private readonly GameComponentList m_flyweights;

        /// <summary>
        /// View.
        /// </summary>
        private IGameView m_view = null;

        #region @@@ CREATION @@@

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="world">World.</param>
        internal GameEntity(GameWorld world)
        {
            World = world;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="world">World.</param>
        /// <param name="template">Template.</param>
        internal GameEntity(GameWorld world, Template template)
        {
            World        = world;
            Template     = template;
            m_flyweights = world.GetTemplateFlyweights(template.Name);

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

        #region @@@ COMMON INTERFACE FOR COMPONENTS AND FLYWEIGHTS @@@

        public T FindFirstAny<T>() where T : IGameComponent
        {
            T c;

            if ((c = FindFirstComponent<T>()) != null) { return c; }
            if ((c = FindFirstFlyweight<T>()) != null) { return c; }

            return c;
        }

        #endregion

        #region @@@ COMPONENTS @@@

        public bool ContainsComponent<T>() where T : IGameComponent => m_components.Contains<T>();

        public T FindComponent<T>(int nth) where T : IGameComponent => m_components.Find<T>(nth);

        public T FindFirstComponent<T>() where T : IGameComponent => m_components.FindFirst<T>();

        public T FindLastComponent<T>() where T : IGameComponent => m_components.FindLast<T>();

        public void AddComponent<T>() where T : IGameComponent, new() =>  AddComponent(new T());

        public void AddComponent(IGameComponent component) 
        {
            IGameComponent gc = m_components.Add(component);
            if (gc != null)
            {
                World.OnComponentAdded(this, gc);
            }
        }

        public void RemoveComponent<T>(int nth)
        {
            IGameComponent gc = m_components.Remove<T>(nth);
            if (gc != null)
            {
                World.OnComponentRemoved(this, gc);
            }
        }

        public void RemoveAllComponents<T>()
        {
            m_components.RemoveAll<T>(gc => World.OnComponentRemoved(this, gc));
        }

        #endregion

        #region @@@ FLYWEIGHTS @@@

        public bool ContainsFlyweight<T>() where T : IGameComponent => m_flyweights != null && m_flyweights.Contains<T>();

        public T FindFlyweight<T>(int nth) where T : IGameComponent => m_flyweights != null ? m_flyweights.Find<T>(nth) : default;

        public T FindFirstFlyweight<T>() where T : IGameComponent => m_flyweights != null ? m_flyweights.FindFirst<T>() : default;

        #endregion

        #region @@@ BEHAVIOURS @@@

        /// <summary>
        /// Checks if the entity contains a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to check.</typeparam>
        /// <returns>True if the entity contains the behaviour; otherwise, null.</returns>
        public bool ContainsBehaviour<T>() where T : IGameBehaviour => m_behaviours.Contains<T>();

        /// <summary>
        /// Finds a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to find.</typeparam>
        /// <returns>Reference to the behaviour  if it is found; otherwise, null.</returns>
        public T FindBehaviour<T>() where T : IGameBehaviour => m_behaviours.Find<T>();

        /// <summary>
        /// Adds a new behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to add.</typeparam>
        public void AddBehaviour<T>() where T : IGameBehaviour, new() => AddBehaviour(new T());

        /// <summary>
        /// Adds a behaviour.
        /// </summary>
        /// <param name="behaviour">Behaviour to add.</param>
        public void AddBehaviour(IGameBehaviour behaviour)
        {
            IGameBehaviour gb = m_behaviours.Add(behaviour);
            if (gb != null)
            {
                World.OnBehaviourAdded(this, gb);
            }
        }

        /// <summary>
        /// Removes a behaviour.
        /// </summary>
        /// <typeparam name="T">Type of behaviour to remove.</typeparam>
        public void RemoveBehaviour<T>()
        {
            IGameBehaviour gb = m_behaviours.Remove<T>();
            if (gb != null)
            {
                World.OnBehaviourRemoved(this, gb);
            }
        }

        #endregion

        #region @@@ VIEW @@@

        public bool ContainsView() => m_view != null;

        public void SetView(string type)
        {
            IGameView view = GameViewUtil.Create(type, null);

            SetView(view);
        }

        public void SetView(string type, string name)
        {
            IGameView view = GameViewUtil.Create(type, name);

            SetView(view);
        }

        public void SetView(IGameView view)
        {
            m_view = view;

            if (m_view != null && m_running)
            {
                m_view.OnSetup(World, this);
            }
        }

        public void RemoveView()
        {
            if (m_view != null)
            {
                m_view.OnQuit();
            }

            m_view = null;
        }

        #endregion

        private void Populate(Template template)
        {
            // Components.
            for (int i = 0; i < template.ComponentCount; i++) { AddComponent(template.CloneComponent(i)); }
            for (int i = 0; i < template.BehaviourCount; i++) { AddBehaviour(template.CloneBehaviour(i)); }
        }
    }
}
