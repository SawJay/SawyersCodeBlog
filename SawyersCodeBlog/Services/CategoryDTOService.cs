using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Client.Services.Interfaces;
using SawyersCodeBlog.Helpers;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;

namespace SawyersCodeBlog.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description,               
            };

            if (category.ImageUrl!.StartsWith("data:"))
            {
                newCategory.Image = UploadHelper.GetImageUpload(category.ImageUrl);
            }

            Category createdCategory = await repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await repository.DeleteCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await repository.GetCategoriesAsync();

            // List<CategoryDTO> categoryDtos = new List<CategoryDTO>();

            // foreach (Category category in categories)
            // {
            //    CategoryDTO categoryDTO = category.ToDTO();
            //    categoryDtos.Add(categoryDTO);
            // } OR!!

            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());

            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            Category? category = await repository.GetCategoryByIdAsync(id);
            return category?.ToDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetPopularCategoriesAsync()
        {
            IEnumerable<Category> categories = await repository.GetPopularCategoriesAsync();

            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());

            return categoryDTOs;
        }

        public async Task UpdateCategoryAsync(CategoryDTO category)
        {
            Category? categoryToUpdate = await repository.GetCategoryByIdAsync(category.Id);

            if (categoryToUpdate is not null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;

                if (category.ImageUrl!.StartsWith("data:"))
                {
                    categoryToUpdate.Image = UploadHelper.GetImageUpload(category.ImageUrl);
                }
                else
                {
                    categoryToUpdate.Image = null;
                }

                await repository.UpdateCategoryAsync(categoryToUpdate);
            }
        }
    }
}
