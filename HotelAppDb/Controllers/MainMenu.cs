
using Spectre.Console;

namespace HotelAppDb.Controllers
{
    public class MainMenu
    {
        private readonly CustomerController _customerController;
        private readonly RoomController _roomController;
        private readonly BookingController _bookingController;

        public MainMenu(CustomerController customerController, RoomController roomController, BookingController bookingController)
        {
            _customerController = customerController;
            _roomController = roomController;
            _bookingController = bookingController;
        }

        public void Run()
        {
            // Skapa menyalternativen en gång
            var menuPrompt = new SelectionPrompt<string>()
                .Title("[yellow]Hotel Management System[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                .AddChoices(new[]
                {
                    "Customer Management",
                    "Room Management",
                    "Booking Management",
                    "Exit"
                });

            while (true)
            {
                Console.Clear();

                // Visa menyn och få användarens val
                var menuSelection = AnsiConsole.Prompt(menuPrompt);

                // Hantera användarens val
                switch (menuSelection)
                {
                    case "Customer Management":
                        _customerController.DisplayCustomerMenu();
                        break;
                    case "Room Management":
                        _roomController.DisplayRoomMenu();
                        break;
                    case "Booking Management":
                        _bookingController.DisplayBookingMenu();
                        break;
                    case "Exit":
                        Environment.Exit(0); // Avslutar programmet
                        break;
                }
            }
        }
    }
}
