using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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

    /// <summary>
    /// Configures Newtonsoft.Json to use the database column name declared through
    /// <see cref="ColumnAttribute"/> when deserializing JSON into a model.
    ///
    /// When a property does not have a Column attribute, its name is converted to
    /// snake_case. For example, SkillLevelId is matched with skill_level_id.
    /// </summary>
    internal sealed class ColumnAttributeContractResolver : DefaultContractResolver
    {
        public ColumnAttributeContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy
            {
                // Apply snake_case to dictionary keys as well.
                ProcessDictionaryKeys = true,

                // Preserve explicitly configured names such as [JsonProperty].
                OverrideSpecifiedNames = false
            };
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            // Reuse the same [Column("...")] attribute employed by DbReaderMapper,
            // avoiding the need to duplicate the name with [JsonProperty].
            ColumnAttribute? columnAttribute =
                member.GetCustomAttribute<ColumnAttribute>();

            if (!string.IsNullOrWhiteSpace(columnAttribute?.Name))
            {
                property.PropertyName = columnAttribute.Name;
            }

            return property;
        }
    }

    // Converts a boxed value coming from the reader into the property's target type.
    // DbDataReader.GetValue already returns native CLR types, so most values only
    // need a cheap type check; the explicit cases below cover the types that
    // Convert.ChangeType cannot handle on its own.
    internal static class ValueConverter
    {
        // These settings are reused for every JSON value read from the database.
        // Newtonsoft creates the actual object graph recursively, including nested
        // objects, collections, dictionaries and their child elements.
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new ColumnAttributeContractResolver(),

            // Ignore JSON fields that do not exist in the destination model.
            MissingMemberHandling = MissingMemberHandling.Ignore,

            // Keep Newtonsoft's normal behavior for null properties.
            NullValueHandling = NullValueHandling.Include
        };

        public static object Convert(object value, Type targetType)
        {
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(targetType);

            // Keep this method safe if it is called directly with Nullable<T>.
            // PropertyBinding already unwraps nullable value types, but this avoids
            // depending exclusively on that implementation detail.
            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Fast path: the value is already the type we need (the common case).
            //
            // This also prevents a string property containing text such as "{}"
            // or "[]" from being unnecessarily deserialized.
            if (targetType.IsInstanceOfType(value))
            {
                return value;
            }

            // SQL queries that use FOR JSON, JSON_ARRAYAGG or equivalent functions
            // normally return the JSON document as a string. Other integrations may
            // already expose the value as JObject, JArray or another JToken.
            if (TryDeserializeJson(value, targetType, out object? jsonResult))
            {
                return jsonResult!;
            }

            if (targetType.IsEnum)
            {
                return value is string name ? Enum.Parse(targetType, name, ignoreCase: true) : Enum.ToObject(targetType, value);
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

        /// <summary>
        /// Attempts to deserialize values that contain a JSON object or JSON array.
        ///
        /// Newtonsoft.Json handles the conversion recursively, so this supports
        /// IEnumerable&lt;T&gt;, List&lt;T&gt;, nested models, dictionaries and other
        /// compatible object graphs without manually traversing every property.
        /// </summary>
        private static bool TryDeserializeJson(object value, Type targetType, out object? result)
        {
            result = null;

            try
            {
                switch (value)
                {
                    // Handles JObject, JArray, JValue and other Newtonsoft tokens.
                    case JToken token:
                        result = token.ToObject(
                            targetType,
                            JsonSerializer.Create(JsonSettings))
                            ?? throw new InvalidCastException(
                                $"The JSON value was deserialized as null for target type '{targetType.FullName}'."
                            );
                        return true;

                    // Database providers usually return JSON columns as strings.
                    // Only strings beginning with '{' or '[' are considered here,
                    // preventing normal scalar strings from entering Newtonsoft.
                    case string json when LooksLikeJson(json):
                        result = JsonConvert.DeserializeObject(json, targetType, JsonSettings)
                            ?? throw new InvalidCastException(
                                $"The JSON value was deserialized as null for target type '{targetType.FullName}'."
                            );
                        return true;

                    // The value is not recognized as a JSON document.
                    default:
                        return false;
                }
            }
            catch (JsonException exception)
            {
                throw new InvalidCastException($"Could not deserialize the JSON value into '{targetType.FullName}'.", exception);
            }
        }

        /// <summary>
        /// Performs a cheap check to determine whether a string appears to contain
        /// a JSON object or JSON array. Newtonsoft performs the full validation later.
        /// </summary>
        private static bool LooksLikeJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            int index = 0;

            // Skip leading spaces, tabs and line breaks without allocating a new string.
            while (index < value.Length && char.IsWhiteSpace(value[index]))
            {
                index++;
            }

            return index < value.Length && (value[index] == '{' || value[index] == '[');
        }
    }
}