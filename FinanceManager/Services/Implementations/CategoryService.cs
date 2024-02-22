using FinanceManager.Data.Exceptions;
using FinanceManager.Data.Models;
using FinanceManager.Data.Repositories.Interfaces;
using FinanceManager.Exceptions;
using FinanceManager.Services.Interfaces;

namespace FinanceManager.Services.Implementations
{
	public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository = categoryRepository;

		public async Task<bool> CreateCategoryAsync(Category category)
		{
			if (string.IsNullOrEmpty(category.Name))
			{
				throw new ArgumentNullException("Category name cannot be empty.", innerException: null);
			}
			try
			{
				return await _categoryRepository.CreateCategoryAsync(category);
			}
			catch (RepositoryException)
			{
				throw new AlreadyExistsException("A category with that name already exists.");
			}
		}

		public async Task<bool> DeleteCategoryAsync(uint id)
		{
			return await _categoryRepository.DeleteCategoryAsync(id);
		}

		public async Task<bool> UpdateCategoryAsync(Category category)
		{
			if (string.IsNullOrEmpty(category.Name))
			{
				throw new ArgumentNullException("Category name cannot be empty.", innerException: null);
			}
			try
			{
				return await _categoryRepository.UpdateCategoryAsync(category);
			}
			catch (RepositoryException)
			{
				throw new AlreadyExistsException("A category with that name already exists.");
			}
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			return await _categoryRepository.GetCategoriesAsync();
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync(bool expenses)
		{
			return await _categoryRepository.GetCategoriesAsync(expenses);
		}

		public async Task<Category?> GetCategoryAsync(uint id)
		{
			return await _categoryRepository.GetCategoryByIdAsync(id);
		}

		public async Task<Category?> GetCategoryAsync(string name)
		{
			return await _categoryRepository.GetCategoryByNameAsync(name);
		}
	}
}
