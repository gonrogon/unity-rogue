using System.Collections.Generic;
using UnityEngine;
using Rogue.Core;
using Rogue.Core.Collections;
using Assets.Scripts.Coe;

namespace Rogue.Coe
{
    public class GameWorld
    {
        /// <summary>
        /// Define a world item.
        /// </summary>
        private struct Item
        {
            public enum State
            {
                Wait,     // Entity is waiting for configuration.
                New,      // Entity is new.
                Ready,    // Entity is ready to initialize and run.
                Running,  // Entity is runnnig (alive or dead).
                Finished, // Entity is ready to be removed.
            }

            public State state;

            public GameEntity entity;

            public bool IsNew => state == State.New;

            public bool IsReady => state == State.Ready;

            public bool IsAlive => state == State.Running && entity.IsAlive;

            public bool IsDead => state == State.Running && !entity.IsAlive;

            public bool IsFinished => state == State.Finished;

            public Item(GameEntity entity)
            {
                this.state  = State.Wait;
                this.entity = entity;
            }

            public void SetReady() => state = State.Ready;

            public void SetRunning() => state = State.Running;

            public void SetFinished() => state = State.Finished;
        }

        public delegate void OnEntityAddedHandler(GameWorld world, GameEntity entity);

        public delegate void OnEntityRemovedHandler(GameWorld world, GameEntity entity);

        /// <summary>
        /// Callback invoked when a new entity is added to the world.
        /// </summary>
        public event OnEntityAddedHandler OnEntityAdded;

        /// <summary>
        /// Callback invoked when a entity is removed from the world.
        /// </summary>
        public event OnEntityRemovedHandler OnEntityRemoved;

        /// <summary>
        /// Templates database.
        /// </summary>
        private readonly TemplateDatabase m_templates = new ();

        /// <summary>
        /// Dictionary of ident/entity pairs. 
        /// </summary>
        private readonly IdentMap<Item> m_entities = new ();

        /// <summary>
        /// List with the idents of the entities created in the last step.
        /// </summary>
        private readonly List<Ident> m_created = new ();

        /// <summary>
        /// List with the idents of the dead entities.
        /// </summary>
        private readonly List<Ident> m_dead = new ();

        /// <summary>
        /// List of listeners.
        /// </summary>
        private readonly List<IGameWorldListerner> m_listeners = new ();

        #region @@@ LISTENERS @@@

        /// <summary>
        /// Adds a listener.
        /// </summary>
        /// <param name="listener">Listener to add.</param>
        public void AddListener(IGameWorldListerner listener) => m_listeners.Add(listener);

        /// <summary>
        /// Removes a listener.
        /// </summary>
        /// <param name="listener">Listener to remove.</param>
        public void RemoveListener(IGameWorldListerner listener) => m_listeners.Remove(listener);

        #endregion

        #region @@@ TEMPLATES @@@

        /// <summary>
        /// Finds a template.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Reference to the template on success; otherwise, null.</returns>
        public Template FindTemplate(string name) => m_templates.Find(name);

        /// <summary>
        /// Adds a template.
        /// </summary>
        /// <param name="template">Template to add.</param>
        public bool AddTemplate(Template template)
        {
            if (!template.Compile(m_templates))
            {
                Debug.LogError($"Unable to compile template \"{template.Name}\"");
                return false;
            }

            m_templates.Add(template);
            return true;
        }

        /// <summary>
        /// Loads templates from a file.
        /// </summary>
        /// <param name="file">File to load.</param>
        /// <returns>True on success; otherwise, false.</returns>
        /*
        public bool LoadTemplates(string file)
        {   
            if (!m_templates.Load(file))
            {
                return false;
            }

            m_templates.Compile();
            return true;
        }
        */

        public bool LoadTemplatesFromText(string text)
        {
            return m_templates.LoadFromText(text);
        }

        #endregion

        #region @@@ ENTITIES @@@

        /// <summary>
        /// Creates a empty entity.
        /// </summary>
        /// <returns>Reference to the entity.</returns>
        public GameEntity Create()
        {
            return Insert(new GameEntity(this));
        }

        /// <summary>
        /// Creates a new entity from a template.
        /// </summary>
        /// <param name="template">Template name.</param>
        /// <returns>Reference to the entity on success; otherwise, null.</returns>
        public GameEntity Create(string template)
        {
            Template tpl = FindTemplate(template);
            if (tpl == null)
            {
                Debug.LogError($"Template \"{template}\" not found");
                return null;
            }

            return Insert(new GameEntity(this, tpl));
        }

        /// <summary>
        /// Starts an entity which is pending of configuration.
        /// </summary>
        /// <param name="entity">Entity to start.</param>
        public void Start(GameEntity entity)
        {
            if (entity.World != this)
            {
                Debug.LogError($"Unable to start entity \"{entity.Id}\", entity does not belong to the world");
                return;
            }

            if (!m_entities.TryFind(entity.Id, out Item item))
            {
                Debug.LogError($"Entity \"{entity.Id}\" not found");
                return;
            }

            if (item.state != Item.State.Wait)
            {
                Debug.LogError($"Entity \"{entity.Id}\" is not waiting for configuration");
                return;
            }

            if (entity.Setup())
            {
                item.state = Item.State.New;
            }
            else
            {
                item.state = Item.State.Finished;
            }
        }

        /*
        public GameEntity Add(GameEntity entity)
        {
            if (entity.World != null)
            {
                Debug.LogError($"Entity \"{entity.Id}\" was already added to a world");
                return null;
            }

            //entity.Setup(this, )

            Ident ident = m_entities.Add(new Item(entity));

            return entity;

            //m_created.Add(ident);
            //entity.Setup(this, ident);

            //return ident;
        }
        */

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        /// <returns>Identifier assigned to the entity.</returns>
        private GameEntity Insert(GameEntity entity)
        {
            Ident id = m_entities.Add(new Item(entity));
                       m_created .Add(id);

            entity.Assign(id);
            return entity;
        }

        /// <summary>
        /// Checks if an entity exists an is alive.
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public bool Exists(Ident eid)
        {
            if (!m_entities.TryFind(eid, out Item item))
            {
                return false;
            }

            return item.entity != null;
        }

        /// <summary>
        /// Finds an entity.
        /// </summary>
        /// <param name="eid">Identifier.</param>
        /// <returns>Reference to the entity is it exists; otherwise, null.</returns>
        public GameEntity Find(Ident eid)
        {
            if (!m_entities.TryFind(eid, out Item item))
            {
                return null;
            }

            return item.entity;
        }

        #endregion

        #region @@@ MESSAGES @@@

        /// <summary>
        /// Sends a message to an entity.
        /// </summary>
        /// <typeparam name="T">Type of message.</typeparam>
        /// <param name="eid">Identifier.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Sent message.</returns>
        public T Send<T>(Ident eid, T message) where T : IGameMessage
        {
            if (!m_entities.TryFind(eid, out Item item))
            {
                return message;
            }

            return item.entity.Send(message);
        }

        #endregion

        #region @@@ LIFE CYCLE @@@

        /// <summary>
        /// Executes a step.
        /// </summary>
        /// <param name="time">Elapsed time, in seconds.</param>
        public void Step(float time)
        {
            Debug.Assert(m_dead.Count == 0);
            
            foreach (Ident id in m_created)
            {
                if (m_entities.TryFind(id, out Item item))
                {
                    item.SetReady();
                    m_entities.Overwrite(id, item);

                    OnEntityAdded?.Invoke(this, item.entity);
                    foreach (var listener in m_listeners)
                    {
                        listener.OnEntityAdded(this, item.entity);
                    }
                }
            }

            m_created.Clear();

            foreach (var pair in m_entities)
            {
                Ident id   = pair.Key;
                Item  item = pair.Value;

                if (item.IsReady)
                {
                    item.SetRunning();
                    item.entity.Init();
                }

                if (item.IsAlive)
                {
                    item.entity.Step(time);
                }

                if (item.IsDead)
                {
                    item.SetFinished();
                    item.entity.Quit();
                    // Note the entity as dead.
                    m_dead.Add(id);
                }
                
                m_entities.Overwrite(id, item);
            }

            RemoveDeadEntities();
        }

        private void RemoveDeadEntities()
        {
            foreach (Ident id in m_dead)
            {
                if (m_entities.TryFind(id, out Item item))
                {
                    OnEntityRemoved?.Invoke(this, item.entity);
                    foreach (var listener in m_listeners)
                    {
                        listener.OnEntityRemoved(this, item.entity);
                    }

                    m_entities.Release(id);
                }
            }

            m_dead.Clear();
        }

        #endregion

        #region @@@ WORLD ENTITY INTERFACE @@@

        internal void OnComponentAdded(GameEntity entity, IGameComponent component)
        { 
            foreach (var listener in m_listeners)
            {
                listener.OnComponentAdded(this, entity, component);
            }
        }

        internal void OnComponentRemoved(GameEntity entity, IGameComponent component)
        { 
            foreach (var listener in m_listeners)
            {
                listener.OnComponentRemoved(this, entity, component);
            }
        }

        internal void OnBehaviourAdded(GameEntity entity, IGameBehaviour behaviour)
        {
            foreach (var listener in m_listeners)
            {
                listener.OnBehaviourAdded(this, entity, behaviour);
            }
        }

        internal void OnBehaviourRemoved(GameEntity entity, IGameBehaviour behaviour)
        {
            foreach (var listener in m_listeners)
            {
                listener.OnBehaviourRemoved(this, entity, behaviour);
            }
        }

        #endregion 
    }
}
