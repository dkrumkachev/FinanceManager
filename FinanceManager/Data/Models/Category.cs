namespace FinanceManager.Data.Models
{
	public class Category
	{
		public uint CategoryId { get; set; }

		public bool IsForExpenses { get; set; }

		public string Name { get; set; } = string.Empty;

		public Category() { }

		public Category(Category other)
		{
			CategoryId = other.CategoryId;
			IsForExpenses = other.IsForExpenses;
			Name = other.Name;
		}

		public override bool Equals(object? obj)
		{
			return obj is Category other &&
				CategoryId == other.CategoryId &&
				IsForExpenses == other.IsForExpenses &&
				Name == other.Name;
		}

		public override int GetHashCode()
		{
			return CategoryId.GetHashCode();
		}
	}
}
