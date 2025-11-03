using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace Billing.FileAttachments;

public class FileAttachment : ValueObject
{
    public string Name { get; }
    public string BlobName { get; }
    public string Path { get; }
    public long SizeInBytes { get; }
    [NotMapped]
    public string Extension => System.IO.Path.GetExtension(BlobName);

    private FileAttachment() { }

    internal FileAttachment(
            string name,
            string blobName,
            string filePath,
            long size = 0)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: FileAttachmentConsts.MaxNameLength);
        BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName), maxLength: FileAttachmentConsts.MaxNameLength);
        Path = Check.NotNullOrWhiteSpace(filePath, nameof(filePath), maxLength: FileAttachmentConsts.MaxPathLength);
        SizeInBytes = Check.Range(size, nameof(size), 0, FileAttachmentConsts.MaxSizeBytes);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return BlobName;
        yield return Path;
        yield return SizeInBytes;
    }
}
