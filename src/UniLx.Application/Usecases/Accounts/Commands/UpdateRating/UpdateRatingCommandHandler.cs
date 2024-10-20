using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Mappers;
using UniLx.Domain.Data;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateRating
{
    internal class UpdateRatingCommandHandler : ICommandHandler<UpdateRatingCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdateRatingCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }        

        public async Task<IResult> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AccountErrors.NotFound.ToBadRequest();

            account.Rating.UpdateRating(request.Rating);

            _accountRepository.UpdateOne(account, cancellationToken);
            await _accountRepository.UnitOfWork.Commit(cancellationToken);

            return Results.Ok(account.Rating.ToResponse());
        }
    }
}
