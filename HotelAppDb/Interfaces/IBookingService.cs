using HotelBookingApp.Model;

namespace HotelAppDb.Interfaces
{
    public interface IBookingService
    {
        Booking? GetBookingById(int bookingId);
        List<Booking> GetAllBookings();
        void AddBooking(int customerId, int roomId, DateTime checkIn, DateTime checkOut, int? extraBeds = null);
        void UpdateBooking(int bookingId, int roomId, DateTime? checkIn = null, DateTime? checkOut = null);
        void DeleteBooking(int bookingId);
        bool IsDateValid(DateTime date);
        //List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
    }
}
