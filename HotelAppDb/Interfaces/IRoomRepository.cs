using HotelBookingApp.Model;

namespace HotelAppDb.Interfaces
{
    public interface IRoomRepository
    {
        Room? GetById(int roomId);
        Room? GetByRoomNumber(int roomNumber);
        List<Room> GetAll();
        List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
        public bool ExistsByRoomNumber(int roomNumber);
        void Add(Room room);
        void Update(Room room);
        void Delete(int roomId);
    }
}