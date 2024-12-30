using HotelBookingApp.Model;

namespace HotelBookingApp.Interface
{
    public interface IRoomRepository
    {
        Room? GetById(int roomId);
        Room? GetByRoomNumber(int roomNumber);
        List<Room> GetAll();
        List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
        void Add(Room room);
        void Update(Room room);
        void Delete(int roomId);
    }
}



