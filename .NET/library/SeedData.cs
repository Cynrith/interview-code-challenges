using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi
{
    public class SeedData
    {
        public static void SetInitialData()
        {
            var ernestMonkjack = new Author
            {
                Name = "Ernest Monkjack"
            };
            var sarahKennedy = new Author
            {
                Name = "Sarah Kennedy"
            };
            var margaretJones = new Author
            {
                Name = "Margaret Jones"
            };
			      var BrandonSanderson = new Author
			      {
				      Name = "Brandon Sanderson"
						};

			var clayBook = new Book
            {
                Name = "The Importance of Clay",
                Format = BookFormat.Paperback,
                Author = ernestMonkjack,
                ISBN = "1305718181"
            };

            var agileBook = new Book
            {
                Name = "Agile Project Management - A Primer",
                Format = BookFormat.Hardback,
                Author = sarahKennedy,
                ISBN = "1293910102"
            };

            var rustBook = new Book
            {
                Name = "Rust Development Cookbook",
                Format = BookFormat.Paperback,
                Author = margaretJones,
                ISBN = "3134324111"
            };

			      var goodBook = new Book
			      {
				      Name = "The Way of Kings",
				      Format = BookFormat.Paperback,
				      Author = BrandonSanderson,
				      ISBN = "9780765365279"
						};

			var daveSmith = new Borrower
            {
                Name = "Dave Smith",
                EmailAddress = "dave@smithy.com",
                Fines = []
            };

            var lianaJames = new Borrower
            {
                Name = "Liana James",
                EmailAddress = "liana@gmail.com",
                Fines = []
            };

            var bookOnLoanUntilToday = new BookStock {
                Book = clayBook,
                OnLoanTo = daveSmith,
                LoanEndDate = DateTime.Now.Date
            };

			      var anotherBookOnLone = new BookStock
			      {
				      Book = goodBook,
				      OnLoanTo = daveSmith,
				      LoanEndDate = DateTime.Now.Date.AddDays(1)
			      };

						var bookNotOnLoan = new BookStock
            {
                Book = clayBook,
                OnLoanTo = null,
                LoanEndDate = null
            };

            var bookOnLoanUntilNextWeek = new BookStock
            {
                Book = agileBook,
                OnLoanTo = lianaJames,
                LoanEndDate = DateTime.Now.Date.AddDays(-7)
            };

            var rustBookStock = new BookStock
            {
                Book = rustBook,
                OnLoanTo = null,
                LoanEndDate = null
            };

            using (var context = new LibraryContext())
            {
                context.Authors.Add(ernestMonkjack);
                context.Authors.Add(sarahKennedy);
                context.Authors.Add(margaretJones);
                context.Authors.Add(BrandonSanderson);


                context.Books.Add(clayBook);
                context.Books.Add(agileBook);
                context.Books.Add(rustBook);
                context.Books.Add(goodBook);

                context.Borrowers.Add(daveSmith);
                context.Borrowers.Add(lianaJames);

                context.Catalogue.Add(bookOnLoanUntilToday);
                context.Catalogue.Add(bookNotOnLoan);
                context.Catalogue.Add(bookOnLoanUntilNextWeek);
                context.Catalogue.Add(rustBookStock);
                context.Catalogue.Add(anotherBookOnLone);

                context.SaveChanges();

            }
        }
    }
}
