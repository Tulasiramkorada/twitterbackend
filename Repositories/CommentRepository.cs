using Dapper;
using Twitter.Models;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ICommentRepository
{
    Task<Comment> Create(Comment Item);
    Task<bool> Delete(long Id);
    Task<List<Comment>> GetAllComments(long PostId);
    Task<Comment> GetById(long Id);
}

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Comment> Create(Comment Item)
    {
        var query = $@"INSERT INTO {TableNames.comment} (comment_text,post_id,user_id)
        VALUES (@CommentText,@PostId, @UserId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, Item);
    }
    public async Task<bool> Delete(long Id)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)
            return await (con.ExecuteAsync(query, new { Id })) > 0;
    }

    public async Task<List<Comment>> GetAllComments(long PostId)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE post_id = @PostId";


        using (var con = NewConnection)
            return (await con.QueryAsync<Comment>(query, new { PostId })).AsList();
    }

    public async Task<Comment> GetById(long Id)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)

            return await con.QuerySingleOrDefaultAsync<Comment>(query, new { Id });
    }


}
