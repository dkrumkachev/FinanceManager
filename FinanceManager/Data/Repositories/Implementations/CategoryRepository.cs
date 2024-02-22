using Dapper;
using FinanceManager.Data.Exceptions;
using FinanceManager.Data.Models;
using FinanceManager.Data.Repositories.Interfaces;
using MySql.Data.MySqlClient;

namespace FinanceManager.Data.Repositories.Implementations
{
	public class CategoryRepository : ICategoryRepository
	{
		public async Task<bool> CreateCategoryAsync(Category category)
		{
			var sql = @"INSERT INTO category
						VALUES (NULL, @IsForExpenses, @Name)";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			try
			{
				var affectedRows = await connection.ExecuteAsync(sql, new
				{
					category.IsForExpenses,
					category.Name,
				});
				if (affectedRows == 0)
				{
					return false;
				}
				var id = await connection.ExecuteScalarAsync<uint>("SELECT LAST_INSERT_ID();");
				category.CategoryId = id;
				return true;
			}
			catch (MySqlException)
			{
				throw new RepositoryException("An error occurred while executing a database query");
			}
		}

		public async Task<bool> DeleteCategoryAsync(uint id)
		{
			var sql = @"DELETE FROM category
						WHERE categoryId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
			return affectedRows != 0;
		}

		public async Task<bool> UpdateCategoryAsync(Category category)
		{
			var sql = @"UPDATE category
						SET isForExpenses = @IsForExpenses, name = @Name
						WHERE categoryId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			try
			{
				var affectedRows = await connection.ExecuteAsync(sql, new
				{
					category.IsForExpenses,
					category.Name,
					Id = category.CategoryId
				});
				return affectedRows != 0;
			}
			catch (MySqlException)
			{
				throw new RepositoryException("An error occurred while executing a database query");
			}
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			var sql = @"SELECT * FROM category";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category>(sql);
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync(bool expenses)
		{
			var sql = @"SELECT * FROM category WHERE isForExpenses = @IsForExpenses";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category>(sql, new { IsForExpenses = expenses });
		}

		public async Task<Category?> GetCategoryByIdAsync(uint id)
		{
			var sql = @"SELECT * FROM category WHERE categoryId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
		}

		public async Task<Category?> GetCategoryByNameAsync(string name)
		{
			var sql = @"SELECT * FROM category WHERE name = @Name";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Name = name });
		}
	}
}
