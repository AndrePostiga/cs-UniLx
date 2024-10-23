using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Usecases.Categories.CreateCategory.Mappers;
using UniLx.Application.Usecases.Categories.CreateCategory.Models;

namespace UniLx.ApiService.Controllers.Categories
{
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/categories", AdminControllersHandlers.CreateCategory);
        }
    }

    public static class AdminControllersHandlers
    {
        internal static async Task<IResult> CreateCategory(
                [FromBody] CreateCategoryRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
