/// <summary>Defines the contract for seniority level business logic.</summary>
public interface ISeniorityLevelService
{
    /// <summary>Creates a new seniority level and returns a view model with the generated identifier.</summary>
    /// <param name="input">The validated input data for the new record.</param>
    /// <returns>A <see cref="SeniorityLevelViewModel"/> representing the created record.</returns>
    Task<SeniorityLevelViewModel> CreateAsync(SeniorityLevelInputModel input);

    /// <summary>Gets all active seniority levels for an organization with pagination.</summary>
    /// <param name="organizationId">The organization identifier.</param>
    /// <param name="rows_page">Number of rows per page.</param>
    /// <param name="page_number">Page number to retrieve.</param>
    /// <returns>A collection of seniority level view models.</returns>
    Task<IEnumerable<SeniorityLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);

    /// <summary>Gets a seniority level by its identifier.</summary>
    /// <param name="id">The seniority level identifier.</param>
    /// <returns>A view model if found; otherwise null.</returns>
    Task<SeniorityLevelViewModel?> GetByIdAsync(int id);

    /// <summary>Updates an existing seniority level.</summary>
    /// <param name="id">The identifier of the record to update.</param>
    /// <param name="input">The validated input data with the updated values.</param>
    /// <exception cref="ResponseExceptionFactory">Thrown when the record is not found.</exception>
    Task UpdateAsync(int id, SeniorityLevelInputModel input);

    /// <summary>Soft-deletes (disables) a seniority level.</summary>
    /// <param name="id">The identifier of the record to disable.</param>
    /// <param name="updatedBy">The identifier of the user performing the operation.</param>
    /// <exception cref="ResponseExceptionFactory">Thrown when the record is not found.</exception>
    Task DeleteAsync(int id, int updatedBy);
}
