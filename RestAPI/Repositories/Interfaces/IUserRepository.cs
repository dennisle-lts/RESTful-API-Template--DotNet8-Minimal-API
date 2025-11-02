using RestAPI.Domain;

namespace RestAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<int> GetTotalCountAsync();
}
