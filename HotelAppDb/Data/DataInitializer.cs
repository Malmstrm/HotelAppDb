using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Model;

namespace HotelAppDb.Data
{
    public class DataInitializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedRooms(dbContext);
            SeedCustomers(dbContext);
            dbContext.SaveChanges();
        }
        private void SeedRooms(ApplicationDbContext dbContext)
        {
            if (!dbContext.Room.Any(r => r.RoomNumber == 101))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 101,
                    Type = "Double",
                    Price = 850,
                    SquareMeter = 25,
                    IsAvailible = true,
                });
            }
            if (!dbContext.Room.Any(r => r.RoomNumber == 102))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 102,
                    Type = "Single",
                    Price = 500,
                    SquareMeter = 15,
                    IsAvailible = true,
                });
            }
            if (!dbContext.Room.Any(r => r.RoomNumber == 103))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 103,
                    Type = "Single",
                    Price = 500,
                    SquareMeter = 15,
                    IsAvailible = true,
                });
            }
            if (!dbContext.Room.Any(r => r.RoomNumber == 201))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 201,
                    Type = "Single",
                    Price = 500,
                    SquareMeter = 15,
                    IsAvailible = true,
                });
            }
            if (!dbContext.Room.Any(r => r.RoomNumber == 202))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 202,
                    Type = "Single",
                    Price = 500,
                    SquareMeter = 15,
                    IsAvailible = true,
                });
            }
            if (!dbContext.Room.Any(r => r.RoomNumber == 203))
            {
                dbContext.Room.Add(new Room
                {
                    RoomNumber = 203,
                    Type = "Single",
                    Price = 500,
                    SquareMeter = 15,
                    IsAvailible = true,
                });
            }
        }
        private void SeedCustomers(ApplicationDbContext dbContext) // måste fundera över vad man kan göra här för ändra jag email på dessa bentliga så lägger den ny.
        {
            if (!dbContext.Customer.Any(c => c.Email.ToLower() == "emma.svensson@example.com"))
            {
                dbContext.Customer.Add(new Customer
                {
                    FirstName = "Emma",
                    LastName = "Svensson",
                    Email = "emma.svensson@example.com",
                    PhoneNumber = "0701234567",
                    IsActive = true,

                });
            }

            if (!dbContext.Customer.Any(c => c.Email.ToLower() == "stefan.malmstrom@example.com"))
            {
                dbContext.Customer.Add(new Customer
                {
                    FirstName = "Stefan",
                    LastName = "Malmström",
                    Email = "stefan.malmstrom@example.com",
                    PhoneNumber = "0734567890",
                    IsActive = true,
                });
            }

            if (!dbContext.Customer.Any(c => c.Email.ToLower() == "lisa.andersson@example.com"))
            {
                dbContext.Customer.Add(new Customer
                {
                    FirstName = "Lisa",
                    LastName = "Andersson",
                    Email = "lisa.andersson@example.com",
                    PhoneNumber = "0723456789",
                    IsActive = true,
                });
            }

            if (!dbContext.Customer.Any(c => c.Email.ToLower() == "karl.johansson@example.com"))
            {
                dbContext.Customer.Add(new Customer
                {
                    FirstName = "Karl",
                    LastName = "Johansson",
                    Email = "karl.johansson@example.com",
                    PhoneNumber = "0761234567",
                    IsActive = true,
                });
            }

            if (!dbContext.Customer.Any(c => c.Email.ToLower() == "anna.nilsson@example.com"))
            {
                dbContext.Customer.Add(new Customer
                {
                    FirstName = "Anna",
                    LastName = "Nilsson",
                    Email = "anna.nilsson@example.com",
                    PhoneNumber = "0799876543",
                    IsActive = true,
                });
            }
        }
    }
}
