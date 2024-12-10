using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Categories
{
    public static class CategoryErrors
    {
        public static readonly Error Conflict = new(System.Net.HttpStatusCode.Conflict, "Category.Conflict", "Category name already in database, cannot create.");
        public static readonly Error NotFound = new(System.Net.HttpStatusCode.NotFound, "Category.NotFound", "Category name was not found.");
    }
}
