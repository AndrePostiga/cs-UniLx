using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts
{
    public static class AccountErrors
    {
        public static readonly Error NotFound = new(System.Net.HttpStatusCode.NotFound, "Accounts.Id.NotFound", "Can't found account with provided id,");
        public static readonly Error ProfilePictureNotUploaded = new(System.Net.HttpStatusCode.PreconditionFailed, "Accounts.ProfilePicture.NotUploaded", "The profile picture at the given path was not uploaded to storage.");
        public static readonly Error Conflict = new(System.Net.HttpStatusCode.Conflict, "Accounts.Conflict", "The account already exists.");
    }
}
