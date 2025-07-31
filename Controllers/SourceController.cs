using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Services;
using ExpenseTrackerCrudWebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [ApiController]
   
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly ILogger<SourceController> _logger;

        public SourceController(ISourceService sourceService, ILogger<SourceController> logger)
        {
            _sourceService = sourceService;
            _logger = logger;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSources()
        {
            _logger.LogInformation("Fetching all sources");
            try
            {
                var sources = await _sourceService.GetSourcesAsync();
                return Ok(sources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sources");
                return StatusCode(500, "An error occurred while retrieving sources.");
            }
        }

        [HttpPost("add")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddSource([FromBody] SourceDTO sourceDto)
        {
            _logger.LogInformation("Adding new source: {SourceType}", sourceDto.SourceType);
            try
            {
                var added = await _sourceService.AddSourceAsync(sourceDto);
                return Ok(added);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding source: {SourceType}", sourceDto.SourceType);
                return StatusCode(500, "An error occurred while adding the source.");
            }
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            _logger.LogInformation("Deleting source with ID: {SourceId}", id);
            try
            {
                var deleted = await _sourceService.DeleteSourceAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting source with ID {SourceId}", id);
                return StatusCode(500, "An error occurred while deleting the source.");
            }
        }
    }
}
