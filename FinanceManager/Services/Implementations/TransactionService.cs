using FinanceManager.Data.Exceptions;
using FinanceManager.Data.Models;
using FinanceManager.Data.Repositories.Interfaces;
using FinanceManager.Exceptions;
using FinanceManager.Services.Interfaces;

namespace FinanceManager.Services.Implementations
{
	public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository = transactionRepository;

		public async Task<bool> CreateTransactionAsync(Transaction transaction)
		{
			if (transaction.Category == null)
			{
				throw new ArgumentNullException("Category cannot be empty", innerException: null);
			}
			if (transaction.Date > DateTime.Now)
			{
				throw new ArgumentException("Transaction's date cannot be later than the current date");
			}
			if (transaction.Amount <= 0)
			{
				throw new ArgumentException("Amount must be a positive number");
			}
			try
			{
				return await _transactionRepository.CreateTransactionAsync(transaction);
			}
			catch (RepositoryException)
			{
				throw new NotFoundException("Category with that id does not exist");
			}
		}

		public async Task<bool> DeleteTransactionAsync(uint id)
		{
			return await _transactionRepository.DeleteTransactionAsync(id);
		}

		public async Task<Transaction?> GetTransactionByIdAsync(uint id)
		{
			return await _transactionRepository.GetTransactionByIdAsync(id);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
		{
			return await _transactionRepository.GetTransactionsAsync();
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to)
		{
			if (from > to)
			{
				throw new ArgumentException("'from' date cannot be later than 'to' date.");
			}
			return await _transactionRepository.GetTransactionsAsync(from, to);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(bool expenses)
		{
			return await _transactionRepository.GetTransactionsAsync(expenses);
		}
		public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime from, DateTime to, bool expenses)
		{
			if (from > to)
			{
				throw new ArgumentException("'from' date cannot be later than 'to' date.");
			}
			return await _transactionRepository.GetTransactionsAsync(from, to, expenses);
		}

		public async Task<bool> UpdateTransactionAsync(Transaction transaction)
		{
			if (transaction.Category == null)
			{
				throw new ArgumentNullException("Category cannot be empty.", innerException: null);
			}
			if (transaction.Date > DateTime.Now)
			{
				throw new ArgumentException("Transaction's date cannot be later than the current date");
			}
			if (transaction.Amount <= 0)
			{
				throw new ArgumentException("Amount must be a positive number");
			}
			try
			{
				return await _transactionRepository.UpdateTransactionAsync(transaction);
			}
			catch (RepositoryException)
			{
				throw new NotFoundException("Category with that id does not exist.");
			}
		}
	}
}
