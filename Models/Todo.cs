using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Simple_db_crud.Models;

public class Todo
{
    public int Id { get; set; }

    [DisplayName("Title")]
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }

    [DisplayName("Completed")]
    public bool Done { get; set; }

    [DisplayName("Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Last Update")]
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}
