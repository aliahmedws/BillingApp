using Billing.PlotDocuments;
using Billing.PlotTransferHistoryDocuments;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotTransferHistories;

public class PlotTransferHistoryDto : EntityDto<Guid>
{
    public Guid PlotId { get; set; }
    public Guid FromConsumerId { get; set; }
    public Guid ToConsumerId { get; set; }
    public DateTime TransferDate { get; set; }
    public TransferType TransferType { get; set; }
    public string RegistryNo { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public string? RejectionReason { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Guid? RejectByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public TransferStatus Status { get; set; } = TransferStatus.Pending;

    // Lookups
    public string? FromConsumerName { get; set; }
    public string? ToConsumerName { get; set; }
    public string? PlotNo { get; set; }
    public string? ApprovedByUserName { get; set; }
    public string? RejectByUserName { get; set; }
    public List<PlotTransferHistoryDocumentDto> PlotTransferHistoryDocuments { get; set; } = new();
}
