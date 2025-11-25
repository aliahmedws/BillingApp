using Volo.Abp.BlobStoring;

namespace Billing.FileAttachments;

[BlobContainerName(FileContainerName)]
public class FileContainer
{
    public const string FileContainerName = "files";
}
