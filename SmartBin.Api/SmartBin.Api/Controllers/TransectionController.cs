using Microsoft.AspNetCore.Mvc;
using SmartBin.Api.Services;

namespace SmartBin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] int binId, [FromQuery] int userId, [FromQuery] int pageNumber = 1)
        {
            // Input validation in the controller
            int pageSize = 10;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var response = await _transactionService.GetTransactionsAsync(binId, userId, pageNumber, pageSize);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromForm] int binId, [FromForm] string token, IFormFile? image, [FromForm] int? materialId)
        {
            var transactionId = await _transactionService.AddTransactionAsync(binId, token, image, materialId);

            if (transactionId == null)
            {
                return NotFound("Bin not found or invalid token.");
            }

            return Ok(new { Message = "Transaction added successfully.", TransactionId = transactionId });
        }
    }
}