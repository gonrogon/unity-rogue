using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    public class Template
    {
        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Base template.
        /// </summary>
        [JsonProperty(PropertyName = "extends")]
        public string Base { get; private set; } = string.Empty;

        /// <summary>
        /// List of component.
        /// </summary>
        [JsonProperty(PropertyName = "components")]
        private List<TemplateComponent> m_components = new ();

        /// <summary>
        /// List of behaviours.
        /// </summary>
        [JsonProperty(PropertyName = "behaviours")]
        private List<TemplateBehaviour> m_behaviours = new ();

        /// <summary>
        /// View.
        /// </summary>
        [JsonProperty(PropertyName = "view")]
        private TemplateView m_view = null;

        /// <summary>
        /// Number of components.
        /// </summary>
        [JsonIgnore]
        public int ComponentCount => m_components.Count;

        /// <summary>
        /// Number of behavours.
        /// </summary>
        [JsonIgnore]
        public int BehaviourCount => m_behaviours.Count;

        /// <summary>
        /// Flag indicating whether the template is compiled or not.
        /// </summary>
        [JsonIgnore]
        public bool Compiled { get; private set; } = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Template() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        public Template(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="extends">Base template.</param>
        public Template(string name, string extends)
        {
            Name = name;
            Base = extends;
        }

        #region @@@ COMPONENTS @@@

        /// <summary>
        /// Finds the first component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>Reference to the component if it exists; otherwise, null.</returns>
        public T FindFirstComponent<T>() where T : IGameComponent => FindComponent<T>(0);

        /// <summary>
        /// Finds the nth component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Reference to the component if it exists; otherwise, null.</returns>
        public T FindComponent<T>(int nth) where T : IGameComponent
        {
            int i = FindComponentIndex<T>(nth);
            if (i < 0)
            {
                return default;
            }

            return (T)m_components[i].component;
        }

        /// <summary>
        /// Finds the index of the nth component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the component if it exists; otherwise, less than zero.</returns>
        public int FindComponentIndex<T>(int nth) where T : IGameComponent => FindComponentIndex(typeof(T), nth);
        
        /// <summary>
        /// Finds the index of the nth component of a specific type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the component if it exists; otherwise, less than zero.</returns>
        public int FindComponentIndex(Type type, int nth)
        {
            int count = 0;

            for (int i = 0; i < m_components.Count; i++)
            {
                if (m_components[i].IsRemove || m_components[i].Flyweight)
                {
                    continue;
                }

                if (m_components[i].component.GetType() != type || nth != count++)
                {
                    continue;
                }

                return i;
            }

            return -1;
        }

        /// <summary>
        /// Clones a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the component.</returns>
        public IGameComponent CloneComponent(int index)
        {
            TemplateComponent tc = m_components[index];

            if (tc.IsRemove || tc.Flyweight)
            {
                return null;
            }

            return tc.component.Clone();
        }

        /// <summary>
        /// Adds a new component.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public void AddComponent(IGameComponent component) => m_components.Add(TemplateComponent.CreateNew(component));

        /// <summary>
        /// Removes the nth component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        public void RemoveComponent<T>(int nth) where T : IGameComponent
        {
            int i = FindComponentIndex<T>(nth);
            if (i < 0)
            {
                return;
            }

            m_components.RemoveAt(i);
        }

        /// <summary>
        /// Removes all the components of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        public void RemoveAllComponents<T>() where T : IGameComponent
        {
            for (int i = FindComponentIndex<T>(0); i >= 0; i = FindComponentIndex<T>(0))
            {
                if (m_components[i].IsRemove || m_components[i].Flyweight)
                {
                    continue;
                }

                m_components.RemoveAt(i);
            }
        }

        #endregion

        #region @@@ FLYWEIGHTS @@@

        /// <summary>
        /// Finds the first flyweight component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>Reference to the flyweight component if it exists; otherwise, null.</returns>
        public T FindFirstFlyweight<T>() where T : IGameComponent => FindFlyweight<T>(0);

        /// <summary>
        /// Finds the nth flyweight component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Reference to the flyweight component if it exists; otherwise, null.</returns>
        public T FindFlyweight<T>(int nth) where T : IGameComponent
        {
            int i = FindFlyweightIndex<T>(nth);
            if (i < 0)
            {
                return default;
            }

            return (T)m_components[i].component;
        }

        /// <summary>
        /// Finds the index of the nth flyweight component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the flyweight component if it exists; otherwise, less than zero.</returns>
        public int FindFlyweightIndex<T>(int nth) where T : IGameComponent => FindFlyweightIndex(typeof(T), nth);
        
        /// <summary>
        /// Finds the index of the nth flyweight component of a specific type.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <param name="nth">Number.</param>
        /// <returns>Index of the flyweight component if it exists; otherwise, less than zero.</returns>
        public int FindFlyweightIndex(Type type, int nth)
        {
            int count = 0;

            for (int i = 0; i < m_components.Count; i++)
            {
                if (m_components[i].IsRemove || m_components[i].Flyweight == false)
                {
                    continue;
                }

                if (m_components[i].component.GetType() != type || nth != count++)
                {
                    continue;
                }

                return i;
            }

            return -1;
        }

        /// <summary>
        /// Adds a new component.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public void AddFlyweight(IGameComponent component) => m_components.Add(TemplateComponent.CreateFlyweight(component, false));

        /// <summary>
        /// Removes the nth flyweight component of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        public void RemoveFlyweight<T>(int nth) where T : IGameComponent
        {
            int i = FindFlyweightIndex<T>(nth);
            if (i < 0)
            {
                return;
            }

            m_components.RemoveAt(i);
        }

        /// <summary>
        /// Removes all the flyweight components of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        public void RemoveAllFlyweights<T>() where T : IGameComponent
        {
            for (int i = FindComponentIndex<T>(0); i >= 0; i = FindComponentIndex<T>(0))
            {
                if (m_components[i].IsRemove || m_components[i].Flyweight == false)
                {
                    continue;
                }

                m_components.RemoveAt(i);
            }
        }

        #endregion

        #region @@@ BEHAVIOURS @@@

        /// <summary>
        /// Finds the index of a behaviour of a specific type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Index of the behavour if it exists; otherwise, less than zero.</returns>
        public int FindBehaviourIndex(string type) => m_behaviours.FindIndex(item => item.type == type);

        /// <summary>
        /// Clones a behaviour.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the behavour.</returns>
        public IGameBehaviour CloneBehaviour(int index) => GameBehaviourUtil.CreateFromName(m_behaviours[index].type);

        /// <summary>
        /// Adds a new behavour.
        /// </summary>
        /// <param name="type">Type.</param>
        public void AddBehaviour(string type) 
        {
            if (FindBehaviourIndex(type) >= 0)
            {
                return;
            }

            m_behaviours.Add(TemplateBehaviour.CreateNew(type));
        }
        
        /// <summary>
        /// Removes a behaviour.
        /// </summary>
        /// <param name="behaviour"></param>
        public void RemoveBehaviour(string behaviour)
        {
            int i = FindBehaviourIndex(behaviour);
            if (i < 0)
            {
                return;
            }

            m_behaviours.RemoveAt(i);
        }

        public bool IsBehaviourInherited(int i)
        {
            return m_behaviours[i].Inherited;
        }

        #endregion

        #region @@@ VIEW @@@

        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="type">Type.</param>
        public void SetView(string type) => m_view = TemplateView.CreateNew(type, null);

        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="name">Name.</param>
        public void SetView(string type, string name) => m_view = TemplateView.CreateNew(type, name);

        /// <summary>
        /// Clones the view.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the behavour.</returns>
        public IGameView CloneView() => GameViewUtil.Create(m_view.type, m_view.name);

        /// <summary>
        /// Removes the view.
        /// </summary>
        public void RemoveView() => m_view = null;

        #endregion

        #region @@@ COMPILATION @@@

        /// <summary>
        /// Compiles the template.
        /// </summary>
        /// <param name="db">Template database.</param>
        /// <param name="force">True to force the compilation of the template; otherwise, false.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Compile(TemplateDatabase db, bool force = false)
        {
            if (Compiled)
            {
                if (!force)
                {
                    return true;
                    
                }

                ClearCompilation();
            }

            Compiled = false;
            Template extend = null;

            if (!string.IsNullOrEmpty(Base))
            {
                if (!db.TryGet(Base, out Template tpl) || !tpl.Compiled)
                {
                    return false;
                }

                extend = tpl;
            }

            if (extend != null)
            {
                Extend(extend);
            }

            return Compiled = true;
        }

        /// <summary>
        /// Clears the compilation result.
        /// </summary>
        public void ClearCompilation()
        {
            for (int i = 0; i < m_components.Count; i++)
            {
                if (m_components[i].Inherited)
                {
                    m_components.RemoveAt(i);
                }
            }

            for (int i = 0; i < m_behaviours.Count; i++)
            {
                if (m_behaviours[i].Inherited)
                {
                    m_behaviours.RemoveAt(i);
                }
            }

            if (m_view.Inherited)
            {
                m_view = null;
            }

            Compiled = false;
        }

        /// <summary>
        /// Extends a template.
        /// </summary>
        /// <param name="template">Template to extend.</param>
        public void Extend(Template template)
        {
            // Creates a temporal template add all the components and behaviours.
            Template temp = new (Name);

            // ### COMPONENTS ###

            // Add the component in the base template as inherited components.
            foreach (TemplateComponent item in template.m_components)
            {
                if (item.Flyweight)
                {
                    temp.m_components.Add(TemplateComponent.CreateFlyweight(item.component, true));
                }
                else
                {
                    temp.m_components.Add(TemplateComponent.CreateInherited(item.component));
                }
            }
            // Try to add the components in this template.
            foreach (TemplateComponent item in m_components)
            {
                if (item.Flyweight)
                {
                    if (item.Override == TemplateOverride.None)
                    {
                        temp.m_components.Add(TemplateComponent.CreateFlyweight(item.component, false));
                    }
                    else
                    {
                        TemplateComponent overwrite;
                        // If the component is not found in the base template it is added as a new one.
                        int index = temp.FindFlyweightIndex(item.component.GetType(), item.OverrideIndex < 0 ? 0 : item.OverrideIndex);
                        if (index < 0)
                        {
                            overwrite = TemplateComponent.CreateFlyweight(item.component, false);
                            temp.m_components.Add(overwrite);
                        }
                        else
                        {
                            overwrite = temp.m_components[index];
                        }
                        // Overwrite the old component with the new one.
                        overwrite.Inherited         = false;
                        overwrite.Override          = item.Override;
                        overwrite.OverrideIndex     = item.OverrideIndex;
                        overwrite.component         = item.component;
                    }
                }
                else
                {
                    // If the component is not an overwrite it is added as a new one.
                    if (item.Override == TemplateOverride.None)
                    {
                        temp.AddComponent(item.component);
                    }
                    // Otherwise, find the component in the base template an overwrite it with its new values.
                    else
                    {
                        TemplateComponent overwrite;
                        // If the component is not found in the base template it is added as a new one.
                        int index = temp.FindComponentIndex(item.component.GetType(), item.OverrideIndex < 0 ? 0 : item.OverrideIndex);
                        if (index < 0)
                        {
                            overwrite = TemplateComponent.CreateNew(item.component);
                            temp.m_components.Add(overwrite);
                        }
                        else
                        {
                            overwrite = temp.m_components[index];
                        }
                        // Overwrite the old component with the new one.
                        overwrite.Inherited         = false;
                        overwrite.Override          = item.Override;
                        overwrite.OverrideIndex     = item.OverrideIndex;
                        overwrite.component         = item.component;
                    }
                }
            }

            // ### BEHAVIOURS ###

            // Add the behaviours in the base template as inherited behaviours.
            foreach (TemplateBehaviour item in template.m_behaviours)
            {
                temp.m_behaviours.Add(item.CloneAsInherited());
            }
            // Try to add the behaviours in this template.
            foreach (TemplateBehaviour item in m_behaviours)
            {
                // If the behaviour is not found in the base template it is added as a new one.
                int index = temp.FindBehaviourIndex(item.type);
                if (index < 0)
                {
                    temp.AddBehaviour(item.type);
                    continue;
                }
                // Overwrite the old behaviour with the new one.
                TemplateBehaviour overwrite = temp.m_behaviours[index];
                overwrite.Inherited         = false;
                overwrite.type              = item.type;
            }

            // ### VIEW ###

            // View is inherited if a new view is not defined.
            if (m_view == null)
            {
                if (template.m_view != null)
                {
                    temp.m_view = template.m_view.CloneAsInherited();
                }
            }
            else
            {
                temp.m_view = m_view;
            }

            // ### DONE ###

            // Swap components and behaviours in this template with the temporal one.
            (temp.m_components, m_components) = (m_components, temp.m_components);
            (temp.m_behaviours, m_behaviours) = (m_behaviours, temp.m_behaviours);
            (temp.m_view,       m_view)       = (m_view,       temp.m_view);
        }

        #endregion

        #region @@@ SERIALIZATION @@@

        public TemplateComponent GetComponentInfo(int index) => m_components[index];

        public TemplateComponent GetFlyweightInfo(int index) => m_components[index];

        public TemplateBehaviour GetBehaviourInfo(int index) => m_behaviours[index];

        public TemplateView GetViewInfo() => m_view;

        #endregion
    }
}
