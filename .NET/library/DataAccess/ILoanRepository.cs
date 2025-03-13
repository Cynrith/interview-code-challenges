using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
	public interface ILoanRepository
	{
		List<BookStock> GetLoans();
		string ReturnLoan(Guid loanGuid);
	}
}