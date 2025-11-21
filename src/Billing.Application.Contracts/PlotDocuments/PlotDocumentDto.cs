using Billing.FileAttachments;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotDocuments;

public class PlotDocumentDto : EntityDto<Guid>
{
    public Guid PlotInfoId { get; set; }
    public string? Description { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public PlotDocumentType PlotDocumentType { get; set; }
    public bool IsVerified { get; set; } = false;
    public FileAttachmentDto? FileAttachments { get; set; }

}
