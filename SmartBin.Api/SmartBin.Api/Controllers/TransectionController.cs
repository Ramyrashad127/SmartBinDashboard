using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBin.Api.Data;
using SmartBin.Api.Models;

namespace SmartBin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransectionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransectionController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("/api/transection")]
        public async Task<IActionResult> GetTransections(int BinId, int UserId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var baseQuery = _context.Transections
                .Where(t => t.Bin.UserId == UserId && t.BinId == BinId);

            var totalRecords = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var transections = await baseQuery
                .OrderByDescending(t => t.TimeStmp)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)
                .Select(t => new
                {
                    Timestamp = t.TimeStmp,
                    MaterialName = t.Material.Name,
                    AiConfidence = t.AiConfidence
                })
                .ToListAsync();

            var response = new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = transections
            };

            return Ok(response);
        }

        [HttpPost("/api/transection")]
        public async Task<IActionResult> AddTransection(int BinId, string Token, IFormFile? image, int? MaterialId)
        {
            var bin = await _context.Bins.FirstOrDefaultAsync(b => b.Id == BinId && b.IdentificationToken == Token);
            if (bin == null)
            {
                return NotFound("Bin not found or invalid token.");
            }
            Transection transection;
            if (image == null || image.Length == 0)
            {
                transection = new Transection
                {
                    BinId = BinId,
                    TimeStmp = DateTime.UtcNow,
                    MaterialId = MaterialId ?? 1
                };
            }
            else 
            {
                transection = new Transection
                {
                    BinId = BinId,
                    TimeStmp = DateTime.UtcNow,
                    AiConfidence = 0.85f, // Simulated confidence
                    MaterialId = 1 // Simulated material (e.g., metal)
                };
            }
            _context.Transections.Add(transection);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Transection added successfully.", TransectionId = transection.Id });
        }

    }
}
