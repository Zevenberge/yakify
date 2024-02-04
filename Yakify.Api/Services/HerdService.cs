using Yakify.Api.CodeAnalysis;
using Yakify.Api.Models;
using Yakify.Domain;
using Yakify.Repository;

namespace Yakify.Api.Services;

public class HerdService(IYakRepository yakRepository)
{
    public async Task LoadNewHerd(CreateHerdDto dto, CancellationToken cancellationToken)
    {
        var yaks = dto.Herd.Select(yak =>
        {
            Assume.NotNull(yak.Name);
            Assume.NotNull(yak.Age);
            Assume.NotNull(yak.Sex);
            return new Yak(yak.Name, yak.Age.Value, yak.Sex.Value);
        }).ToArray();
        await yakRepository.DeleteAll(cancellationToken);
        await yakRepository.AddRange(yaks, cancellationToken);
    }

    public async Task<HerdStatusDto> GetHerdStatus(int day, CancellationToken cancellationToken)
    {
        var yaks = await yakRepository.GetAll(cancellationToken);
        return new HerdStatusDto(
            yaks.Where(yak => !yak.HasDied(day))
                .Select(yak => new YakStatusDto(yak.Name, yak.ActualAgeInYearsAfterDay(day), yak.AgeLastShavedInYears(day)))
                .ToArray()
        );
    }

    public async Task<StockDto> GetTotalProduce(int day, CancellationToken cancellationToken)
    {
        var yaks = await yakRepository.GetAll(cancellationToken);
        return new StockDto(
            yaks.Sum(yak => yak.TotalMilkProduceUpToAndIncludingDay(day)),
            yaks.Sum(yak => yak.TotalAmountOfHidesProducedUpToAndInclusingDay(day))
        );
    }
}
