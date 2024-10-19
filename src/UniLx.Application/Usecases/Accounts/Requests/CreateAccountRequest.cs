namespace UniLx.Application.Usecases.Accounts.Requests
{
    public class CreateAccountRequest
    {
        public string Name { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string? Description { get; set; }

        public string? ProfilePicturePath { get; set; }
    }

}
