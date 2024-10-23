using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisement.CreateAdvertisement
{
    public class CreateAdvertisementCommand : ICommand<IResult>
    {
        public string? Type { get; set; }



        public CreateAdvertisementCommand() { }
    }
}
