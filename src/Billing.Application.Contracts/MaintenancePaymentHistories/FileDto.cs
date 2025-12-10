namespace Billing.MaintenancePaymentHistories;

public class FileDto
{
    public string FileName { get; set; }
    public byte[] FileBytes { get; set; }

    public FileDto(string fileName, byte[] fileBytes)
    {
        FileName = fileName;
        FileBytes = fileBytes;
    }
}
