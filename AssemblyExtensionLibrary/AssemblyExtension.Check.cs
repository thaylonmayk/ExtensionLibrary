using System.Diagnostics;
using System.Reflection;

namespace AssemblyExtensionLibrary
{
    public static partial class AssemblyExtension
    {
        /// <summary>
        /// Verifica se o assembly foi compilado em modo debug.
        /// </summary>
        /// <param name="assembly">O assembly para verificar.</param>
        /// <returns>True se o assembly foi compilado em modo debug; caso contrário, False.</returns>
        public static bool IsDebugBuild(this Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);
            if (attributes.Length == 0)
            {
                return false;
            }
            var debuggableAttribute = (DebuggableAttribute)attributes[0];
            return debuggableAttribute.IsJITTrackingEnabled;
        }

    }
}
