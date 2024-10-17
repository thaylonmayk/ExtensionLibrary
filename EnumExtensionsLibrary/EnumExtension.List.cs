using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumExtensionsLibrary
{
    public static partial class EnumExtension
    {

        /// <summary>
        /// Gets all values of the enum as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A list of enum values.</returns>
        public static IList<T> GetValues<T>() where T : Enum
        {
            Type e = typeof(T);
            if (!e.IsEnum)
            {
                throw new ArgumentException("T must be of type System.Enum");
            }
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Gets all names of the enum as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A list of enum names.</returns>
        public static IList<string> GetNames<T>() where T : Enum
        {
            Type e = typeof(T);
            if (!e.IsEnum)
            {
                throw new ArgumentException("T must be of type System.Enum");
            }
            return Enum.GetNames(typeof(T)).ToList();
        }

    }
}
