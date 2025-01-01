using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Model
{
    public class Room
    {
        public int RoomId { get; set; } // Primary Key

        [Required] // Fältet är obligatoriskt
        [Range(100, 1000, ErrorMessage = "Room number must be between 100 and 1000.")]
        public int RoomNumber { get; set; } // Unikt rumsnummer

        [Required]
        [Range(1, 100, ErrorMessage = "Square meter must be between 1 and 100.")]
        public int SquareMeter { get; set; } // Max 100 kvadratmeter

        [Required]
        [StringLength(10, ErrorMessage = "Type must be 'Single', 'Double', or 'Suite'.")]
        public string Type { get; set; } = string.Empty; // Single, Double, Suite

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; } // Pris på rummet

        [Required]
        public bool IsAvailible { get; set; } // Är rummet tillgängligt?

        public ICollection<Booking>? Bookings { get; set; } // Relation till bokningar
    }
}

