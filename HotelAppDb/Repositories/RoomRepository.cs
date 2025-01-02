using HotelAppDb.Data;
using HotelAppDb.Interfaces;
using HotelBookingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelAppDb.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoomRepository(ApplicationDbContext dbContent)
        {
            _dbContext = dbContent;
        }
        public List<Room> GetAll() => _dbContext.Room
            .OrderBy(r => r.RoomNumber)
            .ToList();
        public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut) => _dbContext.Room
            .Include(r => r.Bookings)
            .Where(r => r.Bookings.All(b =>
                checkOut <= b.CheckInDate || checkIn >= b.CheckOutDate))
            .ToList();

        public Room? GetById(int roomId) => _dbContext.Room.FirstOrDefault(r => r.RoomId == roomId);
        public bool ExistsByRoomNumber(int roomNumber) => _dbContext.Room.Any(r => r.RoomNumber == roomNumber);
        public Room? GetByRoomNumber(int roomNumber) => _dbContext.Room.FirstOrDefault(r => r.RoomNumber == roomNumber);
        public void Add(Room room)
        {
            _dbContext.Room.Add(room);
            _dbContext.SaveChanges();
        }
        public void Update(Room room)
        {
            _dbContext.Room.Update(room);
            _dbContext.SaveChanges();
        }
        public void Delete(int roomID)
        {
            var room = GetById(roomID);
            if (room != null)
            {
                _dbContext.Room.Remove(room);
                _dbContext.SaveChanges();
            }
        }
    }
}
