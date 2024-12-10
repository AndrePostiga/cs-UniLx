namespace UniLx.Application.Usecases.Categories.CreateCategory.Models
{
    public class CreateCategoryRequest
    {
        public string RootType { get; set; }
        public string Name { get; set; }
        public string NameInPtBr { get; set; }
        public string? Description { get; set; }
    }
}
