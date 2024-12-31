using HotelBookingApp.Model;

namespace HotelAppDb.Interfaces
{
    public interface IBookingRepository
    {
        Booking? GetById(int bookingId);
        List<Booking> GetAll();
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(int bookingId);
        //List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
    }
}
