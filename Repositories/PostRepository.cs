using Twitter.Models;
using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface IPostRepository
{

    Task<Post> Create(Post Data);

    Task<bool> Update(Post Data);
    Task<bool> Delete(long id);

    Task<List<Post>> GetAllPosts();

    Task<Post> GetById(long post_id);
    Task<List<Post>> GetPostByUserId(long currentUserId);
}
public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(IConfiguration config) : base(config)
    {

    }
    public async Task<Post> Create(Post Data)
    {

        var query = $@"INSERT INTO {TableNames.post}
        (title,user_id)
        VALUES (@Title,@UserId) RETURNING *";

        using (var con = NewConnection)

            return await con.QuerySingleOrDefaultAsync<Post>(query, Data);

    }

    public async Task<Post> GetById(int post_id)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE id = @PostId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, new { post_id });
    }


    public async Task<bool> Update(Post Data)
    {
        var query = $@"UPDATE {TableNames.post} SET title = @Title 
         WHERE post_id = @PostId";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, Data)) > 0;
    }
    public async Task<bool> Delete(long PostId)
    {
        var query = $@"DELETE FROM {TableNames.post} WHERE post_id = @PostId";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, new { PostId }) > 0);
    }

    public async Task<List<Post>> GetAllPosts()
    {
        var query = $@"SELECT * FROM {TableNames.post} ";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query)).AsList();
    }

    public async Task<Post> GetById(long Id)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE post_id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, new { Id });
    }

    public async Task<List<Post>> GetPostByUserId(long currentUserId)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE user_id = @UserId";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query, new { UserId = currentUserId })).AsList();

    }


}










