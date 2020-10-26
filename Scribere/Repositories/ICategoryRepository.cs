using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void DeleteCategory(int categoryId);
        List<Category> GetAll();
        Category GetCategoryById(int categoryId);
        void UpdateCategory(Category category);
    }
}