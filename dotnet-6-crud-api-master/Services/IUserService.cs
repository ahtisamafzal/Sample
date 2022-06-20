using WebApi.Models.Users;

namespace WebApi.Services;

public interface IUserService<T>
{
    Task<IEnumerable<T>> GetAllAsync(int count = -1, int skip = -1, string searchTerm = null, string sortBy = null);
    Task<IEnumerable<T>> GetAllDeletedAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(CreateRequest model);
    Task UpdateAsync(int id, UpdateRequest model);
    Task DeleteAsync(int id);
}