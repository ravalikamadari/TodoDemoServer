using System.ComponentModel.DataAnnotations;

namespace Todo.DTOs;

public record TodoCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int UserId { get; set; }
}

public record TodoUpdateDTO
{
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; } = null;

    public bool? IsComplete { get; set; } = null;
}
