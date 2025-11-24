using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.TarrifSlabs.Default)]
public class TarrifSlabAppService : BillingAppService, ITarrifSlabAppService
{
    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    private readonly TarrifSlabManager _tarrifSlabManager;

    public TarrifSlabAppService(
       ITarrifSlabRepository tarrifSlabRepository,
       TarrifSlabManager tarrifSlabManager)
    {
        _tarrifSlabRepository = tarrifSlabRepository;
        _tarrifSlabManager = tarrifSlabManager;
    }
    public async Task<TarrifSlabDto> GetAsync(Guid id)
    {
        var tarrifSlab = await _tarrifSlabRepository.GetAsync(id);
        return ObjectMapper.Map<TarrifSlab, TarrifSlabDto>(tarrifSlab);
    }

    public async Task<PagedResultDto<TarrifSlabDto>> GetListAsync(GetTarrifSlabLIstDto input)
    {
        if(input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(TarrifSlab.UnitPrice);
        }

        var tarrifSlabs = await _tarrifSlabRepository.GetListAsync(
              input.SkipCount,
              input.MaxResultCount,
              input.Sorting!,
              input.Filter,
              input.LowerSlab,
              input.UpperSlab,
              input.UnitPrice
            );

        var totalCount = await _tarrifSlabRepository.GetCountAsync(
             input.Filter,
             input.LowerSlab,
             input.UpperSlab,
             input.UnitPrice
   );

        return new PagedResultDto<TarrifSlabDto>(totalCount,
        ObjectMapper.Map<List<TarrifSlab>, List<TarrifSlabDto>>(tarrifSlabs));
    }

    [Authorize(BillingPermissions.TarrifSlabs.Create)]
    public async Task<TarrifSlabDto> CreateAsync(CreateTarrifSlabDto input)
    {
        var tarrifSlab = await _tarrifSlabManager.CreateAsync(
            input.LowerSlab,
            input.UpperSlab,
            input.UnitPrice
        );

        await _tarrifSlabRepository.InsertAsync(tarrifSlab);

        return ObjectMapper.Map<TarrifSlab, TarrifSlabDto>(tarrifSlab);
    }


    [Authorize(BillingPermissions.TarrifSlabs.Edit)]
    public async Task UpdateAsync(Guid id, UpdateTarrifSlabDto input)
    {
        var tarrifSlabs = await _tarrifSlabRepository.GetAsync(id);

        await _tarrifSlabManager.UpdateAsync(
            tarrifSlabs,
            input.LowerSlab,
            input.UpperSlab,
            input.UnitPrice);

        await _tarrifSlabRepository.UpdateAsync(tarrifSlabs);
    }


    [Authorize(BillingPermissions.TarrifSlabs.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _tarrifSlabRepository.DeleteAsync(id);
    }
}
