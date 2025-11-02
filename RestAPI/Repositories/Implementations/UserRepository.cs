using System.Data;
using Dapper;
using RestAPI.Domain;
using RestAPI.Repositories.Interfaces;

namespace RestAPI.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(IDbConnection db)
    {
        _db = db;
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

        var users = await _db.QueryAsync<User>(sql, new { Limit = limit, Offset = offset });
        return [.. users]; // equal to users.ToList()
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _db.QueryFirstOrDefaultAsync<User>(
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

        await _db.ExecuteAsync(sql, user);
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

        await _db.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<int> GetTotalCountAsync()
    {
        const string sql = "SELECT COUNT(1) FROM user";
        return await _db.ExecuteScalarAsync<int>(sql);
    }
}
