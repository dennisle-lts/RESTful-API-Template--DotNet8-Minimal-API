using Dapper;
using RestAPI.Database;
using RestAPI.Domain;
using RestAPI.Repositories.Interfaces;

namespace RestAPI.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<User>> GetAllAsync(int limit, int offset)
    {
        const string sql =
            @"
            SELECT id, email, first_name, last_name, password_hash, phone, business_name,
                   email_verified, created_at, updated_at, is_active
            FROM user
            ORDER BY created_at DESC
            LIMIT @Limit OFFSET @Offset";

        using var connection = _dbConnectionFactory.CreateConnection();
        var users = await connection.QueryAsync<User>(sql, new { Limit = limit, Offset = offset });
        return [.. users]; // equal to users.ToList()
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT id, email, first_name, last_name, password_hash, phone, business_name, email_verified, created_at, updated_at, is_active FROM user WHERE id = @Id",
            new { Id = id }
        );
    }

    public async Task<User> CreateAsync(User user)
    {
        const string sql =
            @"
            INSERT INTO user (id, email, first_name, last_name, password_hash, phone, business_name, is_active, email_verified, created_at, updated_at)
            VALUES (@Id, @Email, @FirstName, @LastName, @PasswordHash, @Phone, @BusinessName, @IsActive, @EmailVerified, @CreatedAt, @UpdatedAt)";

        using var connection = _dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        const string sql =
            @"
            UPDATE user
            SET email = @Email,
                first_name = @FirstName,
                last_name = @LastName,
                password_hash = @PasswordHash,
                phone = @Phone,
                business_name = @BusinessName,
                is_active = @IsActive,
                email_verified = @EmailVerified,
                updated_at = @UpdatedAt
            WHERE id = @Id";

        using var connection = _dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<int> GetTotalCountAsync()
    {
        const string sql = "SELECT COUNT(1) FROM user";
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql);
    }
}
