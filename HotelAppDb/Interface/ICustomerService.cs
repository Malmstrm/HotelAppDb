using HotelBookingApp.Model;
namespace HotelBookingApp.Interface
{
    public interface ICustomerService
    {
        Customer? GetCustomerById(int customerId);
        List<Customer> GetAllCustomers();
        void AddCustomer(string firstName, string lastName, string email, string phoneNumber);
        void UpdateCustomer(int customerId, string? firstName, string? lastName, string? email, string? phoneNumber);
        void DeleteCustomer(int customerId);
    }
}