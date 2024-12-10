using UniLx.Application.Usecases.Categories.CreateCategory.Models;

namespace UniLx.Application.Usecases.Categories.CreateCategory.Mappers
{
    public static class CreateCategoryRequestToCreateCategoryCommandMapper
    {
        public static CreateCategoryCommand ToCommand(this CreateCategoryRequest source)
            => new(root: source.RootType, name: source.Name, nameInPtBr: source.NameInPtBr, description: source.Description);
    }
}
