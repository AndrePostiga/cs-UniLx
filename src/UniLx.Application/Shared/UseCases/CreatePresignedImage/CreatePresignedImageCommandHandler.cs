using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Shared.UseCases.CreatePresignedImage
{
    public class CreatePresignedImageCommandHandler : ICommandHandler<CreatePresignedImageCommand, IResult>
    {
        private readonly IStorageRepository<AccountAvatarBucketOptions> _storageRepository;
        private readonly IOptions<AccountAvatarBucketOptions> _options;

        public CreatePresignedImageCommandHandler(
            IOptions<AccountAvatarBucketOptions> options,
            IStorageRepository<AccountAvatarBucketOptions> storageRepository)
        {
            _storageRepository = storageRepository;
            _options = options;
        }

        public async Task<IResult> Handle(CreatePresignedImageCommand request, CancellationToken cancellationToken)
        {
            var signedUrl = await _storageRepository.GeneratePreSignedUrlAsync("algo.png", 10);
            return Results.Ok(signedUrl);
        }
    }
}
