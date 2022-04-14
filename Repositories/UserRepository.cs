using Todo.Models;
using Dapper;
using Todo.Utilities;

namespace Todo.Repositories;

public interface IUserRepository
{
    Task<User> GetByUsername(string Username);
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<User> GetByUsername(string Username)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
        WHERE username = @Username";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Username });
    }
}