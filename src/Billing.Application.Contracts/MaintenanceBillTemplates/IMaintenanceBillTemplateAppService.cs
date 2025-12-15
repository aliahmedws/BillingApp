using Billing.MaintenanceBills;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Billing.MaintenanceBillTemplates;

public interface IMaintenanceBillTemplateAppService : IApplicationService
{
    Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid id);
    Task<IRemoteStreamContent> PrintFilteredAsync(GetMaintenanceBillListDto input);

}
