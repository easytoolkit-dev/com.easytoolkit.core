using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Defines flags for filtering assemblies by their type and origin.
    /// Multiple flags can be combined to retrieve assemblies from multiple categories.
    /// </summary>
    [Flags]
    public enum AssemblyCategory
    {
        /// <summary>
        /// No assemblies selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// User project assemblies that start with "Assembly." prefix.
        /// These are assemblies generated from the user's Unity project code.
        /// </summary>
        UserProject = 1 << 0,

        /// <summary>
        /// Unity Engine runtime assemblies that start with "UnityEngine." prefix.
        /// These contain the core Unity engine API and functionality.
        /// </summary>
        UnityEngine = 1 << 1,

        /// <summary>
        /// Unity Editor assemblies that start with "UnityEditor." prefix.
        /// These contain editor-specific functionality and are only available in the Unity Editor.
        /// </summary>
        UnityEditor = 1 << 2,

        /// <summary>
        /// General Unity framework assemblies that start with "Unity." prefix.
        /// This includes various Unity utility and framework assemblies.
        /// </summary>
        UnityPackages = 1 << 3,

        /// <summary>
        /// .NET System framework assemblies that start with "System." prefix.
        /// These contain the standard .NET framework classes and utilities.
        /// </summary>
        System = 1 << 4,

        /// <summary>
        /// Third-party assemblies that do not belong to any of the other categories.
        /// These include external libraries and plugins without standard Unity or .NET prefixes.
        /// </summary>
        ThirdParty = 1 << 5,

        /// <summary>
        /// Custom assemblies combining user project and third-party assemblies.
        /// This provides a convenient way to retrieve both user-defined and external libraries.
        /// </summary>
        Custom = UserProject | ThirdParty,

        /// <summary>
        /// All Unity-related assemblies (Engine, Editor, and Packages).
        /// </summary>
        UnityAll = UnityEngine | UnityEditor | UnityPackages,

        /// <summary>
        /// All assemblies currently loaded in the AppDomain.
        /// </summary>
        All = UserProject | UnityAll | System | ThirdParty
    }

    /// <summary>
    /// Provides utility methods for retrieving and filtering assemblies based on category flags.
    /// </summary>
    public static class AssemblyUtility
    {
        private static Assembly[] s_allAssemblies;
        private static readonly object Lock = new();

        static AssemblyUtility()
        {
            RefreshAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        }

        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            RefreshAssemblies();
        }

        /// <summary>
        /// Refreshes the cached assembly list by reloading all assemblies from the AppDomain.
        /// This method is automatically called when new assemblies are loaded.
        /// </summary>
        public static void RefreshAssemblies()
        {
            lock (Lock)
            {
                s_allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
        }

        /// <summary>
        /// Retrieves assemblies filtered by the specified category flags.
        /// </summary>
        /// <param name="categories">Category flags indicating which types of assemblies to retrieve.</param>
        /// <returns>
        /// A collection of assemblies matching the specified categories.
        /// Returns an empty collection if <see cref="AssemblyCategory.None"/> is specified.
        /// </returns>
        public static IEnumerable<Assembly> GetAssemblies(AssemblyCategory categories)
        {
            if (categories == AssemblyCategory.None)
            {
                yield break;
            }

            if (categories == AssemblyCategory.All)
            {
                Assembly[] assemblies;
                lock (Lock)
                {
                    assemblies = s_allAssemblies;
                }

                foreach (var assembly in assemblies)
                {
                    yield return assembly;
                }

                yield break;
            }

            if ((categories & AssemblyCategory.UserProject) != 0)
            {
                foreach (var assembly in GetUserProjectAssemblies())
                {
                    yield return assembly;
                }
            }

            if ((categories & AssemblyCategory.UnityEngine) != 0)
            {
                foreach (var assembly in GetUnityEngineAssemblies())
                {
                    yield return assembly;
                }
            }

            if ((categories & AssemblyCategory.UnityEditor) != 0)
            {
                foreach (var assembly in GetUnityEditorAssemblies())
                {
                    yield return assembly;
                }
            }

            if ((categories & AssemblyCategory.UnityPackages) != 0)
            {
                foreach (var assembly in GetUnityPackagesAssemblies())
                {
                    yield return assembly;
                }
            }

            if ((categories & AssemblyCategory.System) != 0)
            {
                foreach (var assembly in GetSystemAssemblies())
                {
                    yield return assembly;
                }
            }

            if ((categories & AssemblyCategory.ThirdParty) != 0)
            {
                foreach (var assembly in GetThirdPartyAssemblies())
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all assemblies except those matching the specified exclusion category flags.
        /// </summary>
        /// <param name="excludeCategories">Category flags indicating which types of assemblies to exclude.</param>
        /// <returns>
        /// A collection of assemblies not matching the excluded categories.
        /// Returns all assemblies if <see cref="AssemblyCategory.None"/> is specified.
        /// Returns an empty collection if <see cref="AssemblyCategory.All"/> is specified.
        /// </returns>
        public static IEnumerable<Assembly> GetAssembliesExcept(AssemblyCategory excludeCategories)
        {
            var includeCategories = AssemblyCategory.All & ~excludeCategories;
            return GetAssemblies(includeCategories);
        }

        /// <summary>
        /// Retrieves all user project assemblies that start with "Assembly." prefix.
        /// </summary>
        /// <returns>A collection of user project assemblies.</returns>
        public static IEnumerable<Assembly> GetUserProjectAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("Assembly."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all Unity Engine runtime assemblies that start with "UnityEngine." prefix.
        /// </summary>
        /// <returns>A collection of Unity Engine assemblies.</returns>
        public static IEnumerable<Assembly> GetUnityEngineAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("UnityEngine."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all Unity Editor assemblies that start with "UnityEditor." prefix.
        /// </summary>
        /// <returns>A collection of Unity Editor assemblies.</returns>
        /// <remarks>
        /// These assemblies are only available in the Unity Editor environment.
        /// </remarks>
        public static IEnumerable<Assembly> GetUnityEditorAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("UnityEditor."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all Unity package assemblies that start with "Unity." prefix.
        /// </summary>
        /// <returns>A collection of Unity package assemblies.</returns>
        public static IEnumerable<Assembly> GetUnityPackagesAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("Unity."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all .NET System framework assemblies that start with "System." prefix.
        /// </summary>
        /// <returns>A collection of System framework assemblies.</returns>
        public static IEnumerable<Assembly> GetSystemAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("System."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all third-party assemblies that do not belong to any standard category.
        /// </summary>
        /// <returns>A collection of third-party assemblies.</returns>
        /// <remarks>
        /// Third-party assemblies are those that do not start with any of the following prefixes:
        /// "Assembly.", "UnityEngine.", "UnityEditor.", "Unity.", or "System."
        /// </remarks>
        public static IEnumerable<Assembly> GetThirdPartyAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            foreach (var assembly in assemblies)
            {
                string fullName = assembly.FullName;
                if (!fullName.StartsWith("Assembly.") &&
                    !fullName.StartsWith("UnityEngine.") &&
                    !fullName.StartsWith("UnityEditor.") &&
                    !fullName.StartsWith("Unity.") &&
                    !fullName.StartsWith("System."))
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// Retrieves all assemblies currently loaded in the AppDomain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static Assembly[] GetAllAssemblies()
        {
            Assembly[] assemblies;
            lock (Lock)
            {
                assemblies = s_allAssemblies;
            }

            return assemblies;
        }

        /// <summary>
        /// Retrieves all types from assemblies filtered by the specified category flags.
        /// </summary>
        /// <param name="categories">Category flags indicating which types of assemblies to retrieve types from.</param>
        /// <returns>
        /// A collection of types from assemblies matching the specified categories.
        /// Returns an empty collection if <see cref="AssemblyCategory.None"/> is specified.
        /// </returns>
        public static IEnumerable<Type> GetTypes(AssemblyCategory categories)
        {
            foreach (var assembly in GetAssemblies(categories))
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip assemblies that fail to load types
                    continue;
                }

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Retrieves all types from assemblies except those matching the specified exclusion category flags.
        /// </summary>
        /// <param name="excludeCategories">Category flags indicating which types of assemblies to exclude.</param>
        /// <returns>
        /// A collection of types from assemblies not matching the excluded categories.
        /// Returns all types if <see cref="AssemblyCategory.None"/> is specified.
        /// Returns an empty collection if <see cref="AssemblyCategory.All"/> is specified.
        /// </returns>
        public static IEnumerable<Type> GetTypesExcept(AssemblyCategory excludeCategories)
        {
            return GetTypes(AssemblyCategory.All & ~excludeCategories);
        }
    }
}
