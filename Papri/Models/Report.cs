using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public class Report
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Display(Name = "File")]
    public string FilePath { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "File type")]
    public string FileType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
