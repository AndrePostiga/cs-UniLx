using Microsoft.AspNetCore.Http;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Shared.CreatePresignedImage
{
    public class CreatePresignedImageCommandHandler : ICommandHandler<CreatePresignedImageCommand, IResult>
    {
        private readonly IStorageRepository<AccountAvatarBucketOptions> _storageRepository;

        public CreatePresignedImageCommandHandler(IStorageRepository<AccountAvatarBucketOptions> storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<IResult> Handle(CreatePresignedImageCommand request, CancellationToken cancellationToken)
        {
            var signedUrl = await _storageRepository.GeneratePreSignedUrlAsync("algo.png", 10);
            return Results.Ok(signedUrl);
        }
    }
}
