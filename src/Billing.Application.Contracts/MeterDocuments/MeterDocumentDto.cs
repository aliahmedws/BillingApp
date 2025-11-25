using Billing.FileAttachments;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterDocuments;

public class MeterDocumentDto : EntityDto<Guid>
{
    public Guid MeterInfoId { get; set; }

    public string? Description { get; set; }

    public MeterDocumentType MeterDocumentType { get; set; }

    public FileAttachmentDto FileAttachments { get; set; }

}
