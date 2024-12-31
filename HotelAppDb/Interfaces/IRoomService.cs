using HotelAppDb.Repositories;
using HotelBookingApp.Model;

namespace HotelAppDb.Interfaces
{
    public interface IRoomService
    {
        Room? GetRoomById(int roomId);
        List<Room> GetAllRooms();
        List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
        void AddRoom(int roomNumber, string type, decimal price, int squareMeter);
        void UpdateRoom(int roomNumber, string? type = null, decimal? price = null, int? squareMeter = null);
        void DeleteRoom(int roomId);
        public Room? GetRoomByNumber(int roomNumber);
    }
}