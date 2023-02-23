using System;
using System.Reflection;
using System.Collections.Generic;

namespace Rogue.Coe
{
    public static class GameBehaviourUtil
    {
        /// <summary>
        /// Cache of components.
        /// </summary>
        public static Dictionary<string, Type> mCache = new Dictionary<string, Type>();

        /// <summary>
        /// Create a behaviour from a component name.
        /// </summary>
        /// <param name="name">Name of the type of behaviour.</param>
        /// <returns>Behaviour if it exists; otherwise, null.</returns>
        public static IGameBehaviour CreateFromName(string name)
        {
            if (TryGetBehaviour(name, out Type type))
            {
                return CreateFromType(type);
            }

            return null;
        }

        /// <summary>
        /// Create a behaviour from a type of component.
        /// </summary>
        /// <param name="type">Type of behaviour.</param>
        /// <returns>Behaviour if it is a valid type; otherwise, null.</returns>
        public static IGameBehaviour CreateFromType(Type type)
        {
            if (!typeof(IGameBehaviour).IsAssignableFrom(type))
            {
                return null;
            }

            return (IGameBehaviour)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Try to get a behaviour type.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of behaviour.</param>
        /// <returns>True on success; otherwise, false.</returns>
        private static bool TryGetBehaviour(string name, out Type type)
        {
            type = null;

            if (!mCache.TryGetValue(name, out type))
            {
                if (TryGetBehaviourInDomain(name, out type))
                {
                    mCache.Add(name, type);
                }
            }

            return type != null;
        }

        /// <summary>
        /// Try to get a behaviour type from current domain.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of behaviour.</param>
        /// <returns>True on success; otherwise, false.</returns>
        private static bool TryGetBehaviourInDomain(string name, out Type type)
        {
            type = null;
            // Get the assemblies in the domain.
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            // Try to get the behaviour from any of the assemblies.
            foreach (var assembly in assemblies)
            {
                if (TryGetBehaviourInAssembly(assembly, name, out type))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to get a behaviour type from an assembly.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of behaviour.</param>
        /// <returns>True on success; otherwise, false.</returns>
        private static bool TryGetBehaviourInAssembly(Assembly assembly, string name, out Type type)
        {
            type = null;

            if (assembly == null)
            {
                return false;
            }

            Type[] types;
            // Try to the get the types defined in the assembly.
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }
            // Find a behaviour with the specified name.
            type = Array.Find(types, item => 
            {
                if (item != null && typeof(IGameBehaviour).IsAssignableFrom(item) && item.Name == name) 
                {
                    return true;
                }

                return false;
            });

            return type != null;
        }
    }
}
