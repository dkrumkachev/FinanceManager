using Dapper;
using FinanceManager.Data.Exceptions;
using FinanceManager.Data.Models;
using FinanceManager.Data.Repositories.Interfaces;
using MySql.Data.MySqlClient;

namespace FinanceManager.Data.Repositories.Implementations
{
	public class TransactionRepository : ITransactionRepository
	{
		public async Task<bool> CreateTransactionAsync(Transaction transaction)
		{
			var sql = @"INSERT INTO transaction
						VALUES (NULL, @IsExpense, @Date, @CategoryId, @Amount, @Comment)";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			try
			{
				var affectedRows = await connection.ExecuteAsync(sql, new
				{
					transaction.IsExpense,
					transaction.Date,
					transaction.Category.CategoryId,
					transaction.Amount,
					transaction.Comment
				});
				if (affectedRows == 0)
				{
					return false;
				}
				var id = await connection.ExecuteScalarAsync<uint>("SELECT LAST_INSERT_ID();");
				transaction.TransactionId = id;
				return true;
			}
			catch (MySqlException)
			{
				throw new RepositoryException("An error occurred while executing a database query");
			}
		}

		public async Task<bool> DeleteTransactionAsync(uint id)
		{
			var sql = @"DELETE FROM transaction
						WHERE transactionId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
			return affectedRows != 0;
		}

		public async Task<Transaction?> GetTransactionByIdAsync(uint id)
		{
			var sql = @"SELECT category.*, transaction.*
						FROM transaction
						JOIN category USING(categoryId)
						WHERE transactionId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			var result = await connection.QueryAsync<Category, Transaction, Transaction>(sql,
				(category, transaction) =>
				{
					transaction.Category = category;
					return transaction;
				},
				splitOn: "transactionId",
				param: new { Id = id });

			return result.FirstOrDefault();
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
		{
			var sql = @"SELECT category.*, transaction.*
						FROM transaction
						JOIN category USING(categoryId)
						ORDER BY date DESC";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category, Transaction, Transaction>(sql,
				(category, transaction) =>
				{
					transaction.Category = category;
					return transaction;
				},
				splitOn: "transactionId");
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to)
		{
			var sql = @"SELECT category.*, transaction.*
						FROM transaction
						JOIN category USING(categoryId)
						WHERE date >= @FromDate AND date <= @ToDate
						ORDER BY date DESC";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category, Transaction, Transaction>(sql,
				(category, transaction) =>
				{
					transaction.Category = category;
					return transaction;
				},
				splitOn: "transactionId",
				param: new { FromDate = from, ToDate = to });
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(bool expenses)
		{
			var sql = @"SELECT category.*, transaction.*
						FROM transaction
						JOIN category USING(categoryId)
						WHERE isExpense = @IsExpense
						ORDER BY date DESC";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category, Transaction, Transaction>(sql,
				(category, transaction) =>
				{
					transaction.Category = category;
					return transaction;
				},
				splitOn: "transactionId",
				param: new { IsExpense = expenses });
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to, bool expenses)
		{
			var sql = @"SELECT category.*, transaction.*
						FROM transaction
						JOIN category USING(categoryId)
						WHERE date >= @FromDate AND date <= @ToDate AND isExpense = @IsExpense
						ORDER BY date DESC";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			return await connection.QueryAsync<Category, Transaction, Transaction>(sql,
				(category, transaction) =>
				{
					transaction.Category = category;
					return transaction;
				},
				splitOn: "transactionId",
				param: new
				{
					FromDate = from,
					ToDate = to,
					IsExpense = expenses
				});
		}

		public async Task<bool> UpdateTransactionAsync(Transaction transaction)
		{
			var sql = @"UPDATE transaction
						SET isExpense = @IsExpense,
							date = @Date,
							categoryId = @CategoryId,
							amount = @Amount,
							comment = @Comment
						WHERE transactionId = @Id";
			using var connection = new MySqlConnection(AppSettings.ConnectionString);
			try
			{
				var affectedRows = await connection.ExecuteAsync(sql, new
				{
					Id = transaction.TransactionId,
					transaction.IsExpense,
					transaction.Date,
					transaction.Category.CategoryId,
					transaction.Amount,
					transaction.Comment,
				});
				return affectedRows != 0;
			}
			catch (MySqlException)
			{
				throw new RepositoryException("An error occurred while executing a database query");
			}
		}
	}
}
