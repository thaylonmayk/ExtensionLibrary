using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Obtém os nomes dos recursos incorporados no assembly.
        /// </summary>
        /// <param name="assembly">O assembly para obter os nomes dos recursos.</param>
        /// <returns>Uma lista de nomes dos recursos incorporados no assembly.</returns>
        public static IEnumerable<string> GetManifestResourceNames(this Assembly assembly) => assembly.GetManifestResourceNames();

        /// <summary>
        /// Obtém os namespaces definidos no assembly.
        /// </summary>
        /// <param name="assembly">O assembly para obter os namespaces.</param>
        /// <returns>Uma lista de namespaces definidos no assembly.</returns>
        public static IEnumerable<string> GetDefinedNamespaces(this Assembly assembly) => assembly.GetTypes().Select(t => t.Namespace).Distinct();

    }
}
