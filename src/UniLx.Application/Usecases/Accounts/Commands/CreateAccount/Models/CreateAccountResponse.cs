namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models
{
    internal record CreateAccountResponse(
        string Id, 
        string Name, 
        string Cpf, 
        string? Description, 
        string Email, 
        string? ProfilePictureUrl, 
        float Rating, 
        List<string> Advertisements, 
        DateTime CreatedAt);
}
