using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp;

namespace Billing.TarrifSlabs;

[RemoteService(isEnabled: false)]
public class TarrifSlabAppService : BillingAppService, ITarrifSlabAppService
{
    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    private readonly TarrifSlabManager _tarrifSlabManager;

    public TarrifSlabAppService(ITarrifSlabRepository tarrifSlabRepository, TarrifSlabManager tarrifSlabManager)
    {
        _tarrifSlabRepository = tarrifSlabRepository;
        _tarrifSlabManager = tarrifSlabManager;
    }

    public async Task<TarrifSlabDto> GetAsync(Guid id)
    {
        var tarrifSlab = await _tarrifSlabRepository.GetAsync(id);
        return ObjectMapper.Map<TarrifSlab, TarrifSlabDto>(tarrifSlab);
    }

    public async Task<PagedResultDto<TarrifSlabDto>> GetListAsync()
    {
        var tarrifSlabs = await _tarrifSlabRepository.GetListAsync();
        return new PagedResultDto<TarrifSlabDto>(
            tarrifSlabs.Count,
            ObjectMapper.Map<List<TarrifSlab>, List<TarrifSlabDto>>(tarrifSlabs)
        );
    }

    public async Task UpdateAsync(Guid id, UpdateTarrifSlabDto input)
    {
        var tarrifSlabs = await _tarrifSlabRepository.GetAsync(id);

        await _tarrifSlabManager.UpdateAsync(
            tarrifSlabs,
            input.RateRangeOne,
            input.RateRangeTwo,
            input.RateRangeThree,
            input.RateRangeFour,
            input.RateRangeFive,
            input.RateRangeSix,
            input.RateRangeSeven,
            input.RateRangeEight
            );
        await _tarrifSlabRepository.UpdateAsync(tarrifSlabs);
    }


}
