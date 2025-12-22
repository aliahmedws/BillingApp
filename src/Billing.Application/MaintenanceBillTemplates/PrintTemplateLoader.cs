using System.IO;

namespace Billing.BillTemplates;

public static class PrintTemplateLoader
{
    public static string Load(string fileName)
        => LoadFromFolder("MaintenanceBillTemplates", fileName);

    public static string LoadFromFolder(string folderName, string fileName)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var filePath = Path.Combine(basePath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Template not found: {filePath}");
        }

        return File.ReadAllText(filePath);
    }
}
