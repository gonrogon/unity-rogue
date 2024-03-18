using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rogue.Core;
using UnityEngine;

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

        /// <summary>
        /// Removes all the components that match the conditions defined by the specific predicate.
        /// </summary>
        /// <param name="pred">Predicate that defines the conditions of the elements to remove.</param>
        /// <return>The number of components removed from the template.</return>
        public int RemoveAllComponents(Func<TemplateComponent, bool> pred)
        {
            int count = 0;

            for (int i = 0; i < m_components.Count;)
            {
                if (pred(m_components[i]))
                {
                    ArrayUtil.RemoveAndSwap(m_components, i);
                    count++;
                }
                else
                {
                    i++;
                }
            }

            return count;
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
                if (m_components[i].IsOverrideRemove || m_components[i].IsFlyweight)
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
        /// Gets a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Component.</returns>
        public IGameComponent GetComponent(int index) => m_components[index].component;

        /// <summary>
        /// Clones a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the component.</returns>
        public IGameComponent CloneComponent(int index)
        {
            TemplateComponent tc = m_components[index];

            if (tc.IsOverrideRemove || tc.IsFlyweight)
            {
                return null;
            }

            return tc.component.Clone();
        }

        /// <summary>
        /// Adds a new component.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public TemplateComponent AddComponent(IGameComponent component) 
        {
            var tc = TemplateComponent.CreateNew(component);
            m_components.Add(tc);

            return tc;
        }

        public TemplateComponent OverrideComponent(Type type, TemplateOverride @override, int overrideIndex)
        {
            switch (@override)
            {
                case TemplateOverride.Replace:
                {
                    int index = FindComponentIndex(type, overrideIndex < 0 ? 0 : overrideIndex);
                    if (index < 0)
                    {
                        return AddComponent(null);
                    }
                    else
                    {
                        var tc = m_components[index];

                        if (tc.IsInherited)
                        {
                            //tc.component = tc.component.Clone();
                            tc.MarkAsOverrided();
                            tc.ClearInherited();
                        }

                        return tc;
                    }
                }

                case TemplateOverride.Remove:
                {
                    int index = FindComponentIndex(type, overrideIndex < 0 ? 0 : overrideIndex);
                    if (index < 0)
                    {
                        // Nothing to do, component is not preset.
                    }
                    else
                    {
                        var tc = m_components[index];

                        //tc.component = null;
                        tc.MarkAsRemoved();
                        tc.ClearInherited();
                    }

                    return null;
                }

                default:
                {
                    return AddComponent(null);
                }
            }
        }

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
                if (m_components[i].IsOverrideRemove || m_components[i].IsFlyweight)
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
                if (m_components[i].IsOverrideRemove || m_components[i].IsFlyweight == false)
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
        public TemplateComponent AddFlyweight(IGameComponent component)
        {
            var tb = TemplateComponent.CreateFlyweight(component, false);
            m_components.Add(tb);

            return tb;
        }

        public TemplateComponent OverrideFlyweight(Type type, TemplateOverride @override, int overrideIndex)
        {
            switch (@override)
            {
                case TemplateOverride.Replace:
                {
                    int index = FindFlyweightIndex(type, overrideIndex < 0 ? 0 : overrideIndex);
                    if (index < 0)
                    {
                        return AddFlyweight(null);
                    }
                    else
                    {
                        var tc = m_components[index];

                        if (tc.IsInherited)
                        {
                            //tc.component = tc.component.Clone();
                            tc.MarkAsOverrided();
                            tc.ClearInherited();
                        }

                        return tc;
                    }
                }

                case TemplateOverride.Remove:
                {
                    int index = FindFlyweightIndex(type, overrideIndex < 0 ? 0 : overrideIndex);
                    if (index < 0)
                    {
                        // Nothing to do, component is not preset.
                    }
                    else
                    {
                        var tc = m_components[index];

                        //tc.component = null;
                        tc.MarkAsRemoved();
                        tc.ClearInherited();
                    }

                    return null;
                }

                default:
                {
                    return AddFlyweight(null);
                }
            }
        }

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
                if (m_components[i].IsOverrideRemove || m_components[i].IsFlyweight == false)
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
        public int FindBehaviourIndex(string type) => m_behaviours.FindIndex(item => item.behaviour == type);

        /// <summary>
        /// Clones a behaviour.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the behavour.</returns>
        public IGameBehaviour CloneBehaviour(int index) => GameBehaviourUtil.CreateFromName(m_behaviours[index].behaviour);

        /// <summary>
        /// Adds a new behavour.
        /// </summary>
        /// <param name="type">Type.</param>
        public TemplateBehaviour AddBehaviour(string type) 
        {
            int index = FindBehaviourIndex(type);
            if (index < 0)
            {
                var tb = TemplateBehaviour.CreateNew(type);
                m_behaviours.Add(tb);

                return tb;
            }
            else
            {
                return m_behaviours[index];
            }
        }
        
        public TemplateBehaviour OverrideBehaviour(string type, TemplateOverride @override)
        {
            switch (@override)
            {
                case TemplateOverride.Replace:
                {
                    return AddBehaviour(type);
                }

                case TemplateOverride.Remove:
                {
                    int index = FindBehaviourIndex(type);
                    if (index < 0)
                    {
                        // Nothing to do, behaviour is not preset.
                    }
                    else
                    {
                        var tb = m_behaviours[index];

                        tb.behaviour = null;
                        tb.Inherited = false;
                    }

                    return null;
                }

                default:
                {
                    return AddBehaviour(type);
                }
            }
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

        /// <summary>
        /// Removes all the components that match the conditions defined by the specific predicate.
        /// </summary>
        /// <param name="pred">Predicate that defines the conditions of the elements to remove.</param>
        /// <return>The number of components removed from the template.</return>
        public int RemoveAllBehaviours(Func<TemplateBehaviour, bool> pred)
        {
            int count = 0;

            for (int i = 0; i < m_behaviours.Count;)
            {
                if (pred(m_behaviours[i]))
                {
                    ArrayUtil.RemoveAndSwap(m_behaviours, i);
                    count++;
                }
                else
                {
                    i++;
                }
            }

            return count;
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
                if (m_components[i].IsInherited)
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
            if (ComponentCount > 0 || BehaviourCount > 0 || m_view != null)
            {
                throw new InvalidOperationException($"Unable to extend template, template is not empty");
            }
            // Add the component in the base template as inherited components.
            foreach (TemplateComponent item in template.m_components)
            {
                if (item.IsFlyweight)
                {
                    m_components.Add(TemplateComponent.CreateFlyweight(item.component.Clone(), true));
                }
                else
                {
                    m_components.Add(TemplateComponent.CreateInherited(item.component.Clone()));
                }
            }
            // Add the behaviours in the base template as inherited behaviours.
            foreach (TemplateBehaviour item in template.m_behaviours)
            {
                m_behaviours.Add(TemplateBehaviour.CreateInherited(item.behaviour));
            }
            // View is inherited if a new view is not defined.
            if (m_view == null)
            {
                if (template.m_view != null)
                {
                    m_view = template.m_view.CloneAsInherited();
                }
            }
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
