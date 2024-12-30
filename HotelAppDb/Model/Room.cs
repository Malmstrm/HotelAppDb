namespace HotelBookingApp.Model
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int SquareMeter { get; set; }
        public string? Type { get; set; } // Single, Double, Suite
        public decimal Price { get; set; }
        public bool IsAvailible { get; set; }
        public ICollection<Booking>? Bookings { get; set; }

    }
}

