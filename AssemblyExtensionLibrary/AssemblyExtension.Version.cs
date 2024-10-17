using System;
using System.Reflection;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {

        /// <summary>
        /// Gets the version of the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the version for.</param>
        /// <returns>The version of the assembly.</returns>
        public static Version GetVersion(this Assembly assembly) => assembly.GetName().Version;

    }
}
