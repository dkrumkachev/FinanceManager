using FinanceManager.Data.Models;

namespace FinanceManager.Data.Repositories.Interfaces
{
	public interface ITransactionRepository
	{
		public Task<Transaction?> GetTransactionByIdAsync(uint id);

		public Task<IEnumerable<Transaction>> GetTransactionsAsync();

		public Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to);

		public Task<IEnumerable<Transaction>> GetTransactionsAsync(bool expenses);

		public Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to, bool expenses);

		public Task<bool> CreateTransactionAsync(Transaction transaction);

		public Task<bool> DeleteTransactionAsync(uint id);

		public Task<bool> UpdateTransactionAsync(Transaction transaction);
	}
}
