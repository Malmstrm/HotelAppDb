using HotelAppDb.Data;
using HotelAppDb.Interfaces;
using HotelBookingApp.Model;

namespace HotelAppDb.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Customer> GetAll() => _dbContext.Customer.ToList();

        public Customer? GetById(int customerId) => _dbContext.Customer.FirstOrDefault(c => c.CustomerId == customerId);

        public void Add(Customer customer)
        {
            _dbContext.Customer.Add(customer);
            _dbContext.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _dbContext.Customer.Update(customer);
            _dbContext.SaveChanges();
        }

        public void Delete(int customerId)
        {
            var customer = GetById(customerId);
            if (customer != null)
            {
                _dbContext.Customer.Remove(customer);
                _dbContext.SaveChanges();
            }
        }
        public bool IsEmailUnique(string email) => !_dbContext.Customer.Any(c => c.Email.ToLower() == email.ToLower());

    }
}
