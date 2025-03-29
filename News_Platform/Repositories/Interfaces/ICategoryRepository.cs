using News_Platform.Models;

namespace News_Platform.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(long id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task SaveChangesAsync();
    }
}
