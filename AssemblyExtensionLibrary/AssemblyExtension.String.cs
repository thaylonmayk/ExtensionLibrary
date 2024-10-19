using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {
        /// <summary>
        /// Obtém o nome do assembly.
        /// </summary>
        /// <param name="assembly">O assembly para obter o nome.</param>
        /// <returns>O nome do assembly.</returns>
        public static string GetName(this Assembly assembly) =>
            assembly.GetName().Name;

        /// <summary>
        /// Obtém a localização do assembly.
        /// </summary>
        /// <param name="assembly">O assembly para obter a localização.</param>
        /// <returns>O caminho do arquivo onde o assembly está localizado.</returns>
        public static string GetLocation(this Assembly assembly) =>
            assembly.Location;



    }
}
