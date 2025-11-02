using System.Data;

namespace RestAPI.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
