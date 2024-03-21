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
        private struct ComponentInfo
        {
            public IGameComponent value;

            public bool removed;
        }

        private struct BehaviourInfo
        {
            public string value;

            public bool removed;
        }

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
        private readonly List<TemplateComponent> m_components = new ();

        /// <summary>
        /// List of behaviours.
        /// </summary>
        [JsonProperty(PropertyName = "behaviours")]
        private readonly List<TemplateBehaviour> m_behaviours = new ();

        [JsonIgnore]
        private readonly List<ComponentInfo> m_compiledComponents = new ();

        [JsonIgnore]
        private readonly List<ComponentInfo> m_compiledFlyweights = new ();

        [JsonIgnore]
        private readonly List<BehaviourInfo> m_compiledBehaviours = new ();

        /// <summary>
        /// Number of components.
        /// </summary>
        [JsonIgnore]
        public int ComponentCount => m_compiledComponents.Count;

        [JsonIgnore]
        public int FlyweightCount => m_compiledFlyweights.Count;

        /// <summary>
        /// Number of behavours.
        /// </summary>
        [JsonIgnore]
        public int BehaviourCount => m_compiledBehaviours.Count;

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

            return (T)m_compiledComponents[i].value;
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
            int count = nth < 0 ? 1 : nth + 1;

            for (int i = 0; i < m_compiledComponents.Count; i++)
            {
                if (m_compiledComponents[i].value.GetType() != type || --count > 0)
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
        public IGameComponent GetComponent(int index) => m_compiledComponents[index].value;

        /// <summary>
        /// Clones a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the component.</returns>
        public IGameComponent CloneComponent(int index) => m_compiledComponents[index].value.Clone();
        /*
        /// <summary>
        /// Adds a new component.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public TemplateComponent AddComponent(IGameComponent component) 
        {
            var tc = TemplateComponent.Create(component, false);
            m_components.Add(tc);

            return tc;
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
        */
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
            int count = nth < 0 ? 1 : nth + 1;

            for (int i = 0; i < m_compiledFlyweights.Count; i++)
            {
                if (m_compiledFlyweights[i].value.GetType() != type || --count > 0)
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
        public IGameComponent GetFlyweight(int index) => m_compiledFlyweights[index].value;

        /// <summary>
        /// Clones a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the component.</returns>
        public IGameComponent CloneFlyweight(int index) => m_compiledFlyweights[index].value.Clone();

        /*
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
        */
        #endregion

        #region @@@ BEHAVIOURS @@@

        /// <summary>
        /// Finds the index of a behaviour of a specific type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Index of the behavour if it exists; otherwise, less than zero.</returns>
        public int FindBehaviourIndex(string type)
        {
            return m_compiledBehaviours.FindIndex(info => info.value == type);
        }

        /// <summary>
        /// Gets a component.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Component.</returns>
        public string GetBehaviour(int index) => m_compiledBehaviours[index].value;

        /// <summary>
        /// Clones a behaviour.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Clone of the behavour.</returns>
        public IGameBehaviour CloneBehaviour(int index) => GameBehaviourUtil.CreateFromName(m_compiledBehaviours[index].value);
        /*
        /// <summary>
        /// Adds a new behavour.
        /// </summary>
        /// <param name="type">Type.</param>
        public TemplateBehaviour AddBehaviour(string type) 
        {
            int index = FindBehaviourIndex(type);
            if (index < 0)
            {
                var tb = TemplateBehaviour.Create(type, false);
                m_behaviours.Add(tb);

                return tb;
            }
            else
            {
                return m_behaviours[index];
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
        */
        #endregion

        #region @@@ TEMPLATE MANIPULATION @@@

        public bool CompileNew(TemplateDatabase db, Action<Template> onCompiled = null)
        {
            if (Compiled)
            {
                return true;
            }

            Compiled = false;
            Template @base = null;

            if (!string.IsNullOrEmpty(Base))
            {
                if (!db.TryGet(Base, out @base) || !@base.Compiled)
                {
                    return false;
                }
            }
            // Copy the components and behaviours in the base template.
            if (@base != null)
            {
                foreach (ComponentInfo c in @base.m_compiledComponents) { m_compiledComponents.Add(c); }
                foreach (ComponentInfo f in @base.m_compiledFlyweights) { m_compiledFlyweights.Add(f); }
                foreach (BehaviourInfo b in @base.m_compiledBehaviours) { m_compiledBehaviours.Add(b); }
                //foreach (TemplateComponent tc in @base.m_components) { compiled.m_components.Add(tc.CloneAsInherited()); }
                //foreach (TemplateBehaviour tb in @base.m_behaviours) { compiled.m_behaviours.Add(tb.CloneAsInherited()); }
            }
            // Add the components and behaviour in this templates.
            foreach (TemplateComponent tc in m_components) { ApplyTemplateComponent(tc); }
            foreach (TemplateBehaviour tb in m_behaviours) { ApplyTemplateBehaviour(tb); }
            // Views are special as templates can contain only one view.
            // Clear the members marked as removed.
            m_compiledComponents.RemoveAll(info => info.removed);
            m_compiledBehaviours.RemoveAll(info => info.removed);
            Compiled = true;

            onCompiled?.Invoke(this);

            return true;
        }

        private void ApplyTemplateComponent(TemplateComponent tc)
        {
            if (tc.IsOverrideReplace)
            {
                int index;
                if (tc.IsFlyweight)
                {
                    index = FindFlyweightIndex(tc.component.GetType(), tc.OverrideIndex);
                }
                else
                { 
                    index = FindComponentIndex(tc.component.GetType(), tc.OverrideIndex);
                }

                if (index < 0)
                {
                    //m_components.Add(tc);
                    if (tc.IsFlyweight)
                    {
                        m_compiledFlyweights.Add(new ComponentInfo { value = tc.component });
                    }
                    else
                    {
                        m_compiledComponents.Add(new ComponentInfo { value = tc.component });
                    }
                }
                else
                {
                    IGameComponent source = tc.component;
                    IGameComponent target;
                    // Create a clone to override its values.
                    if (tc.IsFlyweight)
                    {
                        ComponentInfo copy = new ComponentInfo
                        {
                            value   = m_compiledFlyweights[index].value.Clone(),
                            removed = m_compiledFlyweights[index].removed
                        };

                        m_compiledFlyweights[index] = copy;
                        target = copy.value;
                    }
                    else
                    {
                        ComponentInfo copy = new ComponentInfo
                        {
                            value   = m_compiledComponents[index].value.Clone(),
                            removed = m_compiledComponents[index].removed
                        };

                        m_compiledComponents[index] = copy;
                        target = copy.value;
                    }
                    // Populate the inherited component with the values changed in the overriding template component.
                    GameComponentUtil.SetValues         (source, target, tc.changes);
                    GameComponentUtil.InvokeDeserialized(target);
                }

                return;
            }

            if (tc.IsOverrideRemove)
            {
                int index;
                if (tc.IsFlyweight)
                {
                    index = FindFlyweightIndex(tc.component.GetType(), tc.OverrideIndex);
                }
                else
                {
                    index = FindComponentIndex(tc.component.GetType(), tc.OverrideIndex);
                }

                if (index >= 0)
                {
                    if (tc.IsFlyweight)
                    {
                        var info = m_compiledFlyweights[index];
                        info.removed = true;
                        m_compiledFlyweights[index] = info;
                    }
                    else
                    {
                        var info = m_compiledComponents[index];
                        info.removed = true;
                        m_compiledComponents[index] = info;
                    }
                }

                return;
            }
            // If this point is reached, the template component is a simple addition.
            if (tc.IsFlyweight)
            {
                m_compiledFlyweights.Add(new ComponentInfo { value = tc.component });
            }
            else
            {
                m_compiledComponents.Add(new ComponentInfo { value = tc.component });
            }
        }

        private void ApplyTemplateBehaviour(TemplateBehaviour tb)
        {
            if (tb.IsOverrideReplace)
            {
                int index = FindBehaviourIndex(tb.behaviour);
                if (index < 0)
                {
                    m_compiledBehaviours.Add(new BehaviourInfo { value = tb.behaviour });
                }

                return;
            }

            if (tb.IsOverrideRemove)
            {
                int index = FindBehaviourIndex(tb.behaviour);
                if (index >= 0)
                {
                    var info = m_compiledBehaviours[index];
                    info.removed = true;
                    m_compiledBehaviours[index] = info;
                }

                return;
            }
            // If this point is reached, the template behaviour is a simple addition.
            m_compiledBehaviours.Add(new BehaviourInfo { value = tb.behaviour });
        }

        #endregion

        /*
        public TemplateComponent GetComponentInfo(int index) => m_components[index];

        public TemplateComponent GetFlyweightInfo(int index) => m_components[index];

        public TemplateBehaviour GetBehaviourInfo(int index) => m_behaviours[index];
        */
    }
}
