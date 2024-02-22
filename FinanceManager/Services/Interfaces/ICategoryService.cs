using FinanceManager.Data.Models;

namespace FinanceManager.Services.Interfaces
{
	public interface ICategoryService
	{
		public Task<Category?> GetCategoryAsync(uint id);

		public Task<Category?> GetCategoryAsync(string name);

		public Task<IEnumerable<Category>> GetCategoriesAsync();

		public Task<IEnumerable<Category>> GetCategoriesAsync(bool expenses);

		public Task<bool> CreateCategoryAsync(Category category);

		public Task<bool> DeleteCategoryAsync(uint id);

		public Task<bool> UpdateCategoryAsync(Category category);
	}
}
