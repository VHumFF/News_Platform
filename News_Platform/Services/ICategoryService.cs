using News_Platform.DTOs;
using News_Platform.Models;

namespace News_Platform.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(long id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(string name, string description);
    }
}
