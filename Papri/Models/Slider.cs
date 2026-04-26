using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Slider
{
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    [Display(Name = "Image")]
    public string ImagePath { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Caption { get; set; }

    [Display(Name = "Display order")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
