using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OnLoneController : ControllerBase
	{
		private readonly ILogger<OnLoneController> _logger;
		private readonly ILoanRepository _loanRepository;

		public OnLoneController(ILogger<OnLoneController> logger, ILoanRepository loanRepository)
		{
			_logger = logger;
			_loanRepository = loanRepository;
		}

		[HttpGet]
		[Route("GetLoans")]
		public IList<BookStock> Get()
		{
			List<BookStock> loans = new List<BookStock>();

			loans = _loanRepository.GetLoans();

			return loans;
		}

		[HttpPost]
		[Route("ReturnLoan")]
		public string Post(Guid loanGuid)
		{
			return _loanRepository.ReturnLoan(loanGuid);
		}
	}
}
