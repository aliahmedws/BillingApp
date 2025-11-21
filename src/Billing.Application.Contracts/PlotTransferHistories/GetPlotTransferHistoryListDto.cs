using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotTransferHistories;

public class GetPlotTransferHistoryListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? PlotId { get; set; }
    public Guid? FromConsumerId { get; set; }
    public Guid? ToConsumerId { get; set; }
    public DateTime? TransferDate { get; set; }
    public TransferType? TransferType { get; set; }
    public string? RegistryNo { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public TransferStatus? Status { get; set; }
}
