using System.Collections.Generic;
using System.Reflection;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {

        /// <summary>
        /// Gets the assemblies referenced by the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get referenced assemblies for.</param>
        /// <returns>A list of referenced assemblies.</returns>
        public static IEnumerable<AssemblyName> GetReferencedAssemblies(this Assembly assembly) => assembly.GetReferencedAssemblies();

    }
}
