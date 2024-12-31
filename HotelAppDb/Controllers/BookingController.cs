using HotelAppDb.Interfaces;
using HotelAppDb.Service;
using HotelAppDb.Utilities;
using HotelBookingApp.Model;
using Spectre.Console;
using static HotelAppDb.Utilities.InputHandler;

namespace HotelAppDb.Controllers
{
    public class BookingController
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly RoomController _roomController;
        private readonly CustomerController _customerController;
        private readonly ICustomerService _customerService;
        public BookingController(IBookingService bookingService, IRoomService roomService, RoomController roomController, CustomerController customerController, ICustomerService customerService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _roomController = roomController;
            _customerController = customerController;
            _customerService = customerService;
        }
        public void DisplayBookingMenu()
        {
            // Skapa menyalternativen en gång
            var menuPrompt = new SelectionPrompt<string>()
                .Title("[yellow]Booking Management[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                .AddChoices(new[] 
                {
                    "Create Booking",
                    "View Bookings",
                    "Update Booking",
                    "Delete Booking",
                    "Back to Main Menu"
                });

            while (true)
            {
                Console.Clear();

                // Visa menyn och få användarens val
                var menuSelection = AnsiConsole.Prompt(menuPrompt);

                // Hantera användarens val
                switch (menuSelection)
                {
                    case "Create Booking":
                        CreateBooking();
                        break;
                    case "View Bookings":
                        ViewBookings();
                        AnsiConsole.MarkupLine("[green]Press any key to return to menu...[/]");
                        Console.ReadKey();
                        break;
                    case "Update Booking":
                        UpdateBooking();
                        break;
                    case "Delete Booking":
                        DeleteBooking();
                        break;
                    case "Back to Main Menu":
                        return; // Gå tillbaka till huvudmenyn
                }
            }
        }
        private void ViewBookings()
        {
            Console.Clear();

            // Hämta alla bokningar
            var bookings = _bookingService.GetAllBookings();

            if (bookings == null || bookings.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No bookings found.[/]");
                return;
            }

            // Skapa en tabell för att visa bokningar
            var table = new Table
            {
                Border = TableBorder.Rounded
            };

            // Lägg till rubriker
            table.AddColumn("[bold]ID[/]");
            table.AddColumn("[bold]Customer[/]");
            table.AddColumn("[bold]Room[/]");
            table.AddColumn("[bold]Check-In[/]");
            table.AddColumn("[bold]Check-Out[/]");
            table.AddColumn("[bold]Nights[/]");
            table.AddColumn("[bold]Price[/]");

            // Iterera genom bokningarna och lägg till rader i tabellen
            foreach (var booking in bookings)
            {
                var customerName = booking.Customer != null
                    ? $"{booking.Customer.FirstName} {booking.Customer.LastName}"
                    : "[grey]Unknown[/]";

                var roomInfo = booking.Room != null
                    ? $"[blue]#{booking.Room.RoomNumber}[/] ([green]{booking.Room.Type}[/])"
                    : "[grey]Unknown[/]";

                table.AddRow(
                    booking.BookingId.ToString(),
                    customerName,
                    roomInfo,
                    $"[green]{booking.CheckInDate:yyyy-MM-dd}[/]",
                    $"[red]{booking.CheckOutDate:yyyy-MM-dd}[/]",
                    booking.NumberOfNights.ToString(),
                    $"[cyan]{booking.NumberOfNights*booking.Room.Price:C}[/]"
                );
            }

            // Skriv ut tabellen
            AnsiConsole.Write(table);
        }

        private void DisplayBookingDetail(int bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                AnsiConsole.MarkupLine($"[bold red]Booking with ID {bookingId} not found.[/]");
                return;
            }

            // Skapa en tabell för att visa bokningsdetaljer
            var table = new Table
            {
                Border = TableBorder.Rounded
            };

            table.AddColumn("[bold]Property[/]");
            table.AddColumn("[bold]Value[/]");

            table.AddRow("Check-In Date", $"[green]{booking.CheckInDate:yyyy-MM-dd}[/]");
            table.AddRow("Check-Out Date", $"[green]{booking.CheckOutDate:yyyy-MM-dd}[/]");
            table.AddRow("Room ID", $"[blue]{booking.RoomId}[/]");
            table.AddRow("Customer", $"[yellow]{booking.Customer?.FirstName} {booking.Customer?.LastName}[/]");
            table.AddRow("Total Price", $"[bold cyan]{booking.NumberOfNights*booking.Room.Price}[/]");

            // Skriv ut tabellen
            AnsiConsole.Write(table);

            // Extra instruktioner för användaren
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }

        private void CreateBooking()
        {
            // Välj kund
            var customer = SelectCustomer();
            if (customer == null)
            {
                Console.WriteLine("No customer selected. Booking canceled.");
                return;
            }

            // Välj Check-In datum
            var checkIn = CheckDate("Enter Check-In date:");

            // Välj Check-Out datum
            var checkOut = CheckDate("Enter Check-Out date:", previousDate: checkIn);

            // Visa tillgängliga rum
            _roomController.ViewAvailableRooms(checkIn, checkOut);

            // Välj rum
            var room = SelectRoom(checkIn, checkOut);
            if (room == null)
            {
                Console.WriteLine("No room selected. Booking canceled.");
                return;
            }

            // Kontrollera om extrasäng behövs
            int extraBeds = 0;
            if (room.Type.Equals("Double", StringComparison.OrdinalIgnoreCase) && room.SquareMeter > 15)
            {
                extraBeds = InputHelper.GetInputWithValidation(
                    "Room allows extra beds. Enter number of extra beds (0-2): ",
                    "Invalid input. Please enter a number between 0 and 2.",
                    () => Console.WriteLine($"Room {room.RoomNumber} selected."),
                    input => int.TryParse(input, out int num) && num >= 0 && num <= 2,
                    int.Parse
                );
            }

            // Skapa bokningen
            try
            {
                _bookingService.AddBooking(customer.CustomerId, room.RoomId, checkIn, checkOut, extraBeds);
                Console.WriteLine($"Booking successfully created for Customer: {customer.FirstName} {customer.LastName}, Room: {room.RoomNumber}, Check-In: {checkIn:yyyy-MM-dd}, Check-Out: {checkOut:yyyy-MM-dd}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating booking: {ex.Message}");
            }
            finally
            {
                Thread.Sleep(2000);
            }
        }
        private void UpdateBooking()
        {
            
            int bookingId;
            while (true)
            {
                bookingId = InputHelper.GetInputWithValidation(
                "Enter ID of booking to update: ",
                "Invalid ID, select correct ID",
                () => ViewBookings(),
                input => int.TryParse(input, out _),
                int.Parse
            );
                var bookingupdate = _bookingService.GetBookingById(bookingId);
                if (bookingupdate != null)
                {
                    break;
                }
                Console.WriteLine($"Booking with ID {bookingId} was not found.");
                Thread.Sleep(1500);
            }


            var booking = _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                Console.WriteLine($"No booking found with ID {bookingId}.");
                return;
            }

            Console.Clear();
            DisplayBookingDetail(bookingId);

            var checkIn = CheckDate("Enter Check-In date (yyyy-MM-dd): ", booking.CheckInDate);
            Console.Clear();
            DisplayBookingDetail(bookingId);

            var checkOut = CheckDate("Enter Check-Out date (yyyy-MM-dd): ", booking.CheckOutDate);
            Console.Clear();
            DisplayBookingDetail(bookingId);

            var roomId = SelectRoom(checkIn, checkOut);
            if (roomId == null)
            {
                Console.WriteLine("No room selected. Booking canceled.");
                return;
            }
            int extraBeds = 0;
            if (roomId.Type.Equals("Double", StringComparison.OrdinalIgnoreCase) && roomId.SquareMeter > 15)
            {
                
                extraBeds = InputHelper.GetInputWithValidation(
                    "Room allows extra beds, up to 2 extra beds: ",
                    "Invalid input. Please enter a number between 0 and 2.",
                    () => Console.WriteLine($"Room {roomId.RoomNumber} selected."),
                    input => int.TryParse(input, out int num) && num >= 0 && num <= 2,
                    int.Parse
                );
            }

            try
            {
                _bookingService.UpdateBooking(bookingId, roomId.RoomId, checkIn, checkOut);
                Console.WriteLine("Booking updated successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Thread.Sleep(2000);
        }
        private Customer SelectCustomer()
        {
            var customerId = InputHelper.GetInputWithValidation(
                "Enter Customer ID: ",
                "Invalid ID",
                () => _customerController.ViewAllCustomers(),
                input => int.TryParse(input, out _),
                int.Parse
            );

            var selectedCustomer = _customerService.GetCustomerById(customerId);

            if (selectedCustomer == null)
            {
                Console.WriteLine($"No customer found with ID {customerId}. Please try again.");
                Thread.Sleep(2000);
                return SelectCustomer(); // Försök igen
            }

            return selectedCustomer;
        }
        private Room SelectRoom(DateTime checkIn, DateTime checkOut)
        {
            var roomId = InputHelper.GetInputWithValidation(
                "Enter Room ID: ",
                "Choose correct ID.",
                () => _roomController.ViewAvailableRooms(checkIn, checkOut),
                input => int.TryParse(input, out _),
                int.Parse
                );
            var selectedRoom = _roomService.GetRoomById(roomId);
            if (selectedRoom == null)
            {
                Console.WriteLine($"No room ID {roomId} was found, try again");
                Thread.Sleep(2000);
                return SelectRoom(checkIn, checkOut);
            }
            return selectedRoom;
        }
        private bool ValidateFutureDate(DateTime date, DateTime? previousDate = null)
        {
            // Datumet måste vara i framtiden och efter ett tidigare datum (om tillämpligt)
            return date >= DateTime.Today && (!previousDate.HasValue || date > previousDate.Value);
        }
        private DateTime CheckDate(string prompt, DateTime? existingDate = null, DateTime? previousDate = null)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(prompt);

                if (existingDate.HasValue)
                {
                    Console.WriteLine($"Current date: {existingDate.Value:yyyy-MM-dd} (leave blank to keep it)");
                }
                try
                {
                    // Provide the prompt as an argument to the calendar method
                    DateTime selectedDate = InputCalendar.SelectDate(existingDate ?? DateTime.Now, prompt, allowPastDates: false);

                    if (ValidateFutureDate(selectedDate, previousDate))
                    {
                        return selectedDate.Date;
                    }
                    else
                    {
                        Console.WriteLine($"The date {selectedDate:yyyy-MM-dd} is invalid. It must be a future date{(previousDate.HasValue ? $" and after {previousDate:yyyy-MM-dd}" : "")}.");
                        Thread.Sleep(2000);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Date selection canceled. Please try again.");
                    Thread.Sleep(2000);
                    
                }
            }
        }
        private void DeleteBooking()
        {
            Console.Clear();
            int bookingId;
            while (true)
            {
                bookingId = InputHelper.GetInputWithValidation(
                "Enter ID of which booking to delete: ",
                "Wrong input, choose correct ID.",
                () => ViewBookings(),
                input => int.TryParse(input, out _),
                int.Parse
                );
                var bookingDelete = _bookingService.GetBookingById(bookingId);
                if (bookingDelete != null)
                {
                    break;
                }
                Console.WriteLine($"Booking ID {bookingId} was not found.");
                Thread.Sleep(1500);
            }
            if (!InputHelper.ConfirmAction("Are you sure you want to delete this Booking?"))
            {
                Console.WriteLine("Deletion canceled.");
                return;
            }
            try
            {
                _bookingService.DeleteBooking(bookingId);
                Console.WriteLine("Booking deleted successfully.");

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Thread.Sleep(2000);
        }
    }
}
