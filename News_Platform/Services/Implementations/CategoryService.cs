using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;

namespace News_Platform.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(long id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                CategoryID = category.CategoryID,
                Name = category.Name
            };
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryID = c.CategoryID,
                Name = c.Name
            });
        }

        public async Task<CategoryDto> AddCategoryAsync(string name, string description)
        {
            var category = new Category
            {
                Name = name,
                Description = description,
            };

            await _categoryRepository.AddCategoryAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryID = category.CategoryID,
                Name = category.Name
            };
        }
    }
}
