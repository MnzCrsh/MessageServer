using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IOwnerRepository
{
    /// <summary>
    /// Asynchronously creates new Owner in database
    /// </summary>
    /// <param name="owner">Owner entity data</param>
    /// <returns>Owner ID</returns>
    public Task<Guid> CreateAsync(Owner owner);
    
    /// <summary>
    /// Asynchronously returns Owner from data base
    /// </summary>
    /// <param name="id">ID of the existing Owner</param>
    /// <returns>Existing Owner</returns>
    public Task<Owner> GetAsync(int id);
    
    /// <summary>
    /// Asynchronously returns every owner from DB with linked pets
    /// </summary>
    /// <returns>IEnumerable with every Owner</returns>
    public Task<IEnumerable<OwnerDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously updates OLD Owner data to the NEW Owner data
    /// </summary>
    /// <param name="owner">New Owner data with the same ID</param>
    /// <returns></returns>
    public Task UpdateAsync(Owner owner);

    /// <summary>
    /// Delete owner from DB
    /// </summary>
    /// <param name="id">ID of the Owner to delete</param>
    /// <returns></returns>
    public Task DeleteAsync(int id);
}