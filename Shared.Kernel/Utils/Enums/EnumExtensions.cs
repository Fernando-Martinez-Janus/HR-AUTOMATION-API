using System.Reflection;

namespace Shared.Kernel.Utils.Enums
{
    /// <summary>
    /// Enum Utils.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves a custom attribute of the specified type applied to an enum field.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the attribute to retrieve.
        /// </typeparam>
        /// <param name="enumValue">
        /// The enum value from which the attribute is obtained.
        /// </param>
        /// <returns>
        /// The attribute instance if found; otherwise, <c>null</c>.
        /// </returns>
        private static T? GetCustomAttibute<T>(this Enum enumValue) where T : Attribute
        {
            FieldInfo? field = enumValue.GetType().GetField(enumValue.ToString());

            return field?.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Gets the numeric value associated with the enum.
        /// </summary>
        /// <param name="enumValue">
        /// The enum value.
        /// </param>
        /// <returns>
        /// The value defined in the <see cref="EnumInfo"/> if present;
        /// otherwise, the underlying numeric value of the enum.
        /// </returns>
        public static int GetValue(this Enum enumValue)
        {
            EnumInfo? attr = enumValue.GetCustomAttibute<EnumInfo>();

            return attr?.Value ?? Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// Gets the description associated with the enum.
        /// </summary>
        /// <param name="enumValue">
        /// The enum value.
        /// </param>
        /// <returns>
        /// The description defined in the <see cref="EnumInfo"/> if present;
        /// otherwise, the enum member name.
        /// </returns>
        public static string GetDescription(this Enum enumValue)
        {
            EnumInfo? attr = enumValue.GetCustomAttibute<EnumInfo>();

            return attr?.Description ?? enumValue.ToString();
        }
    }
}