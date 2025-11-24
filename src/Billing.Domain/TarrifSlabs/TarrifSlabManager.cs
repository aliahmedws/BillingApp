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

    public void ValidateAllSlabs(
        decimal? previousUpperSlab,
        decimal lowerSlab,
        decimal? upperSlab,
        decimal unitPrice)
    {
        ValidateSlabValue(lowerSlab, upperSlab, unitPrice);

        if (previousUpperSlab.HasValue)
        {
            ValidateSlabContinuity(previousUpperSlab.Value, lowerSlab);
        }
    }

    public async Task<TarrifSlab> CreateAsync(
        decimal lowerSlab,
        decimal? upperSlab,
        decimal unitPrice
    )
    {
        Check.NotNull(lowerSlab, nameof(lowerSlab));
        Check.NotNull(unitPrice, nameof(unitPrice));

        var existingSlab = await _tarrifSlabRepository.FindByExistance(lowerSlab, upperSlab, unitPrice);
        if (existingSlab != null)
        {
            throw new SlabsAlreadyExistsException(lowerSlab, upperSlab, unitPrice);
        }

        var queryable = await _tarrifSlabRepository.GetQueryableAsync();
        var lastSlab = queryable.OrderByDescending(x => x.UpperSlab).FirstOrDefault();

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

    public async Task UpdateAsync(TarrifSlab tarrifSlab, decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        ValidateSlabValue(lowerSlab, upperSlab, unitPrice);

        var existingSlab = await _tarrifSlabRepository.FindByExistance(lowerSlab, upperSlab, unitPrice);
        if (existingSlab != null && (existingSlab.LowerSlab != lowerSlab || existingSlab.UpperSlab != upperSlab ||
            existingSlab.UnitPrice != unitPrice))
        {
            throw new SlabsAlreadyExistsException(lowerSlab, upperSlab, unitPrice);
        }

        var queryable = await _tarrifSlabRepository.GetQueryableAsync();
        var lastSlab = queryable.OrderByDescending(x => x.UpperSlab).FirstOrDefault(s => s.Id != tarrifSlab.Id);

        if (lastSlab != null && unitPrice <= lastSlab.UnitPrice)
        {
            throw new UnitPriceLessException(unitPrice);
        }

        decimal? previousUpperSlab = lastSlab?.UpperSlab;

        ValidateAllSlabs(previousUpperSlab, lowerSlab, upperSlab, unitPrice);

        tarrifSlab.LowerSlab = lowerSlab;
        tarrifSlab.UpperSlab = upperSlab;
        tarrifSlab.UnitPrice = unitPrice;

        await _tarrifSlabRepository.UpdateAsync(tarrifSlab);

    }
}
