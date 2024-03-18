using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    public struct TemplateReader
    {
        private readonly Template m_template;

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        public string Name => m_template.Name;

        /// <summary>
        /// Gets the name of the base template.
        /// </summary>
        public string Base => m_template.Base;

        /// <summary>
        /// Number of components.
        /// </summary>
        public int ComponentCount => m_template.ComponentCount;

        /// <summary>
        /// Number of behavours.
        /// </summary>
        public int BehaviourCount => m_template.BehaviourCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="template">Template to read.</param>
        public TemplateReader(Template template)
        {
            m_template = template;
        }

        #region @@@ COMPONENTS @@@

        public T FindFirstComponent<T>() where T : IGameComponent => m_template.FindFirstComponent<T>();

        public T FindComponent<T>(int nth) where T : IGameComponent => m_template.FindComponent<T>(nth);

        public int FindComponentIndex<T>(int nth) where T : IGameComponent => m_template.FindComponentIndex<T>(nth);
        
        public int FindComponentIndex(Type type, int nth) => m_template.FindComponentIndex(type, nth);

        public IGameComponent GetComponent(int index) => m_template.GetComponent(index);

        public IGameComponent CloneComponent(int index) => m_template.CloneComponent(index);

        #endregion

        #region @@@ FLYWEIGHTS @@@

        public T FindFirstFlyweight<T>() where T : IGameComponent => m_template.FindFirstFlyweight<T>();

        public T FindFlyweight<T>(int nth) where T : IGameComponent => m_template.FindFlyweight<T>(nth);

        public int FindFlyweightIndex<T>(int nth) where T : IGameComponent => m_template.FindFlyweightIndex<T>(nth);
        
        public int FindFlyweightIndex(Type type, int nth) => m_template.FindFlyweightIndex(type, nth);

        public T FindFirstAny<T>() where T : IGameComponent
        {
            T c;

            if ((c = FindFirstComponent<T>()) != null) { return c; }
            if ((c = FindFirstFlyweight<T>()) != null) { return c; }

            return default;
        }

        #endregion

        #region @@@ BEHAVIOURS @@@

        public int FindBehaviourIndex(string type) => m_template.FindBehaviourIndex(type);

        public IGameBehaviour CloneBehaviour(int index) => m_template.CloneBehaviour(index);

        #endregion
    }
}
