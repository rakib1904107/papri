using System.ComponentModel.DataAnnotations;

namespace Papri.Models;

public enum DonorPartnerCategory
{
    [Display(Name = "Ongoing Donor / Partner")]
    Ongoing = 0,

    [Display(Name = "Previous Donor / Partner")]
    Previous = 1,

    [Display(Name = "Network Membership")]
    Network = 2
}

public class DonorPartner
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(300)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Category")]
    public DonorPartnerCategory Category { get; set; } = DonorPartnerCategory.Ongoing;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
