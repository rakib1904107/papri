using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public enum NoticeType
{
    News = 0,
    Event = 1
}

public class Notice
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Type")]
    public NoticeType Type { get; set; } = NoticeType.News;

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Details")]
    public string? Body { get; set; }
}
