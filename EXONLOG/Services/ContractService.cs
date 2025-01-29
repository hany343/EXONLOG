﻿namespace EXONLOG.Services
{
    using EXONLOG.Components.Outbound.ContractPages;
    using EXONLOG.Data;
    using EXONLOG.Model.Outbound;
    using Microsoft.EntityFrameworkCore;

    public class ContractService
    {
        private readonly EXONContext _context;

        public ContractService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> GetActiveContractListAsync()
        {
            try
            {
                List<Contract> contracts = await _context.Contracts
                    .Select(c => new Contract
                    {
                        ContractID = c.ContractID,
                        ContractNumber = c.ContractNumber
                    })
                    .ToListAsync();
                return contracts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Contract>> GetAllContractsAsync()
        {
            try
            {
                List<Contract> contracts = await _context.Contracts
                  .Include(c => c.Material)
                  .Include(c => c.Customer)
                  .Include(c => c.User).ToListAsync();

                return contracts;
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Error fetching contracts: {ex.Message}");
                throw;
            }
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Material)
                .Include(c => c.Customer)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ContractID == id);
        }

        public async Task AddContractAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContractAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }
    }

}
