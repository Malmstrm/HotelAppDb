using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Model
{
    public class Booking
    {
        public int BookingId { get; set; } // Primary Key

        [Required]
        public int CustomerId { get; set; } // Foreign Key till Customer

        public Customer? Customer { get; set; } // Navigeringsproperty till Customer

        [Required]
        public int RoomId { get; set; } // Foreign Key till Room

        public Room? Room { get; set; } // Navigeringsproperty till Room

        [Required]
        public DateTime CheckInDate { get; set; } // Datum för incheckning

        [Required]
        public DateTime CheckOutDate { get; set; } // Datum för utcheckning

        [NotMapped] // Beräknad egenskap som ej lagras i databasen
        public int NumberOfNights
        {
            get
            {
                return (CheckOutDate - CheckInDate).Days;
            }
        }

        [Range(0, 5, ErrorMessage = "Extra beds must be between 0 and 5.")]
        public int ExtraBeds { get; set; } // Extra sängar
    }

}