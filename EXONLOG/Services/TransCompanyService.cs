namespace EXONLOG.Services
{
    using EXONLOG.Data;
    using EXONLOG.Model.Trans;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TransCompanyService
    {
        private readonly EXONContext _dbContext;

        public TransCompanyService(EXONContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransCompany>> GetAllAsync()
        {
            return await _dbContext.TransCompanies.ToListAsync();
        }

        public async Task<TransCompany?> GetByIdAsync(int id)
        {
            return await _dbContext.TransCompanies.FindAsync(id);
        }

        public async Task AddAsync(TransCompany company)
        {
            company.CreateDate = DateTime.UtcNow;
            company.UserID = 1;
            _dbContext.TransCompanies.Add(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransCompany company)
        {
            var existingCompany = await _dbContext.TransCompanies.FindAsync(company.TransCompanyID);
            if (existingCompany != null)
            {
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.Address = company.Address;
                existingCompany.Phone = company.Phone;
                existingCompany.Email = company.Email;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _dbContext.TransCompanies.FindAsync(id);
            if (company != null)
            {
                _dbContext.TransCompanies.Remove(company);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
