using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;

namespace OneBeyondApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReserveController : ControllerBase
	{
		private readonly ILogger<ReserveController> _logger;
		private readonly IReserveReporsitory _reserveRepository;

		public ReserveController(ILogger<ReserveController> logger, IReserveReporsitory reserveRepository)
		{
			_logger = logger;
			_reserveRepository = reserveRepository;
		}

		[HttpGet]
		[Route("GetAvailabilityDate")]
		public string GetAvailabilityDate(Guid bookId, Guid? borrowerId = null)
		{
			return _reserveRepository.GetAvailability(bookId, borrowerId);
		}

		[HttpPost]
		[Route("ReserveBook")]
		public string ReserveBook(Guid borrowerID, Guid bookId)
		{
			return _reserveRepository.ReserveBook(borrowerID, bookId);
		}
	}
}
