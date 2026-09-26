using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EnumExtensionsLibrary
{
    public static partial class EnumExtension
    {
        /// <summary>
        /// Gets the description attribute of the enum value with zero boxing allocation on .NET 8.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            return EnumMetadataCache<T>.GetDescription(enumValue);
        }

        /// <summary>
        /// Gets the description attribute of the untyped enum value on .NET 8.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value is null)
            {
                return default;
            }

            return EnumDescriptionCache.GetOrAdd(value, ResolveFallbackDescription);
        }

        private static string ResolveFallbackDescription(Enum val)
        {
            var type = val.GetType();
            var name = val.ToString();
            var field = type.GetField(name);
            if (field is null)
            {
                return name;
            }

            var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
            return attribute is null ? name : attribute.Description;
        }

        private static class EnumMetadataCache<TEnum> where TEnum : Enum
        {
            public static string GetDescription(TEnum value)
            {
                return EnumDescriptionCache<TEnum>.GetOrAdd(value, ResolveDescription);
            }

            private static string ResolveDescription(TEnum val)
            {
                var name = val.ToString();
                var field = typeof(TEnum).GetField(name);
                if (field is null)
                {
                    return name;
                }

                var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
                return attribute is null ? name : attribute.Description;
            }
        }
    }

    /// <summary>
    /// Thread-safe bounded cache with defensive capacity limit and direct reflection fallback.
    /// </summary>
    public static class EnumDescriptionCache
    {
        /// <summary>
        /// Default maximum capacity of the bounded cache.
        /// </summary>
        public const int DefaultCapacity = 1024;

        private static readonly object _syncRoot = new object();
        private static readonly ConcurrentDictionary<Enum, string> _cache = new ConcurrentDictionary<Enum, string>();
        private static int _maxCapacity = DefaultCapacity;

        /// <summary>
        /// Gets or sets the maximum capacity for the untyped description cache.
        /// </summary>
        public static int MaxCapacity
        {
            get => _maxCapacity;
            set => _maxCapacity = value > 0 ? value : DefaultCapacity;
        }

        /// <summary>
        /// Gets the current number of cached entries.
        /// </summary>
        public static int Count => _cache.Count;

        /// <summary>
        /// Clears all entries currently stored in the untyped cache.
        /// </summary>
        public static void Clear()
        {
            lock (_syncRoot)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Retrieves the description from cache or computes it using the provided factory.
        /// When cache capacity is exceeded, safely returns the computed description via fallback without adding to the cache.
        /// </summary>
        /// <param name="key">The enum key.</param>
        /// <param name="valueFactory">Factory to compute description when not present in cache.</param>
        /// <returns>The resolved enum description.</returns>
        public static string GetOrAdd(Enum key, Func<Enum, string> valueFactory)
        {
            if (key is null)
            {
                return default;
            }

            if (_cache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var resolvedValue = valueFactory(key);

            if (_cache.Count >= _maxCapacity)
            {
                return resolvedValue;
            }

            return TryAddEntry(key, resolvedValue);
        }

        private static string TryAddEntry(Enum key, string resolvedValue)
        {
            lock (_syncRoot)
            {
                if (_cache.TryGetValue(key, out var existing))
                {
                    return existing;
                }

                if (_cache.Count < _maxCapacity)
                {
                    _cache.TryAdd(key, resolvedValue);
                }

                return resolvedValue;
            }
        }
    }

    /// <summary>
    /// Thread-safe bounded cache for generic enum descriptions, ensuring zero boxing allocation on modern runtimes.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    public static class EnumDescriptionCache<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// Default maximum capacity of the bounded cache.
        /// </summary>
        public const int DefaultCapacity = 1024;

        private static readonly object _syncRoot = new object();
        private static readonly ConcurrentDictionary<TEnum, string> _cache = new ConcurrentDictionary<TEnum, string>();
        private static int _maxCapacity = DefaultCapacity;

        /// <summary>
        /// Gets or sets the maximum capacity for this enum type description cache.
        /// </summary>
        public static int MaxCapacity
        {
            get => _maxCapacity;
            set => _maxCapacity = value > 0 ? value : DefaultCapacity;
        }

        /// <summary>
        /// Gets the current number of cached entries for this enum type.
        /// </summary>
        public static int Count => _cache.Count;

        /// <summary>
        /// Clears all entries currently stored in the cache for this enum type.
        /// </summary>
        public static void Clear()
        {
            lock (_syncRoot)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Retrieves the description from cache or computes it using the provided factory.
        /// When cache capacity is exceeded, safely returns the computed description via fallback without adding to the cache.
        /// </summary>
        /// <param name="key">The enum key.</param>
        /// <param name="valueFactory">Factory to compute description when not present in cache.</param>
        /// <returns>The resolved enum description.</returns>
        public static string GetOrAdd(TEnum key, Func<TEnum, string> valueFactory)
        {
            if (_cache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var resolvedValue = valueFactory(key);

            if (_cache.Count >= _maxCapacity)
            {
                return resolvedValue;
            }

            return TryAddEntry(key, resolvedValue);
        }

        private static string TryAddEntry(TEnum key, string resolvedValue)
        {
            lock (_syncRoot)
            {
                if (_cache.TryGetValue(key, out var existing))
                {
                    return existing;
                }

                if (_cache.Count < _maxCapacity)
                {
                    _cache.TryAdd(key, resolvedValue);
                }

                return resolvedValue;
            }
        }
    }
}
