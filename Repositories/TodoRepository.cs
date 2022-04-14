using Todo.Models;
using Dapper;
using Todo.Utilities;

namespace Todo.Repositories;

public interface ITodoRepository
{
    Task<TodoItem> Create(TodoItem Item);
    Task Update(TodoItem Item);
    Task Delete(int Id);
    Task<List<TodoItem>> GetAll();
    Task<TodoItem> GetById(int TodoId);
}

public class TodoRepository : BaseRepository, ITodoRepository
{
    public TodoRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<TodoItem> Create(TodoItem Item)
    {
        var query = $@"INSERT INTO {TableNames.todo} (title, user_id) 
        VALUES (@Title, @UserId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TodoItem>(query, Item);
    }

    public async Task Delete(int Id)
    {
        var query = $@"DELETE FROM {TableNames.todo} WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<TodoItem>> GetAll()
    {
        var query = $@"SELECT * FROM {TableNames.todo} ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<TodoItem>(query)).AsList();
    }

    public async Task<TodoItem> GetById(int TodoId)
    {
        var query = $@"SELECT * FROM {TableNames.todo} WHERE id = @TodoId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TodoItem>(query, new { TodoId });
    }

    public async Task Update(TodoItem Item)
    {
        var query = $@"UPDATE {TableNames.todo} SET title = @Title, 
        is_complete = @IsComplete WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }
}