using System;
using System.Collections.Generic;
using System.Text;

namespace EnumExtensionsLibrary
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        public int Key { get; }
        public string Description { get; }

        public EnumDescriptionAttribute(int key, string description)
        {
            Key = key;
            Description = description;
        }
    }

}
