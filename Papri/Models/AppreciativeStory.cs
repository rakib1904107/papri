using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class AppreciativeStory
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Story is required.")]
    [Display(Name = "Story")]
    public string Story { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Image")]
    public string? ImagePath { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
