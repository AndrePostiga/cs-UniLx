using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Services
{
    public interface ICreateAdvertisementDomainService
    {
        Task<ServiceResult<Advertisement>> CreateAdvertisement(
            AdvertisementType type,
            Category category,
            Details details,
            DateTime? ExpiresAt,
            Account account, 
            Address rawAddress,
            CancellationToken cancellationToken);
    }

    public interface IMapsService
    {
        Task<ServiceResult<Address>> GetAddressFromFullAddress(Address rawAddress, CancellationToken cancellationToken);
        Task<ServiceResult<Address>> GetAddressFromCordinates(Address rawAddress, CancellationToken cancellationToken);
    }

    public class CreateAdvertisementDomainService : ICreateAdvertisementDomainService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapsService _mapsService;

        public CreateAdvertisementDomainService(
            IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            IMapsService mapsService)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _mapsService = mapsService;
        }

        public async Task<ServiceResult<Advertisement>> CreateAdvertisement(AdvertisementType type, Category category, 
            Details details, DateTime? ExpiresAt, Account account, Address rawAddress, CancellationToken cancellationToken)
        {
            var fullAddress = await GetResolvedAddress(rawAddress, cancellationToken);
            if (fullAddress.IsError)
                return ServiceResult<Advertisement>.Failure(fullAddress.Error);

            var advertisement = new Advertisement(
                type.Name,
                category,
                details,
                ExpiresAt,
                fullAddress.Content!,
                account);

            _advertisementRepository.InsertOne(advertisement);
            _accountRepository.UpdateOne(account);
            await _advertisementRepository.UnitOfWork.Commit(cancellationToken);
            return ServiceResult.Success(advertisement);
        }

        private async Task<ServiceResult<Address>> GetResolvedAddress(Address rawAddress, CancellationToken cancellationToken)
        {
            if (rawAddress.Type == AddressType.Coordinates || rawAddress!.Type == AddressType.Both)
            {
                return await _mapsService.GetAddressFromCordinates(rawAddress, cancellationToken);
            }
            
            return await _mapsService.GetAddressFromFullAddress(rawAddress, cancellationToken);
        }
    }
}
