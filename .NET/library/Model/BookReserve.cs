namespace OneBeyondApi.Model
{
	public class BookReserve
	{
		public Guid Id { get; set; }
		public int PlaceInQueue { get; set; }
		public Borrower Borrower { get; set; }
		public Book Book { get; set; }
		public DateTime EstimatedReturnDate { get; set; }
	}
}
