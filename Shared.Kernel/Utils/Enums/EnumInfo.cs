namespace Shared.Kernel.Utils.Enums
{
    /// <summary>
    /// Specifies metadata for an enum field, including a numeric value and a description.
    /// </summary>
    /// <param name="value">The numeric value associated with the enum field.</param>
    /// <param name="description">A description of the enum field.</param>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumInfo(int value, string description) : Attribute
    {
        /// <summary>
        /// Numeric value associated with the enum field.
        /// </summary>
        public int Value { get; set; } = value;

        /// <summary>
        /// Description associated with the enum field.
        /// </summary>
        public string Description { get; set; } = description;
    }
}