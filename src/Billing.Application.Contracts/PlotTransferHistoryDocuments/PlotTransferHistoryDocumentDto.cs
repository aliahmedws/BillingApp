using Billing.FileAttachments;
using Billing.PlotDocuments;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotTransferHistoryDocuments;

public class PlotTransferHistoryDocumentDto : EntityDto<Guid>
{
    public Guid PlotTransferHistoryId { get; set; }
    public PlotHistoryDocumentType PlotHistoryDT { get; set; }
    public string? RegistryNo { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Remarks { get; set; }
    public bool IsVerified { get; set; }
    public FileAttachmentDto? FileAttachments { get; set; }
}
