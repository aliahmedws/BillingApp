using System.ComponentModel.DataAnnotations;

namespace Billing.Phases;

public class UpdatePhaseDto
{
    [Required]
    [StringLength(PhaseConsts.MaxPhaseCodeLength)]
    public string PhaseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(PhaseConsts.MaxPhaseNameLength)]
    public string PhaseName { get; set; } = string.Empty;

    [StringLength(PhaseConsts.MaxDescriptionLength)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
