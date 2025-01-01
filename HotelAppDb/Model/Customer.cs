using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Model
{
    public class Customer
    {
        public int CustomerId { get; set; } // Primärnyckel

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(25, ErrorMessage = "First Name must be less than 25 characters.")]
        public string FirstName { get; set; } = string.Empty; // Säkerställ att null-värden inte används

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(25, ErrorMessage = "Last Name must be less than 25 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(128, ErrorMessage = "Email must be less than 128 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(12)] // Maximal längd för telefonnummer
        [RegularExpression(@"^\d{1,12}$", ErrorMessage = "Phone number must contain up to 12 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } // Anger att det är obligatoriskt och inte null

        public ICollection<Booking>? Bookings { get; set; } // Navigering till bokningar

    }
}