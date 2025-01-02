using HotelAppDb.Interfaces;
using HotelBookingApp.Model;
using Spectre.Console;

using static HotelAppDb.Utilities.InputHandler;

namespace HotelAppDb.Controllers
{
    public class RoomController
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public void DisplayRoomMenu()
        {
            // Skapa menyalternativen en gång
            var menuPrompt = new SelectionPrompt<string>()
                .Title("[yellow]Room Management[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                .AddChoices(new[] 
                {
                    "Add Room",
                    "View Rooms",
                    "Update Room",
                    "Delete Room",
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
                    case "Add Room":
                        AddRoom();
                        break;
                    case "View Rooms":
                        ViewAllRooms();
                        AnsiConsole.MarkupLine("[green]Press any key to return to menu...[/]");
                        Console.ReadKey();
                        break;
                    case "Update Room":
                        UpdateRoom();
                        break;
                    case "Delete Room":
                        DeleteRoom();
                        break;
                    case "Back to Main Menu":
                        return; // Gå tillbaka till huvudmenyn
                }
            }
        }


        private void ViewAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            Console.Clear();

            if (rooms == null || !rooms.Any())
            {
                AnsiConsole.MarkupLine("[red]No rooms available to display.[/]");
                return;
            }

            // Skapa en Spectre.Console-tabell
            var table = new Table
            {
                Border = TableBorder.Rounded,
                Expand = true
            };

            // Lägg till kolumnrubriker
            table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Number[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Type[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Price[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Square Meter[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Available[/]").Centered());

            // Lägg till rader för varje rum
            foreach (var room in rooms)
            {
                table.AddRow(
                    room.RoomId.ToString(),
                    room.RoomNumber.ToString(),
                    room.Type,
                    room.Price.ToString("C"),
                    room.SquareMeter.ToString(),
                    room.IsAvailible ? "[green]Yes[/]" : "[red]No[/]"
                );
            }

            // Visa tabellen
            AnsiConsole.Write(table);
        }

        public void ViewAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            Console.Clear();

            var rooms = _roomService.GetAvailableRooms(checkIn, checkOut);

            if (rooms == null || !rooms.Any())
            {
                AnsiConsole.MarkupLine($"[red]No available rooms found between {checkIn:yyyy-MM-dd} and {checkOut:yyyy-MM-dd}.[/]");
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                Console.ReadKey();
                return;
            }

            // Skapa en Spectre.Console-tabell
            var table = new Table
            {
                Border = TableBorder.Rounded,
                Expand = true
            };

            // Lägg till kolumnrubriker
            table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Number[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Type[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Price[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Square Meter[/]").Centered());

            // Lägg till rubriker och underrubriker
            table.Title = new TableTitle($"[bold green]Available Rooms[/]\n[blue]{checkIn:yyyy-MM-dd} to {checkOut:yyyy-MM-dd}[/]");
            table.Caption = new TableTitle("[grey]Use the room ID to select a room.[/]");

            // Lägg till rader för varje tillgängligt rum
            foreach (var room in rooms)
            {
                table.AddRow(
                    room.RoomId.ToString(),
                    room.RoomNumber.ToString(),
                    room.Type,
                    room.Price.ToString("C"),
                    room.SquareMeter.ToString()
                );
            }

            // Visa tabellen
            AnsiConsole.Write(table);

            // Prompt användaren att fortsätta

        }


        private void AddRoom()
        {
            try
            {
                Console.Clear();
                AnsiConsole.Write(new Panel("[bold blue]Add New Room[/]").Border(BoxBorder.Double).Expand());

                var roomNumber = InputHelper.GetInputWithValidation(
                    "Enter Room Number (100-1000): ",
                    "Invalid Room Number. Must be between 100 and 1000.",
                    () => ViewAllRooms(),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Add Room process canceled.");
                        return int.TryParse(input, out int num) && num >= 100 && num <= 1000;
                    },
                    int.Parse
                );

                var type = InputHelper.GetInputWithValidation(
                    "Enter Room Type (Single/Double/Suite): ",
                    "Invalid room type. Please enter one of the following: Single, Double, or Suite.",
                    () => ViewAllRooms(),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Add Room process canceled.");
                        return !string.IsNullOrWhiteSpace(input) &&
                               (input.Equals("Single", StringComparison.OrdinalIgnoreCase) ||
                                input.Equals("Double", StringComparison.OrdinalIgnoreCase) ||
                                input.Equals("Suite", StringComparison.OrdinalIgnoreCase));
                    },
                    input => char.ToUpper(input[0]) + input.Substring(1).ToLower()
                );

                var price = InputHelper.GetInputWithValidation(
                    "Enter Room Price: ",
                    "Invalid price. Must be a positive number.",
                    () => ViewAllRooms(),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Add Room process canceled.");
                        return decimal.TryParse(input, out decimal num) && num > 0;
                    },
                    decimal.Parse
                );

                var squareMeter = InputHelper.GetInputWithValidation(
                    "Enter Room Square Meter: ",
                    "Invalid input. Must be a positive number.",
                    () => ViewAllRooms(),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Add Room process canceled.");
                        return int.TryParse(input, out int num) && num > 0;
                    },
                    int.Parse
                );

                _roomService.AddRoom(roomNumber, type, price, squareMeter);
                AnsiConsole.MarkupLine("[bold green]Room added successfully![/]");
            }
            catch (OperationCanceledException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
            }
            catch (ArgumentException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Error: {ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
            finally
            {
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }


        private void DisplayRoomDetails(int roomNumber, string? newType = null, decimal? newPrice = null, int? newSquareMeter = null)
        {
            var room = _roomService.GetRoomByNumber(roomNumber);
            if (room == null)
            {
                AnsiConsole.MarkupLine($"[red]Room with number {roomNumber} not found.[/]");
                return;
            }

            // Skapa textinnehållet för det befintliga rummet
            string roomDetails =
                $"[bold]Current Room Details[/]\n" +
                $"[blue]Type:[/] {room.Type}\n" +
                $"[blue]Price:[/] {room.Price:C}\n" +
                $"[blue]Square Meter:[/] {room.SquareMeter}\n";

            // Lägg till nya värden om de har angetts
            if (!string.IsNullOrWhiteSpace(newType))
                roomDetails += $"[green]New Type:[/] {newType}\n";
            if (newPrice.HasValue)
                roomDetails += $"[green]New Price:[/] {newPrice.Value:C}\n";
            if (newSquareMeter.HasValue)
                roomDetails += $"[green]New Square Meter:[/] {newSquareMeter.Value}\n";

            // Skapa en enkel panel för att visa detaljerna
            var panel = new Panel(roomDetails)
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader($"Room Number [green]{roomNumber}[/]"),
                Padding = new Padding(1, 1, 1, 1)
            };

            AnsiConsole.Write(panel);

            // Instruktion om att lämna fält tomma
            AnsiConsole.MarkupLine("[dim]Leave blank to keep current value.[/]");
        }

        private void UpdateRoom()
        {
            try
            {
                Console.Clear();
                int roomNumber;
                while (true)
                {
                    roomNumber = InputHelper.GetInputWithValidation(
                        "Enter number of which Room you want to update: ",
                        "Invalid number. Please enter a valid number.",
                        () => ViewAllRooms(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Update Room process canceled.");
                            return int.TryParse(input, out _);
                        },
                        int.Parse
                    );

                    var room = _roomService.GetRoomByNumber(roomNumber);
                    if (room != null)
                        break;

                    Console.WriteLine($"Room with number {roomNumber} not found. Please try again.");
                    Thread.Sleep(2000);
                }

                if (!InputHelper.ConfirmAction("Are you sure you want to update this Room?"))
                {
                    Console.WriteLine("Update canceled.");
                    return;
                }

                var newType = InputHelper.GetInputWithValidation<string>(
                    "(Single/Double/Suite)\nEnter new Type : ",
                    "Invalid type. Please choose Single, Double, or Suite.",
                    () => DisplayRoomDetails(roomNumber),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Update Room process canceled.");
                        return string.IsNullOrWhiteSpace(input) ||
                               input.Equals("Single", StringComparison.OrdinalIgnoreCase) ||
                               input.Equals("Double", StringComparison.OrdinalIgnoreCase) ||
                               input.Equals("Suite", StringComparison.OrdinalIgnoreCase);
                    },
                    input => char.ToUpper(input[0]) + input.Substring(1).ToLower(),
                    allowEmpty: true
                );

                var newPrice = InputHelper.GetInputWithValidation(
                    "Enter Price: ",
                    "Invalid price. Must be a positive value.",
                    () => DisplayRoomDetails(roomNumber),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Update Room process canceled.");
                        return string.IsNullOrWhiteSpace(input) || decimal.TryParse(input, out decimal num) && num > 0;
                    },
                    decimal.Parse,
                    allowEmpty: true
                );

                var newSquareMeter = InputHelper.GetInputWithValidation(
                    "Enter Square Meter: ",
                    "Invalid input. Must be a positive number.",
                    () => DisplayRoomDetails(roomNumber),
                    input =>
                    {
                        if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                            throw new OperationCanceledException("Update Room process canceled.");
                        return string.IsNullOrWhiteSpace(input) || int.TryParse(input, out int num) && num > 0;
                    },
                    int.Parse,
                    allowEmpty: true
                );

                _roomService.UpdateRoom(roomNumber, newType, newPrice, newSquareMeter);
                Console.WriteLine("Room updated successfully.");
                
            }
            catch (OperationCanceledException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
            finally
            {
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }

        private void DeleteRoom()
        {
            int roomId;

            try
            {
                while (true)
                {
                    roomId = InputHelper.GetInputWithValidation(
                        "Enter ID of which Room to delete (or type 'Cancel' to abort): ",
                        "Wrong input, choose a correct ID.",
                        () => ViewAllRooms(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Deletion process canceled by the user.");
                            return int.TryParse(input, out _);
                        },
                        int.Parse
                    );

                    var roomDelete = _roomService.GetRoomById(roomId);
                    if (roomDelete != null)
                    {
                        break;
                    }
                    Console.WriteLine($"Room ID {roomId} was not found.");
                    Thread.Sleep(1500);
                }

                if (!InputHelper.ConfirmAction("Are you sure you want to delete this Room?"))
                {
                    Console.WriteLine("Deletion canceled.");
                    return;
                }

                _roomService.DeleteRoom(roomId);
                Console.WriteLine("Room deleted successfully.");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Operation canceled: {ex.Message}");
                Thread.Sleep(2000);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }
    }
}
