using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Application.Usecases.Advertisement.CreateAdvertisement
{
    internal class CreateAdvertisementCommandHandler : ICommandHandler<CreateAdvertisementCommand, IResult>
    {
        public async Task<IResult> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {

            var category = new Category();

            return Results.Ok(ad);
        }
    }
}
