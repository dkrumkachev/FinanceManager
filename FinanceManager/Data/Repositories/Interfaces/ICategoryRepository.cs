using FinanceManager.Data.Models;

namespace FinanceManager.Data.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		public Task<Category?> GetCategoryByIdAsync(uint id);

		public Task<Category?> GetCategoryByNameAsync(string name);

		public Task<IEnumerable<Category>> GetCategoriesAsync();

		public Task<IEnumerable<Category>> GetCategoriesAsync(bool expenses);

		public Task<bool> CreateCategoryAsync(Category category);

		public Task<bool> DeleteCategoryAsync(uint id);

		public Task<bool> UpdateCategoryAsync(Category category);
	}
}
