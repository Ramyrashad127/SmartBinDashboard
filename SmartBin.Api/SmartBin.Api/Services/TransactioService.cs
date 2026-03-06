using Microsoft.EntityFrameworkCore;
using SmartBin.Api.Data;
using SmartBin.Api.DTOs;
using SmartBin.Api.Models;
namespace SmartBin.Api.Services
{
    public interface ITransactionService
    {
        Task<PaginatedResponse<TransactionDto>> GetTransactionsAsync(int binId, int userId, int pageNumber, int pageSize);
        Task<int?> AddTransactionAsync(int binId, string token, IFormFile? image, int? materialId);
    }

    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<TransactionDto>> GetTransactionsAsync(int binId, int userId, int pageNumber, int pageSize)
        {
            var baseQuery = _context.Transactions // Assuming table name is corrected to Transactions
                .Where(t => t.Bin.UserId == userId && t.BinId == binId);

            var totalRecords = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var transactions = await baseQuery
                .OrderByDescending(t => t.TimeStmp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionDto
                {
                    Timestamp = t.TimeStmp,
                    MaterialName = t.Material.Name,
                    AiConfidence = t.AiConfidence
                })
                .ToListAsync();

            return new PaginatedResponse<TransactionDto>
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = transactions
            };
        }

        public async Task<int?> AddTransactionAsync(int binId, string token, IFormFile? image, int? materialId)
        {
            var binExists = await _context.Bins.AnyAsync(b => b.Id == binId && b.IdentificationToken == token);
            if (!binExists) return null;

            var transaction = new Transaction
            {
                BinId = binId,
                TimeStmp = DateTime.UtcNow
            };

            if (image == null || image.Length == 0)
            {
                transaction.MaterialId = materialId ?? (int)MaterialType.Plastic;
            }
            else
            {
                // TODO: Future AI integration logic goes here
                transaction.AiConfidence = 0.85f;
                transaction.MaterialId = (int)MaterialType.Metal;
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction.Id;
        }
    }
}
