using Volo.Abp.Application.Dtos;

namespace Billing.FileAttachments;

public class FileAttachmentDto : EntityDto
{
    public string Name { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public byte[]? FileBytes { get; set; }
}
