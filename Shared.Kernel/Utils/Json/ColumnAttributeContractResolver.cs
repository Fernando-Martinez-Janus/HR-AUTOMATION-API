using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Shared.Kernel.Utils.Json
{
    /// <summary>
    /// Custom JSON contract resolver that maps JSON property names using
    /// <see cref="ColumnAttribute"/> definitions.
    /// 
    /// This allows the same model metadata used for database column mapping
    /// to also be reused when serializing and deserializing JSON payloads.
    /// </summary>
    public sealed class ColumnAttributeContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates a JSON property definition for a given class member.
        /// 
        /// If the member contains a <see cref="ColumnAttribute"/> with a valid name,
        /// that name is used as the JSON property name instead of the default
        /// property naming convention.
        /// </summary>
        /// <param name="member">
        /// The class member being converted into a JSON property.
        /// </param>
        /// <param name="memberSerialization">
        /// The serialization mode configured for the containing object.
        /// </param>
        /// <returns>
        /// A configured <see cref="JsonProperty"/> instance.
        /// </returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            ColumnAttribute? columnAttribute = member.GetCustomAttribute<ColumnAttribute>();

            if (!string.IsNullOrWhiteSpace(columnAttribute?.Name))
            {
                property.PropertyName = columnAttribute.Name;
            }

            return property;
        }
    }
}