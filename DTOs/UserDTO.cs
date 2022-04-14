using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.DTOs;

public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("username")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Username { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
