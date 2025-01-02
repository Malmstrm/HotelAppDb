using HotelAppDb.Data;
using HotelAppDb.Interfaces;
using HotelBookingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelAppDb.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BookingRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public List<Booking> GetAll()
        {
            return _dbContext.Booking
                .Include(b => b.Customer) // Inkludera kunddata
                .Include(b => b.Room)     // Inkludera rumsdata
                .Where(b => b.CheckOutDate >= DateTime.Today) // Visa endast aktiva bokningar
                .OrderBy(b => b.CheckInDate)
                .ToList();
        }

        public Booking? GetById(int bookingId) => _dbContext.Booking
            .FirstOrDefault(b => b.BookingId == bookingId);
        public void Add(Booking booking)
        {
            _dbContext.Booking.Add(booking);
            _dbContext.SaveChanges();
        }
        public void Update(Booking booking)
        {
            _dbContext.Booking.Update(booking);
            _dbContext.SaveChanges();
        }
        public void Delete(int bookingId)
        {
            var booking = GetById(bookingId);
            if (booking != null)
            {
                _dbContext.Booking.Remove(booking);
                _dbContext.SaveChanges();
            }
        }
    }
}
