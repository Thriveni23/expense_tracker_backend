// Services/SourceService.cs
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SourceDTO>> GetSourcesAsync()
        {
            var sources = await _unitOfWork.Sources.GetAllAsync();
            return sources.Select(s => new SourceDTO
            {
                Id = s.Id,
                SourceType = s.SourceType
            }).ToList();
        }

        public async Task<SourceDTO> AddSourceAsync(SourceDTO sourceDto)
        {
            var source = new Source
            {
                SourceType = sourceDto.SourceType
            };

            await _unitOfWork.Sources.AddAsync(source);
            await _unitOfWork.CompleteAsync();

            return new SourceDTO
            {
                Id = source.Id,
                SourceType = source.SourceType
            };
        }

        public async Task<bool> DeleteSourceAsync(int id)
        {
            var source = await _unitOfWork.Sources.GetByIdAsync(id);
            if (source == null) return false;

            _unitOfWork.Sources.Delete(source);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
