
namespace OneBeyondApi.DataAccess
{
	public interface IReserveReporsitory
	{
		string GetAvailability(Guid bookId, Guid? borrowerId);
		string ReserveBook(Guid borrowerId, Guid bookId);
	}
}