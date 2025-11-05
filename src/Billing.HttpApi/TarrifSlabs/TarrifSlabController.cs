using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.TarrifSlabs;

[RemoteService(IsEnabled = true)]
[ControllerName("TarrifSlabs")]
[Area("app")]
[Route("api/app/tarrif-slabs")]
public class TarrifSlabController : AbpController
{
    private readonly ITarrifSlabAppService _tarrifSlabAppServicep;

    public TarrifSlabController (ITarrifSlabAppService tarrifSlabAppServicep)
    {
        _tarrifSlabAppServicep = tarrifSlabAppServicep;
    }

    [HttpGet("{id}")]
    public async Task<TarrifSlabDto> GetAsync(Guid id)
    {
        return await _tarrifSlabAppServicep.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<TarrifSlabDto>> GetListAsync()
    {
        return await _tarrifSlabAppServicep.GetListAsync();
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateTarrifSlabDto input)
    {
        await _tarrifSlabAppServicep.UpdateAsync(id, input);
    }
}
