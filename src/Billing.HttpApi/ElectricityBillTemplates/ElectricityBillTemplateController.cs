using Asp.Versioning;
using Billing.ElectricityBills;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;


namespace Billing.ElectricityBillTemplates;

[RemoteService(IsEnabled = true)]
[ControllerName("ElectricityBillTemplateBills")]
[Area("app")]
[Route("api/app/electricity-bill-templates")]
public class ElectricityBillTemplateController(IElectricityBillTemplateAppService electricityBillTemplateAppService) : AbpController, IElectricityBillTemplateAppService
{
    private readonly IElectricityBillTemplateAppService _electricityBillTemplateAppService = electricityBillTemplateAppService;

    [HttpGet("print-template")]
    public Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid billId)
    {
        return _electricityBillTemplateAppService.GetPrintHtmlAsync(billId);
    }

    [HttpGet("print-multiple")]
    public Task<IRemoteStreamContent> PrintFilteredAsync(GetElectricityBillListDto input)
    {
        return _electricityBillTemplateAppService.PrintFilteredAsync(input);
    }
}
