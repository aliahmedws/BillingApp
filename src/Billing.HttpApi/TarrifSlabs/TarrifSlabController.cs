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
public class TarrifSlabController : AbpController, ITarrifSlabAppService
{
    private readonly ITarrifSlabAppService _tarrifSlabAppService;

    public TarrifSlabController (ITarrifSlabAppService tarrifSlabAppService)
    {
        _tarrifSlabAppService = tarrifSlabAppService;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _tarrifSlabAppService.DeleteAsync(id);
    }

    [HttpPost]
    public async Task<TarrifSlabDto> CreateAsync(CreateTarrifSlabDto input)
    {
        return await _tarrifSlabAppService.CreateAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<TarrifSlabDto> GetAsync(Guid id)
    {
        return await _tarrifSlabAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<TarrifSlabDto>> GetListAsync(GetTarrifSlabLIstDto input)
    {
        return await _tarrifSlabAppService.GetListAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateTarrifSlabDto input)
    {
        await _tarrifSlabAppService.UpdateAsync(id, input);
    }
}
