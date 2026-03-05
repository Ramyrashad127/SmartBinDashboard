using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBin.Api.Data;
using SmartBin.Api.Models;

namespace SmartBin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BinController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BinController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("/api/bin")]
        public async Task<IActionResult> GetBins(int UserId)
        {
            var bins = await _context.Bins.Where(b => b.UserId == UserId).ToListAsync();
            return Ok(bins);
        }
        [HttpPost("/api/bin")]
        public async Task<IActionResult> CreateBin(int UserId)
        {
            Bin bin = new Bin
            {
                IdentificationToken = Guid.NewGuid().ToString(),
                Latitude = 0,
                Longitude = 0,
                UserId = UserId,
                User = await _context.Users.FindAsync(UserId)
            };
            bin.CreatedAt = DateTime.UtcNow;

            _context.Bins.Add(bin);
            await _context.SaveChangesAsync();
            BinSection section1 = new BinSection
            {
                BinId = bin.Id,
                MaterialId = 0, // plastic
                LevelPercentage = 0,
                Weight = 0
            };
            _context.BinSections.Add(section1);
            BinSection section2 = new BinSection
            {
                BinId = bin.Id,
                MaterialId = 1, // metal
                LevelPercentage = 0,
                Weight = 0
            };
            _context.BinSections.Add(section2);
            await _context.SaveChangesAsync();
            return Ok(bin);
        }

        [HttpPut("/api/bin")]
        public async Task<IActionResult> UpdateBinLocation(int BinId,float Latitude, float Longitude, string token)
        {
            var bin = await _context.Bins.FirstOrDefaultAsync(b => b.Id == BinId && b.IdentificationToken == token);
            if (bin == null) return NotFound();
            bin.Latitude = Latitude;
            bin.Longitude = Longitude;
            await _context.SaveChangesAsync();
            return Ok(bin);
        }

        [HttpGet("/api/bin/section/")]
        public async Task<IActionResult> GetBinSections(int BinId, int UserId)
        {
            var bin = await _context.Bins.Include(b => b.BinSections).FirstOrDefaultAsync(b => b.Id == BinId && b.UserId == UserId);
            if (bin == null) return NotFound();
            return Ok(bin);
        }
    }
}