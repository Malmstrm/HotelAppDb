using HotelAppDb.Interfaces;
using HotelAppDb.Repositories;
using HotelBookingApp.Model;
using System.Text.RegularExpressions;


namespace HotelAppDb.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAllCustomers() => _customerRepository.GetAll();
        public Customer? GetCustomerById(int customerId) => _customerRepository.GetById(customerId);

        public void AddCustomer(string firstName, string lastName, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("First name and last name are required.");
            }

            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            if (!_customerRepository.IsEmailUnique(email))
            {
                throw new ArgumentException("The email address is already in use. Please use a different email.");
            }

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                IsActive = true
            };

            _customerRepository.Add(customer);
        }
        public void UpdateCustomer(int customerId, string? firstName, string? lastName, string? email, string? phoneNumber)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
            {
                throw new ArgumentException($"No customer found with ID {customerId}.");
            }

            // Endast uppdatera fält som inte är null eller tomma
            if (!string.IsNullOrWhiteSpace(firstName)) customer.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) customer.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email)) throw new ArgumentException("Invalid email format.");
                customer.Email = email;
            }
            if (!string.IsNullOrWhiteSpace(phoneNumber)) customer.PhoneNumber = phoneNumber;

            _customerRepository.Update(customer);
        }


        public void DeleteCustomer(int customerId)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
            {
                throw new ArgumentException($"No customer found with ID {customerId}.");
            }

            _customerRepository.Delete(customerId);
        }

        public bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }
        public bool IsEmailUnique(string email) => _customerRepository.IsEmailUnique(email);
    }
}
