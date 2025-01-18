namespace EXONLOG.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EXONLOG.Data;
    using EXONLOG.Model.Stocks;
    using Microsoft.EntityFrameworkCore;

    public class StockService
    {
        private readonly EXONContext _context;

        public StockService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.Include(s => s.Material).Include(s => s.User).ToListAsync();
        }

        public async Task<Stock> GetStockDetailsAsync(int stockId)
        {
            return await _context.Stocks.Include(s => s.Material).Include(s => s.User)
                                         .FirstOrDefaultAsync(s => s.StockID == stockId);
        }

        public async Task CreateStockAsync(Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(Stock stock)
        {
            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(int stockId)
        {
            var stock = await _context.Stocks.FindAsync(stockId);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }
    }

}
