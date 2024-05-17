using SawyersCodeBlog.Client.Models;

namespace SawyersCodeBlog.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(CategoryDTO category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
