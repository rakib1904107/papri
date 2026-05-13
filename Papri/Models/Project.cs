using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Project
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Details are required.")]
    public string Details { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Image")]
    public string? ImagePath { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
