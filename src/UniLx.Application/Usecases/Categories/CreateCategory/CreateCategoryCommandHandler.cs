using JasperFx.Core;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Categories.CreateCategory
{
    internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Category, bool>> spec =
                cat => cat.Id == request.Name && 
                cat.Root.Name == request.Root;

            var cat = await _categoryRepository.FindOne(spec, cancellationToken);            
            if (cat is not null)
                return CategoryErrors.Conflict.ToBadRequest();

            var category = Category.CreateNewCategory(
                root: request.Root,
                name: request.Name.Capitalize(),
                nameInPtBr: request.NameInPtBr,
                description: request.Description);

            _categoryRepository.InsertOne(category);
            await _categoryRepository.UnitOfWork.Commit();
            return Results.Ok(category);
        }
    }
}
