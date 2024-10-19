using System;
using System.Reflection;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {

        /// <summary>
        /// Gets the types defined in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get types for.</param>
        /// <returns>An array of types defined in the assembly.</returns>
        public static Type[] GetTypes(this Assembly assembly) => assembly.GetTypes();

    }
}
