using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Video
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "YouTube video ID is required.")]
    [StringLength(50)]
    [Display(Name = "YouTube video ID")]
    public string YouTubeId { get; set; } = string.Empty;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
