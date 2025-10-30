using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Blocks;

public class CreateBlockDto
{
    [Required]
    [StringLength(BlockConsts.MaxBlockCodeLength)]
    public string BlockCode { get; set; } = string.Empty;

    [Required]
    [StringLength(BlockConsts.MaxBlockNameLength)]
    public string BlockName { get; set; } = string.Empty;

    [Required]
    public Guid PhaseId { get; set; }

    [StringLength(BlockConsts.MaxDescriptionLength)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
