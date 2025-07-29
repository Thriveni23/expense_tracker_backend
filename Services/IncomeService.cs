using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IncomeDTO> CreateIncomeAsync(IncomeDTO incomeDto, string userId)
        {
            var income = new Income
            {
                Date = incomeDto.Date,
                Description = incomeDto.Description,
                Amount = incomeDto.Amount,
                Source = incomeDto.Source,
                UserId = userId
            };

            await _unitOfWork.Incomes.AddAsync(income);
            await _unitOfWork.CompleteAsync();

            incomeDto.Id = income.Id;  // Map back ID from DB
            incomeDto.UserId = userId;

            return incomeDto;
        }

        public async Task<IEnumerable<IncomeDTO>> GetAllIncomesAsync(string userId)
        {
            var incomes = await _unitOfWork.Incomes.GetAllAsync();
            return incomes.Where(i => i.UserId == userId)
                          .Select(i => new IncomeDTO
                          {
                              Id = i.Id,
                              Date = i.Date,
                              Description = i.Description,
                              Amount = i.Amount,
                              Source = i.Source,
                              UserId = i.UserId
                          });
        }

        public async Task<IncomeDTO?> GetIncomeByIdAsync(int id)
        {
            var income = await _unitOfWork.Incomes.GetByIdAsync(id);
            if (income == null) return null;

            return new IncomeDTO
            {
                Id = income.Id,
                Date = income.Date,
                Description = income.Description,
                Amount = income.Amount,
                Source = income.Source,
                UserId = income.UserId
            };
        }

        public async Task<IncomeDTO?> UpdateIncomeAsync(int id, IncomeDTO updatedIncomeDto, string userId)
        {
            var existing = await _unitOfWork.Incomes.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return null;

            existing.Description = updatedIncomeDto.Description;
            existing.Amount = updatedIncomeDto.Amount;
            existing.Source = updatedIncomeDto.Source;
            existing.Date = updatedIncomeDto.Date;

            _unitOfWork.Incomes.Update(existing);
            await _unitOfWork.CompleteAsync();

            return new IncomeDTO
            {
                Id = existing.Id,
                Date = existing.Date,
                Description = existing.Description,
                Amount = existing.Amount,
                Source = existing.Source,
                UserId = existing.UserId
            };
        }

        public async Task<bool> DeleteIncomeAsync(int id, string userId)
        {
            var income = await _unitOfWork.Incomes.GetByIdAsync(id);
            if (income == null || income.UserId != userId)
                return false;

            _unitOfWork.Incomes.Delete(income);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}