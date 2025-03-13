using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
	public class ReserveReporsitory : IReserveReporsitory
	{
		public ReserveReporsitory() { }

		private bool getBookAvailabilityInfo(LibraryContext context, Guid bookId, out BookStock soonestReturn)
		{
			soonestReturn = null;

				var stocks = context.BookStocks
					.Include(b => b.Book)
					.Include(b => b.OnLoanTo)
					.AsQueryable();

				var availableBooks = stocks.Where(s => s.Book.Id == bookId);

				var bookFound = false;
				foreach (var availableBook in availableBooks)
				{
					bookFound = true;

					if (availableBook.OnLoanTo == null)
					{
						soonestReturn = null;
						return true;
					}
					else if (soonestReturn == null
						|| soonestReturn.LoanEndDate > availableBook.LoanEndDate)
					{
						soonestReturn = availableBook;
					}
				}

				return bookFound;
		}

		public string GetAvailability(Guid bookId, Guid? borrowerId)
		{
			string returnMessage = "Under Construction";

			using (var context = new LibraryContext())
			{
				BookStock soonestReturn = null;
				var bookFound = getBookAvailabilityInfo(context, bookId, out soonestReturn);

				if (bookFound && soonestReturn != null)
				{
					var reserves = context.BookReserves
						.Include(r => r.Book)
						.Include(r => r.Borrower)
						.AsQueryable();

					var existingReserves = reserves.Where(r => r.Book.Id == bookId);
					var lastPlaceInQueue = existingReserves.ToArray().Length;

					var estimatedAvailableDate = ((DateTime)soonestReturn.LoanEndDate);
					if (lastPlaceInQueue > 0)
					{
						if (borrowerId == null)
						{
							var lastInQueue = (existingReserves.Where(r => r.PlaceInQueue == lastPlaceInQueue)).ToArray()[0];
							estimatedAvailableDate = lastInQueue.EstimatedReturnDate;
						}
						else
						{
							var borrowerArray = (existingReserves.Where(r => r.Borrower.Id == borrowerId)).ToArray();
							if(borrowerArray.Length == 0)
							{
								return "Borrower not found for this book.";
							}

							var lastInQueue = borrowerArray[0];
							estimatedAvailableDate = lastInQueue.EstimatedReturnDate.AddDays(-7);
						}
					}

					returnMessage = "This book is estimated to be available from: " + estimatedAvailableDate.ToString("dd/MM/yyyy");
				}
				else
				{
					returnMessage = "Book available";
				}
			}

			return returnMessage;
		}

		public string ReserveBook(Guid borrowerId, Guid bookId)
		{
			string returnMessage = "Book not found.";

			using (var context = new LibraryContext())
			{
				BookStock soonestReturn = null;
				var bookFound = getBookAvailabilityInfo(context, bookId, out soonestReturn);

				if (bookFound && soonestReturn != null)
				{
					var reserves = context.BookReserves
						.Include(r => r.Book)
						.Include(r => r.Borrower)
						.AsQueryable();

					var existingReserves = reserves.Where(r => r.Book.Id == bookId);

					if (existingReserves.Where(r => r.Borrower.Id == borrowerId).Count() > 0)
					{
						return "You already have this book reserved.";
					}

					var lastPlaceInQueue = existingReserves.ToArray().Length;

					var estimatedReturnDate = ((DateTime)soonestReturn.LoanEndDate).AddDays(7);
					if (lastPlaceInQueue > 0)
					{
						var lastInQueue = (existingReserves.Where(r => r.PlaceInQueue == lastPlaceInQueue)).ToArray()[0];
						estimatedReturnDate = lastInQueue.EstimatedReturnDate.AddDays(7);
					}

					var borrowers = context.Borrowers
						.Include(b => b.Fines)
						.AsQueryable();

					var borrower = borrowers.Where(b => b.Id == borrowerId).ToArray()[0];

					context.BookReserves.Add(new BookReserve()
					{
						Book = soonestReturn.Book,
						EstimatedReturnDate = estimatedReturnDate,
						PlaceInQueue = lastPlaceInQueue + 1,
						Borrower = borrower,
					});
					context.SaveChanges();

					return "Book reserved.";
				}
				else if(bookFound && soonestReturn == null)
				{
					return "Book available.";
				}
			}

			return returnMessage;
		}
	}
}
