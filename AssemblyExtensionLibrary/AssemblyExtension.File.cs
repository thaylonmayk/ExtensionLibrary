using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {
        /// <summary>
        /// Gets the FileInfo object for the specified assembly's location.
        /// </summary>
        /// <param name="assembly">The assembly to get the FileInfo for.</param>
        /// <returns>A FileInfo object representing the assembly's location.</returns>
        public static FileInfo FileInfo(this Assembly assembly) => new(assembly.Location);

        /// <summary>
        /// Gets a custom attribute of the specified type from the assembly.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute.</typeparam>
        /// <param name="assembly">The assembly to get the custom attribute from.</param>
        /// <returns>The custom attribute of type T if found; otherwise, null.</returns>
        public static T? GetCustomAttribute<T>(this Assembly assembly) where T : Attribute
        {
            return assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        /// <summary>
        /// Gets the entry assembly (the process executable) in the current application.
        /// </summary>
        /// <returns>The entry assembly.</returns>
        public static Assembly GetEntryAssembly() => Assembly.GetEntryAssembly();


    }
}
