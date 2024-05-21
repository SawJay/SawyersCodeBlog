﻿using Microsoft.EntityFrameworkCore;
using SawyersCodeBlog.Data;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;

namespace SawyersCodeBlog.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is not null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Category> categories = await context.Categories.Include(c => c.Posts).ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.Categories.AnyAsync(c => c.Id == category.Id);

            if (shouldUpdate)
            {
                ImageUpload? oldImage = null;

                if (category.Image is not null)
                {
                    // save the new image
                    context.Images.Add(category.Image);

                    // check for an old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == category.ImageId);

                    // fix the foreign key
                    category.ImageId = category.Image.Id;
                }

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
