using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Billing.MeterDocuments;

public interface IMeterDocumentAppService : IApplicationService
{
    Task<MeterDocumentDto> UploadAsync(IFormFile file, CreateMeterDocumentDto input);
    Task DeleteAsync(Guid id);
    Task<MeterDocumentDto> UpdateAsync(Guid id, UpdateMeterDocumentDto input);
}
