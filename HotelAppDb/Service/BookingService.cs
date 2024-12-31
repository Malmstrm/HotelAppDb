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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public List<Booking> GetAllBookings() => _bookingRepository.GetAll();
        public Booking? GetBookingById(int bookingId) => _bookingRepository.GetById(bookingId);
        public void AddBooking(int selected, int roomId, DateTime checkIn, DateTime checkOut, int? extraBeds = null)
        {
            int extraBedsValue = extraBeds ?? 0;

            var booking = new Booking()
            {
                CustomerId = selected,
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                ExtraBeds = extraBedsValue
            };
            _bookingRepository.Add(booking);
        }
        public void UpdateBooking(int bookingId, int roomId, DateTime? checkIn = null, DateTime? checkOut = null)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
                throw new ArgumentException($"No booking with that ID {bookingId}");
            booking.RoomId = roomId;
            booking.CheckInDate = checkIn ?? booking.CheckInDate;
            booking.CheckOutDate = checkOut ?? booking.CheckOutDate;

            _bookingRepository.Update(booking);
        }
        public void DeleteBooking(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
                throw new ArgumentException($"No booking found with ID {bookingId}");
            _bookingRepository.Delete(bookingId);
        }
        public bool IsDateValid(DateTime selectedDate)
        {
            DateTime today = DateTime.Today;
            return selectedDate >= today;
        }
    }
}
