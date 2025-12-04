using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.TarrifSlabs;

public class TarrifSlabManager : DomainService
{

    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    public TarrifSlabManager(ITarrifSlabRepository tarrifSlabRepository)
    {
        _tarrifSlabRepository = tarrifSlabRepository;
    }
    private void ValidateSlabValue(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        if (lowerSlab < TarrifSlabConsts.MinValue || lowerSlab > TarrifSlabConsts.MaxValue)
            throw new LowerSlabException(lowerSlab);

        if (upperSlab.HasValue && upperSlab <= lowerSlab)
            throw new UpperSlabException(upperSlab);

        if (unitPrice < 0)
            throw new UnitPriceException(unitPrice);
    }
    private void ValidateSlabContinuity(decimal previousUpperSlab, decimal newLowerSlab)
    {
        if (newLowerSlab != previousUpperSlab + 1)
        {
            throw new InvalidSlabRangeException(previousUpperSlab, newLowerSlab);
        }
    }
    public void ValidateAllSlabs(decimal? previousUpperSlab, decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        ValidateSlabValue(lowerSlab, upperSlab, unitPrice);

        if (previousUpperSlab.HasValue)
        {
            ValidateSlabContinuity(previousUpperSlab.Value, lowerSlab);
        }
    }
    // create Async
    public async Task<TarrifSlab> CreateAsync(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        Check.NotNull(lowerSlab, nameof(lowerSlab));
        Check.NotNull(unitPrice, nameof(unitPrice));

        var existingLSlab = await _tarrifSlabRepository.FindByLowerSlab(lowerSlab);
        if (existingLSlab != null)
        {
            throw new LowerSlabAlreadyExist(lowerSlab);
        }

        if (upperSlab.HasValue)
        {
            var existingUSlab = await _tarrifSlabRepository.FindByUpperSlab(upperSlab.Value);
            if (existingUSlab != null)
            {
                throw new UpperSlabAlreadyExist(upperSlab);
            }
        }

        var existingUnitPrice = await _tarrifSlabRepository.FindByUnitPrice(unitPrice);
        if (existingUnitPrice != null)
        {
            throw new UnitPriceAlreadyExist(unitPrice);
        }

        var queryable = await _tarrifSlabRepository.GetQueryableAsync();
        var lastSlab = queryable
            .Where(x => x.UpperSlab.HasValue)
            .OrderByDescending(x => x.UpperSlab)
            .FirstOrDefault();


        if (lastSlab != null)
        {
            if (lowerSlab != lastSlab.UpperSlab + 1)
            {
                throw new InvalidSlabRangeException(lastSlab.UpperSlab ?? 0, lowerSlab);
            }

            if (unitPrice <= lastSlab.UnitPrice)
            {
                throw new UnitPriceLessException(unitPrice);
            }
        }

        decimal? previousUpperSlab = lastSlab?.UpperSlab;

        ValidateAllSlabs(previousUpperSlab, lowerSlab, upperSlab, unitPrice);

        return new TarrifSlab(
            GuidGenerator.Create(),
            lowerSlab,
            upperSlab,
            unitPrice
        );
    }
    //update Async
    public async Task UpdateAsync(TarrifSlab tarrifSlab, decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        Check.NotNull(tarrifSlab, nameof(tarrifSlab));

        ValidateSlabValue(lowerSlab, upperSlab, unitPrice);

        var existingLSlab = await _tarrifSlabRepository.FindByLowerSlab(lowerSlab);
        if (existingLSlab != null && existingLSlab.Id != tarrifSlab.Id)
        {
            throw new LowerSlabAlreadyExist(lowerSlab);
        }

        if (upperSlab.HasValue)
        {
            var existingUSlab = await _tarrifSlabRepository.FindByUpperSlab(upperSlab.Value);
            if (existingUSlab != null && existingUSlab.Id != tarrifSlab.Id)
            {
                throw new UpperSlabAlreadyExist(upperSlab.Value);
            }
        }

        var existingUnitPrice = await _tarrifSlabRepository.FindByUnitPrice(unitPrice);
        if (existingUnitPrice != null && existingUnitPrice.Id != tarrifSlab.Id)
        {
            throw new UnitPriceAlreadyExist(unitPrice);
        }

        var queryable = await _tarrifSlabRepository.GetQueryableAsync();
        var previousSlab = queryable
        .Where(s => s.Id != tarrifSlab.Id)
        .Where(s => s.UpperSlab.HasValue && s.UpperSlab < lowerSlab)
        .OrderByDescending(s => s.UpperSlab)
        .FirstOrDefault();


        if (previousSlab != null && unitPrice <= previousSlab.UnitPrice)
        {
            throw new UnitPriceLessException(unitPrice);
        }

        decimal? previousUpperSlab = previousSlab?.UpperSlab;

        ValidateAllSlabs(previousUpperSlab, lowerSlab, upperSlab, unitPrice);

        tarrifSlab.LowerSlab = lowerSlab;
        tarrifSlab.UpperSlab = upperSlab;
        tarrifSlab.UnitPrice = unitPrice;

        await _tarrifSlabRepository.UpdateAsync(tarrifSlab);

    }
}
