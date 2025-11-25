using System;
using Volo.Abp.Application.Dtos;

namespace Billing.FileAttachments;

public class FileAttachmentDto
{
    public string Name { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
}
