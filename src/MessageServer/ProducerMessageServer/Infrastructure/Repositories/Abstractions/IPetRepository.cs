using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Abstractions;

public interface IPetRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pet"></param>
    /// <returns></returns>
    public Task<Guid> CreateAsync(Pet pet);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Pet?> GetAsync(Guid id);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PetDto>> GetAllAsync(CancellationToken ct = default);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pet"></param>
    /// <returns></returns>
    public Task UpdateAsync(Pet pet);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="confirmDelete"></param>
    /// <returns></returns>
    public Task DeleteAsync(Guid id,bool confirmDelete = false);
}