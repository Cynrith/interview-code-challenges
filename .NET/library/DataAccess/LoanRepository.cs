using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
	public class LoanRepository : ILoanRepository
	{
		public LoanRepository() { }

		public List<BookStock> GetLoans()
		{
			var loans = new List<BookStock>();

			using (var context = new LibraryContext())
			{
				var stocks = context.BookStocks.Include(b => b.Book).Include(b => b.OnLoanTo).ToList();

				foreach (var stock in stocks)
				{
					if(stock.OnLoanTo != null)
					{
						loans.Add(stock);
					}
				}

				return loans;
			}
		}

		public string ReturnLoan(Guid loanGuid)
		{
			using (var context = new LibraryContext())
			{
				var target = context.BookStocks
					.Include(b => b.Book)
					.Include(b => b.OnLoanTo)
					.Include(b => b.OnLoanTo.Fines)
					.FirstOrDefault(b => b.Id == loanGuid);

				if (target != null)
				{
					string returnMessage = "Loan successfully returned.";

					if (target.LoanEndDate < DateTime.Now.Date)
					{
						target.OnLoanTo.Fines.Add(new Fine()
						{
							AmmountCharged = decimal.Parse("10.00"),
							Reason = "Late Return."
						});

						returnMessage = "You have been fined due to a late return.";
					}

					target.OnLoanTo = null;
					target.LoanEndDate = null;
					context.SaveChanges();

					return returnMessage;
				}
				else
				{
					return "Target not found.";
				}
			}
		}
	}
}
