namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the data required to reorder an item.
    /// </summary>
    public class ReorderInputModel
    {
        /// <summary>
        /// Gets or sets the identifier of the item to reorder.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier, if the reorder operation
        /// is scoped to a specific organization.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the new sort order for the item.
        /// </summary>
        public int NewSortOrder { get; set; }
    }
}