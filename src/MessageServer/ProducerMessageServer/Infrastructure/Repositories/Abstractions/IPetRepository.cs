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
    public Task<Pet> GetAsync(int id);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PetDto>> GetAllAsync();
    
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
    /// <returns></returns>
    public Task DeleteAsync(int id);
}