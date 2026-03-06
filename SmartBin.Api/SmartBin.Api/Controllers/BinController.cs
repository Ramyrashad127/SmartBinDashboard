using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBin.Api.DTOs;
using SmartBin.Api.Services;
using System.Security.Claims;

namespace SmartBin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BinController : ControllerBase
    {
        private readonly IBinService _binService;

        public BinController(IBinService binService)
        {
            _binService = binService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? throw new UnauthorizedAccessException());

        [HttpGet]
        public async Task<IActionResult> GetBins()
        {
            var bins = await _binService.GetBinsAsync(GetUserId());
            return Ok(bins);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBin()
        {
            var bin = await _binService.CreateBinAsync(GetUserId());
            return Ok(bin);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBinLocation([FromQuery] int binId, [FromQuery] float latitude, [FromQuery] float longitude, [FromQuery] string token)
        {
            var dto = new UpdateLocationDto { Token = token, Latitude = latitude, Longitude = longitude };
            var success = await _binService.UpdateLocationAsync(binId, dto);
            if (!success) return NotFound("Bin not found or token invalid.");

            var bin = await _binService.GetBinByIdAsync(binId);
            return Ok(bin);
        }

        [HttpGet("section")]
        public async Task<IActionResult> GetBinSections([FromQuery] int binId)
        {
            var bin = await _binService.GetBinSectionsAsync(binId, GetUserId());
            if (bin == null) return NotFound();

            return Ok(bin);
        }

        [HttpPut("section")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBinSection([FromQuery] int binId, [FromQuery] string token, [FromBody] BinSectionDto dto)
        {
            var success = await _binService.UpdateBinSectionAsync(binId, token, dto);
            if (!success) return NotFound("Bin or section not found, or token invalid.");

            var bin = await _binService.GetBinByIdAsync(binId);
            return Ok(bin);
        }
    }
}