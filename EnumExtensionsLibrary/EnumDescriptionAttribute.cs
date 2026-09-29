using System;
using System.Collections.Generic;
using System.Text;

namespace EnumExtensionsLibrary
{
    /// <summary>
    /// Associates a localized or custom numeric key and description with an enum member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Numeric identifier or localization key.
        /// </summary>
        public int Key { get; }

        /// <summary>
        /// Human-readable description text.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of EnumDescriptionAttribute.
        /// </summary>
        /// <param name="key">The numeric key.</param>
        /// <param name="description">The description string.</param>
        public EnumDescriptionAttribute(int key, string description)
        {
            Key = key;
            Description = description;
        }
    }

}
