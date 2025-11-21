using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.PlotTransferHistories;

public class CreatePlotTransferHistoryDto
{
    [Required]
    public Guid PlotId { get; set; }

    [Required]
    public Guid FromConsumerId { get; set; }

    [Required]
    public Guid ToConsumerId { get; set; }

    [Required]
    public DateTime TransferDate { get; set; }

    [Required]
    public TransferType TransferType { get; set; }

    [Required]
    [StringLength(PlotTransferHistoryConsts.MaxRegistryNoLength)]
    public string RegistryNo { get; set; } = string.Empty;

    [StringLength(PlotTransferHistoryConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
    
    [StringLength(PlotTransferHistoryConsts.MaxRemarksLength)]
    public string? RejectionReason { get; set; }

    // Approval details — optional for creation
    public Guid? ApprovedByUserId { get; set; }
    public Guid? RejectByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public TransferStatus Status { get; set; } = TransferStatus.Pending;
}
