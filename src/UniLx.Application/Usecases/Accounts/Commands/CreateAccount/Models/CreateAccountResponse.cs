using UniLx.Application.Usecases.SharedModels.Responses;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models
{
    internal record CreateAccountResponse(
        string Id, 
        string Name, 
        string Cpf, 
        string? Description, 
        string Email, 
        string? ProfilePictureUrl, 
        RatingResponse Rating, 
        List<string>? Advertisements, 
        List<string>? InterestedAdvertisements, 
        DateTime CreatedAt);
}
