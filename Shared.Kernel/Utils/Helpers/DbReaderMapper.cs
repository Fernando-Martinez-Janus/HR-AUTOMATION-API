using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace Shared.Kernel.Utils.Helpers
{
    /// <summary>
    /// Maps rows from any ADO.NET data reader (SQL Server, MySQL, PostgreSQL, ...)
    /// to strongly typed objects.
    ///
    /// Every provider exposes its reader as a <see cref="DbDataReader"/>, so the same
    /// mapper works regardless of the database engine. Use the built-in
    /// <c>[Column("name")]</c> attribute to map a property to a differently named
    /// column, and <c>[NotMapped]</c> to exclude a property from mapping.
    /// </summary>
    public static class DbReaderMapper
    {
        // Cached property/column metadata per type. Reflection runs once per type,
        // never once per row, which is the main optimization over the naive approach.
        private static readonly ConcurrentDictionary<Type, PropertyBinding[]> BindingCache = new();

        /// <summary>
        /// Creates a reusable mapper bound to the current reader's column layout.
        /// Column ordinals are resolved a single time here instead of on every row,
        /// so this is the recommended entry point when reading many rows.
        /// </summary>
        public static RowMapper<T> Bind<T>(DbDataReader reader) where T : new()
            => new(reader, GetBindings(typeof(T)));

        /// <summary>
        /// Convenience overload that maps the reader's current row in a single call.
        /// Prefer <see cref="Bind{T}"/> inside a read loop: this method re-resolves
        /// column ordinals on every call, whereas a bound mapper resolves them once.
        /// </summary>
        public static T Map<T>(DbDataReader reader) where T : new()
            => Bind<T>(reader).Map(reader);

        // Builds (and caches) the bindings for a type: one entry per public,
        // writable, non-ignored property, paired with the column name to look up.
        private static PropertyBinding[] GetBindings(Type type)
        {
            return BindingCache.GetOrAdd(type, static t =>
            {
                List<PropertyBinding> bindings = [];

                foreach (PropertyInfo property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Skip read-only properties and anything explicitly excluded.
                    if (!property.CanWrite || property.IsDefined(typeof(NotMappedAttribute)))
                    {
                        continue;
                    }

                    // Use the [Column("...")] name when present, otherwise the property name.
                    string columnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;

                    bindings.Add(new PropertyBinding(property, columnName));
                }

                return [.. bindings];
            });
        }
    }

    /// <summary>
    /// A mapper bound to one reader's column layout. Reuse it for the whole read
    /// loop; create a new one for each reader or result set.
    /// </summary>
    public sealed class RowMapper<T> where T : new()
    {
        private readonly PropertyBinding[] _bindings;

        // Column ordinal for each binding, or -1 when the column is absent.
        private readonly int[] _ordinals;

        internal RowMapper(DbDataReader reader, PropertyBinding[] bindings)
        {
            _bindings = bindings;
            _ordinals = new int[bindings.Length];

            // Resolve every column name to its ordinal exactly once.
            for (int i = 0; i < bindings.Length; i++)
            {
                _ordinals[i] = FindOrdinal(reader, bindings[i].ColumnName);
            }
        }

        /// <summary>Maps the reader's current row to a new <typeparamref name="T"/>.</summary>
        public T Map(DbDataReader reader)
        {
            T item = new();

            for (int i = 0; i < _bindings.Length; i++)
            {
                int ordinal = _ordinals[i];

                // Column not present in the result set, or the value is NULL:
                // leave the property at its default value.
                if (ordinal < 0 || reader.IsDBNull(ordinal))
                {
                    continue;
                }

                object raw = reader.GetValue(ordinal);
                object value = ValueConverter.Convert(raw, _bindings[i].TargetType);
                _bindings[i].Property.SetValue(item, value);
            }

            return item;
        }

        // Resolves a column name to its ordinal. We return -1 instead of throwing
        // (like GetOrdinal does) so partial result sets simply skip missing columns.
        private static int FindOrdinal(DbDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }

    // Immutable description of how a single property is filled from the reader.
    internal sealed class PropertyBinding(PropertyInfo property, string columnName)
    {
        public PropertyInfo Property { get; } = property;
        public string ColumnName { get; } = columnName;
        public Type TargetType { get; } = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
    }

    // Converts a boxed value coming from the reader into the property's target type.
    // DbDataReader.GetValue already returns native CLR types, so most values only
    // need a cheap type check; the explicit cases below cover the types that
    // Convert.ChangeType cannot handle on its own.
    internal static class ValueConverter
    {
        public static object Convert(object value, Type targetType)
        {
            // Fast path: the value is already the type we need (the common case).
            if (targetType.IsInstanceOfType(value))
            {
                return value;
            }

            if (targetType.IsEnum)
            {
                return value is string name
                    ? Enum.Parse(targetType, name, ignoreCase: true)
                    : Enum.ToObject(targetType, value);
            }

            if (targetType == typeof(Guid))
            {
                return value is Guid guid ? guid : Guid.Parse(value.ToString()!);
            }

            if (targetType == typeof(DateOnly))
            {
                return DateOnly.FromDateTime(System.Convert.ToDateTime(value, CultureInfo.InvariantCulture));
            }

            if (targetType == typeof(TimeOnly))
            {
                return value is TimeSpan span
                    ? TimeOnly.FromTimeSpan(span)
                    : TimeOnly.FromDateTime(System.Convert.ToDateTime(value, CultureInfo.InvariantCulture));
            }

            if (targetType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value.ToString()!, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(System.Convert.ToDateTime(value, CultureInfo.InvariantCulture));
            }

            // Everything else (numeric types, bool, string, char, DateTime, ...) is
            // handled uniformly and culture-safely in a single line.
            return System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
    }
}
