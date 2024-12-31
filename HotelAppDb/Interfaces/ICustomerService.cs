using HotelAppDb.Repositories;
using HotelBookingApp.Model;
using System.Text.RegularExpressions;
namespace HotelAppDb.Interfaces
{
    public interface ICustomerService
    {
        Customer? GetCustomerById(int customerId);
        List<Customer> GetAllCustomers();
        void AddCustomer(string firstName, string lastName, string email, string phoneNumber);
        void UpdateCustomer(int customerId, string? firstName, string? lastName, string? email, string? phoneNumber);
        void DeleteCustomer(int customerId);
        public bool IsValidEmail(string email);
        public bool IsEmailUnique(string email);
    }
}