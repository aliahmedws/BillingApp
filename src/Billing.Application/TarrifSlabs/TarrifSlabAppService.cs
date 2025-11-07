 using Billing.Permissions;
using Billing.SocietyCharges;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using static Billing.Permissions.BillingPermissions;

namespace Billing.TarrifSlabs;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.TarrifSlabs.Default)]
public class TarrifSlabAppService : BillingAppService, ITarrifSlabAppService
{
    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    private readonly TarrifSlabManager _tarrifSlabManager;

    public TarrifSlabAppService(ITarrifSlabRepository tarrifSlabRepository, TarrifSlabManager tarrifSlabManager)
    {
        _tarrifSlabRepository = tarrifSlabRepository;
        _tarrifSlabManager = tarrifSlabManager;
    }

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

    public async Task DeleteAsync(Guid id)
    {
        await _tarrifSlabRepository.DeleteAsync(id);
    }

    //GET ASYNC
    public async Task<TarrifSlabDto> GetAsync(Guid id)
    {
        var tarrifSlab = await _tarrifSlabRepository.GetAsync(id);
        return ObjectMapper.Map<TarrifSlab, TarrifSlabDto>(tarrifSlab);
    }
                  //GET LIST ASYNC
    public async Task<PagedResultDto<TarrifSlabDto>> GetListAsync()
    {
        var tarrifSlabs = await _tarrifSlabRepository.GetListAsync();
        return new PagedResultDto<TarrifSlabDto>(
            tarrifSlabs.Count,
            ObjectMapper.Map<List<TarrifSlab>, List<TarrifSlabDto>>(tarrifSlabs)
        );
    }
    //UPDATE ASYNC
    [Authorize(BillingPermissions.TarrifSlabs.Edit)]
    public async Task UpdateAsync(Guid id, UpdateTarrifSlabDto input)
    {
        var tarrifSlabs = await _tarrifSlabRepository.GetAsync(id);
    
        await _tarrifSlabManager.UpdateAsync(
            tarrifSlabs,
            input.LowerSlab,
            input.UpperSlab,
            input.UnitPrice
            );
        await _tarrifSlabRepository.UpdateAsync(tarrifSlabs);
    }
}
