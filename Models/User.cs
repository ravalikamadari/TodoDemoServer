namespace Todo.Models;

public record User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}