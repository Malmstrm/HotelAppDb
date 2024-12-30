namespace HotelBookingApp.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights
        {
            get
            {
                return (CheckOutDate - CheckInDate).Days;
            }
        }
        public int ExtraBeds { get; set; }
    }
}