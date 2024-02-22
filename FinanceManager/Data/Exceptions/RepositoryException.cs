namespace FinanceManager.Data.Exceptions
{
	public class RepositoryException : Exception
	{
		public RepositoryException() : base() { }

		public RepositoryException(string message) : base(message) { }
	}
}
