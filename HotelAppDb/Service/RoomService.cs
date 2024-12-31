using HotelAppDb.Interfaces;
using HotelAppDb.Repositories;
using HotelBookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppDb.Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public List<Room> GetAllRooms() => _roomRepository.GetAll();
        public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut) => _roomRepository.GetAvailableRooms(checkIn, checkOut);
        public Room GetRoomById(int roomId) => _roomRepository.GetById(roomId);
        public Room? GetRoomByNumber(int roomNumber) => _roomRepository.GetByRoomNumber(roomNumber);
        public void AddRoom(int roomNumber, string type, decimal price, int squareMeter)
        {
            // Kontrollera om rummet redan existerar
            if (_roomRepository.ExistsByRoomNumber(roomNumber))
            {
                throw new ArgumentException($"Room with number {roomNumber} already exists.");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Room type cannot be empty.");
            }

            var room = new Room()
            {
                RoomNumber = roomNumber,
                Type = type,
                Price = price,
                SquareMeter = squareMeter,
                IsAvailible = true
            };

            _roomRepository.Add(room);
        }
        public void UpdateRoom(int roomNumber, string? type = null, decimal? price = null, int? squareMeter = null)
        {
            var room = _roomRepository.GetByRoomNumber(roomNumber);
            if (room == null)
                throw new ArgumentException($"No room was found with that number: {roomNumber}");

            // Uppdatera endast de fält som inte är null
            room.Type = type ?? room.Type;
            room.Price = price ?? room.Price;
            room.SquareMeter = squareMeter ?? room.SquareMeter;

            _roomRepository.Update(room);
        }
        public void DeleteRoom(int roomNumber)
        {
            var room = _roomRepository.GetByRoomNumber(roomNumber);
            if (room == null)
                throw new ArgumentException($"No room was found witht that number: {roomNumber}");
            _roomRepository.Delete(roomNumber);
        }

    }
}
