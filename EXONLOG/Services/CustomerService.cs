namespace EXONLOG.Services
{
    using EXONLOG.Data;
    using EXONLOG.Model.Outbound;
    using EXONLOG.Model.Stocks;
    using EXONLOG.Model.Trans;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class CustomerService
    {
        private readonly EXONContext _context;

        public CustomerService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Customer>(); // Return empty list if search term is null or empty
            }
            var customers = _context.Customers
               .Where(d => EF.Functions.Like(d.CustomerName, $"%{searchTerm}%") ).Take(5)
               .ToList();
            return customers;
        }
        
        public async Task AddCustomerAsync(Customer customer)
        {
            customer.UserID = 1;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }

}
