using HotelBookingApp.Model;

namespace HotelBookingApp.Interface
{
    public interface ICustomerRepository
    {
        Customer? GetById(int customerId);
        List<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int customerId);
        bool IsEmailUnique(string email);
    }
}
