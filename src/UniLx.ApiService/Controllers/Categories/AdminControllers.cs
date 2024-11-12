using Carter;
using Carter.OpenApi;
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
            var categoriesGroup = app.MapGroup("/categories")
                                     .WithTags("Admin Categories")
                                     .IncludeInOpenApi();

            categoriesGroup.MapPost("/", AdminControllersHandlers.CreateCategory)
                           .WithName(nameof(AdminControllersHandlers.CreateCategory));
        }
    }

    public static class AdminControllersHandlers
    {
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a new category with the specified details.
        /// </remarks>
        /// <param name="request">The request containing the details of the category to create.</param>
        /// <param name="mediator">The mediator service for handling the command.</param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>A result indicating success or failure of the category creation.</returns>
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
