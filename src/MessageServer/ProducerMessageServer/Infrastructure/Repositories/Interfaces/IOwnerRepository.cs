using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IOwnerRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    /// <returns></returns>
    public Task<Guid> CreateAsync(Owner owner);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Owner> GetAsync(int id);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<OwnerDto>> GetAllAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="updateCallback"></param>
    /// <returns></returns>
    public Task UpdateAsync(Owner owner, Action<Owner, Owner> updateCallback);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteAsync(int id);
}