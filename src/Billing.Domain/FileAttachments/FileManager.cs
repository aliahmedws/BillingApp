using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;

namespace Billing.FileAttachments;

public class FileManager(
    IBlobContainer<FileContainer> blobContainer,
    IConfiguration configuration) : DomainService
{
    private readonly IBlobContainer<FileContainer> _blobContainer = blobContainer;
    private readonly IConfiguration _configuration = configuration;

    public async Task<(string blobName, string url)> SaveAsync(byte[] fileBytes, string fileName, CancellationToken cancellationToken = default)
    {
        if (fileBytes == null || fileBytes.Length == 0)
        {
            throw new BusinessException(BillingDomainErrorCodes.EmptyFile);
        }

        var fileExtension = Path.GetExtension(fileName);
        // accepted format check
        if (string.IsNullOrWhiteSpace(fileExtension))
        {
            throw new BusinessException(BillingDomainErrorCodes.InvalidFileFormat);
        }

        var blobName = $"{Guid.NewGuid()}{fileExtension}";
        await _blobContainer.SaveAsync(blobName, fileBytes, overrideExisting: true, cancellationToken: cancellationToken);
        var url = BuildUrl(blobName);
        return (blobName, url);
    }

    public async Task<(string, string)> SaveAsync(MemoryStream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new BusinessException(BillingDomainErrorCodes.EmptyFile);
        }

        var fileExtension = Path.GetExtension(fileName);
        // accepted format check
        if (string.IsNullOrWhiteSpace(fileExtension))
        {
            throw new BusinessException(BillingDomainErrorCodes.InvalidFileFormat);
        }
        var blobName = $"{Guid.NewGuid()}{fileExtension}";
        await _blobContainer.SaveAsync(blobName, fileStream, overrideExisting: true, cancellationToken: cancellationToken);
        var url = BuildUrl(blobName);
        return (blobName, url);
    }

    public async Task<bool> DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        ValidateFileName(fileName);

        return await _blobContainer.DeleteAsync(fileName, cancellationToken: cancellationToken);
    }


    private string BuildUrl(string fileName)
    {
        var containerName = BlobContainerNameAttribute.GetContainerName<FileContainer>();
        var baseUrl = _configuration["BlobStorageSettings:BaseUrl"]!.TrimEnd('/');
        var basePath = _configuration["BlobStorageSettings:BasePath"]!.Trim('/');

        //string tenantPathSegment = _currentTenant.IsAvailable
        //    ? Path.Combine("tenants", _currentTenant.Id.ToString()!)
        //    : string.Empty; // Host files can be at the root if desired  
        return $"{baseUrl}/{basePath}/{containerName}/{fileName}".Replace("\\", "/");
    }

    public async Task<byte[]> GetAllBytesAsync(string fileName, CancellationToken cancellationToken = default)
    {
        ValidateFileName(fileName);
        return await _blobContainer.GetAllBytesAsync(fileName, cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetAsync(string fileName, CancellationToken cancellationToken = default)
    {
        ValidateFileName(fileName);
        return await _blobContainer.GetAsync(fileName, cancellationToken: cancellationToken);
    }


    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new BusinessException(BillingDomainErrorCodes.NullField)
                .WithData("field", nameof(fileName));
        }

        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(ext))
        {
            throw new BusinessException(BillingDomainErrorCodes.InvalidFileFormat);
        }
    }
}

