using Asp.Versioning;
using Billing.MaintenanceBills;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Billing.MaintenanceBillTemplates;

[RemoteService(IsEnabled = true)]
[ControllerName("MaintenanceBillTemplateBills")]
[Area("app")]
[Route("api/app/maintenance-bill-templates")]
public class MaintenanceBillTemplateController(IMaintenanceBillTemplateAppService maintenanceBillTemplateAppService) : AbpController, IMaintenanceBillTemplateAppService
{
    private readonly IMaintenanceBillTemplateAppService _maintenanceBillTemplateAppService = maintenanceBillTemplateAppService;

    [HttpGet("print-template")]
    public Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid id)
    {
        return _maintenanceBillTemplateAppService.GetPrintHtmlAsync(id);
    }

    [HttpGet("print-multiple")]
    public Task<IRemoteStreamContent> PrintFilteredAsync(GetMaintenanceBillListDto input)
    {
        return _maintenanceBillTemplateAppService.PrintFilteredAsync(input);
    }
}
