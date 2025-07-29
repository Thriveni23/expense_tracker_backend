using ExpenseTrackerCrudWebAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface ISourceService
    {
        Task<List<SourceDTO>> GetSourcesAsync(); // Return list of DTOs
        Task<SourceDTO> AddSourceAsync(SourceDTO sourceDto); // Accept create DTO, return DTO
        Task<bool> DeleteSourceAsync(int id); // No change
    }
}
