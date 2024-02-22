namespace FinanceManager.Data.Models
{
	public class Transaction
	{
		public uint TransactionId { get; set; }

		public bool IsExpense { get; set; } = true;

		public DateTime Date { get; set; } = DateTime.Today;

		public Category Category { get; set; }

		public decimal Amount { get; set; }

		public string? Comment { get; set; }

		public const decimal MaxAmount = 9_999_999_999_999.99m;

		public Transaction() { }

		public Transaction(Transaction other)
		{
			TransactionId = other.TransactionId;
			IsExpense = other.IsExpense;
			Date = other.Date;
			Category = other.Category;
			Amount = other.Amount;
			Comment = other.Comment;
		}

		public override bool Equals(object? obj)
		{
			return obj is Transaction transaction &&
				transaction.TransactionId == TransactionId && transaction.IsExpense == IsExpense &&
				transaction.Date == Date && transaction.Category.Equals(Category) &&
				transaction.Amount == Amount && transaction.Comment == Comment;
		}

	}
}
