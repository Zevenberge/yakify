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
}
