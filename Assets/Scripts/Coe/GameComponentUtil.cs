﻿using System;
using System.Reflection;
using System.Collections.Generic;

namespace Rogue.Coe
{
    public static class GameComponentUtil
    {
        /// <summary>
        /// Cache of components.
        /// </summary>
        public static Dictionary<string, Type> mCache = new ();

        /// <summary>
        /// Sets the value of a public field or property from a source component to target component.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="name">Name of the field or property.</param>
        /// <param name="source">Source component.</param>
        /// <param name="target">Target component.</param>
        public static void SetValue<T>(string name, T source, T target) where T : IGameComponent
        {
            MemberInfo[] found = typeof(T).GetMember(name, BindingFlags.Public);
            if (found.Length <= 0)
            {
                return;
            }

            foreach (MemberInfo info in found)
            {
                switch (info.MemberType)
                {
                    case MemberTypes.Field:    { ((FieldInfo)info)   .SetValue(target, ((FieldInfo)info)   .GetValue(source)); } return;
                    case MemberTypes.Property: { ((PropertyInfo)info).SetValue(target, ((PropertyInfo)info).GetValue(source)); } return;
                }
            }
        }

        /// <summary>
        /// Create a component from a component name.
        /// </summary>
        /// <param name="name">Name of the type of component.</param>
        /// <returns>Component if it exists; otherwise, null.</returns>
        public static IGameComponent CreateFromName(string name)
        {
            if (TryGetComponent(name, out Type type))
            {
                return CreateFromType(type);
            }

            return null;
        }

        /// <summary>
        /// Create a component from a type of component.
        /// </summary>
        /// <param name="type">Type of component.</param>
        /// <returns>Component if it is a valid type; otherwise, null.</returns>
        public static IGameComponent CreateFromType(Type type)
        {
            if (!typeof(IGameComponent).IsAssignableFrom(type))
            {
                return null;
            }

            return (IGameComponent)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to get a component type.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of component.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public static bool TryGetComponent(string name, out Type type)
        {
            if (!mCache.TryGetValue(name, out type))
            {
                if (TryGetComponentInDomain(name, out type))
                {
                    mCache.Add(name, type);
                }
            }

            return type != null;
        }

        /// <summary>
        /// Tries to get a component type from current domain.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of component.</param>
        /// <returns>True on success; otherwise, false.</returns>
        private static bool TryGetComponentInDomain(string name, out Type type)
        {
            type = null;
            // Get the assemblies in the domain.
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            // Try to get the component from any of the assemblies.
            foreach (var assembly in assemblies)
            {
                if (TryGetComponentInAssembly(assembly, name, out type))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get a component type from an assembly.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="name">Name.</param>
        /// <param name="type">Type of component.</param>
        /// <returns>True on success; otherwise, false.</returns>
        private static bool TryGetComponentInAssembly(Assembly assembly, string name, out Type type)
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
            // Find a component with the specified name.
            type = Array.Find(types, item => 
            {
                if (item != null && typeof(IGameComponent).IsAssignableFrom(item) && item.Name == name) 
                {
                    return true;
                }

                return false;
            });

            return type != null;
        }
    }
}
