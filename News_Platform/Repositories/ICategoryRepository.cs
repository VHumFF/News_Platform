using News_Platform.Models;

namespace News_Platform.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(long id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task SaveChangesAsync();
    }
}
