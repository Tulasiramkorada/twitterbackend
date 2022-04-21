using Twitter.Models;
using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface IUserRepository
{
    Task<User> GetUser(string Email);
    Task<User> Create(User Item);
    Task<bool> Update(User Item);
    Task<User> GetById(long Id);

}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }
    public async Task<User> Create(User item)
    {


        var query = $@"INSERT INTO ""{TableNames.user}""
        (full_name,password,email)
        VALUES (@FullName, @Password,@Email) RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, item);
            return res;
        }
    }

    public async Task<User> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" WHERE user_id = @UserId";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, new { UserId });
            return res;
        }
    }

    public async Task<User> GetUser(string Email)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" 
        WHERE Email = @Email";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
    }
    public async Task<bool> Update(User item)
    {
        var query = $@"UPDATE ""{TableNames.user}"" SET full_name = @FullName
         WHERE user_id = @UserId";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, item)) > 0;


    }
}
