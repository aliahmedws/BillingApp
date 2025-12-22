using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Billing.ElectricityBills;

namespace Billing.ElectricityBillTemplates;

public interface IElectricityBillTemplateAppService : IApplicationService
{
    Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid billId);
    Task<IRemoteStreamContent> PrintFilteredAsync(GetElectricityBillListDto input);
}
