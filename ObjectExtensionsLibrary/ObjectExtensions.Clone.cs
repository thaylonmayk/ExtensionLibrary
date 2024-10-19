﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Creates a deep copy of the object using JSON serialization.
        /// </summary>
        /// <param name="obj">The object to clone.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T Clone<T>(this T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
