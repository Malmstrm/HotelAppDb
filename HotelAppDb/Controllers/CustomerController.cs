using HotelAppDb.Interfaces;
using HotelAppDb.Service;
using HotelBookingApp.Model;
using Spectre.Console;
using System.Text.RegularExpressions;
using static HotelAppDb.Utilities.InputHandler;

namespace HotelAppDb.Controllers
{
    public class CustomerController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public void DisplayCustomerMenu()
        {
            var menuPrompt = new SelectionPrompt<string>()
                .Title("[yellow]Customer Management[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                .AddChoices(new[] 
                {
                    "Add Customer",
                    "View Customers",
                    "Update Customer",
                    "Delete Customer",
                    "Back to Main Menu"
                });

            while (true)
            {
                Console.Clear();


                var menuSelection = AnsiConsole.Prompt(menuPrompt);

                switch (menuSelection)
                {
                    case "Add Customer":
                        AddCustomer();
                        break;
                    case "View Customers":
                        ViewAllCustomers();
                        AnsiConsole.MarkupLine("[green]Press any key to return to menu...[/]");
                        Console.ReadKey();
                        break;
                    case "Update Customer":
                        UpdateCustomer();
                        break;
                    case "Delete Customer":
                        DeleteCustomer();
                        break;
                    case "Back to Main Menu":
                        return; 
                }
            }
        }
        public void ViewAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            Console.Clear();

            if (customers == null || !customers.Any())
            {
                AnsiConsole.MarkupLine("[red]No customers found.[/]");
                return;
            }

            // Skapa en tabell
            var table = new Table
            {
                Border = TableBorder.Rounded,
                Expand = true
            };

            // Lägg till kolumnrubriker
            table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]First Name[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Last Name[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Email[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Phone Number[/]").Centered());

            // Lägg till rader för varje kund
            foreach (var customer in customers)
            {
                table.AddRow(
                    customer.CustomerId.ToString(),
                    customer.FirstName ?? "[red]N/A[/]",
                    customer.LastName ?? "[red]N/A[/]",
                    customer.Email ?? "[red]N/A[/]",
                    customer.PhoneNumber ?? "[red]N/A[/]"
                );
            }

            // Visa tabellen
            AnsiConsole.Write(table);
        }
        private void AddCustomer()
        {

                try
                {
                    while (true)
                    {
                    AnsiConsole.Clear();

                    // First Name
                    var firstName = InputHelper.GetInputWithValidation<string>(
                        "Enter First Name: ",
                        "Invalid input, first names shall only contain max 25 letters. Please try again.",
                        () =>
                        {
                            AnsiConsole.MarkupLine("[gray]Provide the customer's first name.[/]");
                            AnsiConsole.WriteLine();
                        },
                        input => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ]+$") && input.Length <= 25
                    );

                    // Last Name
                    var lastName = InputHelper.GetInputWithValidation<string>(
                        "Enter Last Name: ",
                        "Invalid input, last names shall only contain max 25 letters. Please try again.",
                        () =>
                        {
                            AnsiConsole.MarkupLine($"[gray]First name:[/] [yellow]{firstName}[/]");
                            AnsiConsole.WriteLine();
                        },
                        input => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ]+$") && input.Length <= 25
                    );

                    // Email
                    var email = InputHelper.GetInputWithValidation<string>(
                        "Enter Email: ",
                        "Invalid email format or email already exists. Please try again.",
                        () =>
                        {
                            AnsiConsole.MarkupLine($"[gray]First name:[/] [yellow]{firstName}[/] [gray]Last name:[/] [yellow]{lastName}[/]");
                            AnsiConsole.WriteLine();
                        },
                        input => !string.IsNullOrWhiteSpace(input) &&
                                 _customerService.IsValidEmail(input) &&
                                 _customerService.IsEmailUnique(input) &&
                                 input.Length <= 128
                    );

                    // Phone Number
                    var phoneNumber = InputHelper.GetInputWithValidation<string>(
                        "Enter Phone Number: ",
                        "Invalid phone number format. Please try again.",
                        () =>
                        {
                            AnsiConsole.MarkupLine($"[gray]First name:[/] [yellow]{firstName}[/] [gray]Last name:[/] [yellow]{lastName}[/] [gray]Email:[/] [yellow]{email}[/]");
                            AnsiConsole.WriteLine(); // Lägg till en tom rad
                        },
                        input => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^\d+$") && input.Length <= 12 // Endast siffror
                    );

                    // Lägg till kunden
                    _customerService.AddCustomer(firstName, lastName, email, phoneNumber);
                    AnsiConsole.MarkupLine("[bold green]Customer added successfully![/]");
                    break;
                }
                    
                }
                catch (OperationCanceledException)
                {
                    AnsiConsole.MarkupLine("[yellow]Process canceled by the user. Returning to the menu...[/]");
                }
                catch (ArgumentException ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]An unexpected error occurred:[/] {ex.Message}");
                }
                finally
                {
                    AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                    Console.ReadKey();
                }

        }
        private void UpdateCustomer()
        {
            Console.Clear();

            int customerId;
            while (true)
            {
                try
                {
                    customerId = InputHelper.GetInputWithValidation(
                        "Enter ID of the customer to update: ",
                        "Invalid ID. Please enter a valid numeric ID.",
                        () => ViewAllCustomers(),
                        input => int.TryParse(input, out _), // Validerar att det är ett heltal
                        int.Parse          // Konverterar till heltal
                    );

                    var customerUpdate = _customerService.GetCustomerById(customerId);
                    if (customerUpdate != null)
                    {
                        break;
                    }
                    Console.WriteLine($"No customer with ID {customerId} was found.");
                    Thread.Sleep(1500);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation canceled.");
                    return; // Avsluta metoden
                }
            }

            Console.Clear();
            AnsiConsole.MarkupLine($"[Gray]Are you sure you want to update this ID {customerId}? (yes/no)[/]");

            var confirmation = Console.ReadLine();
            if (!confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Update canceled.");
                return;
            }

            try
            {
                while (true)
                {
                    var firstName = InputHelper.GetInputWithValidation<string>(
                        "Leave Blank to keep current value\nEnter First Name: ",
                        "Invalid input, first names shall only contain max 25 letters. Please try again.",
                        () => ViewAllCustomers(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Update canceled by user.");
                            return string.IsNullOrWhiteSpace(input) || (Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ]+$") && input.Length <= 25);
                        },
                        allowEmpty: true
                    );

                    var lastName = InputHelper.GetInputWithValidation<string>(
                        "Leave Blank to keep current value\nEnter Last Name: ",
                        "Invalid input, last names shall only contain max 25 letters. Please try again.",
                        () => ViewAllCustomers(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Update canceled by user.");
                            return string.IsNullOrWhiteSpace(input) || (Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ]+$") && input.Length <= 25);
                        },
                        allowEmpty: true
                    );

                    var email = InputHelper.GetInputWithValidation<string>(
                        "Leave Blank to keep current value\nEnter Email: ",
                        "Invalid email format or email already exists. Please try again.",
                        () => ViewAllCustomers(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Update canceled by user.");
                            return string.IsNullOrWhiteSpace(input) || (_customerService.IsValidEmail(input) &&
                                                                        _customerService.IsEmailUnique(input) &&
                                                                        input.Length <= 128);
                        },
                        allowEmpty: true
                    );

                    var phoneNumber = InputHelper.GetInputWithValidation<string>(
                        "Leave Blank to keep current value\nEnter Phone Number: ",
                        "Invalid phone number format. Please try again.",
                        () => ViewAllCustomers(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Update canceled by user.");
                            return string.IsNullOrWhiteSpace(input) || (Regex.IsMatch(input, @"^\d+$") && input.Length <= 12);
                        },
                        allowEmpty: true
                    );

                    _customerService.UpdateCustomer(customerId, firstName, lastName, email, phoneNumber);
                    Console.WriteLine("Customer updated successfully.");
                    return;
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Operation canceled: {ex.Message}");
                
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }


        private void DeleteCustomer()
        {
            int customerId;

            try
            {
                while (true)
                {
                    customerId = InputHelper.GetInputWithValidation(
                        "Enter ID of which Customer to delete (or type 'Cancel' to abort): ",
                        "Wrong input, choose a correct ID.",
                        () => ViewAllCustomers(),
                        input =>
                        {
                            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase))
                                throw new OperationCanceledException("Deletion process canceled by the user.");
                            return int.TryParse(input, out _);
                        },
                        int.Parse
                    );

                    var customerToDelete = _customerService.GetCustomerById(customerId);
                    if (customerToDelete != null)
                    {
                        break;
                    }

                    Console.WriteLine($"Customer ID {customerId} was not found.");
                    Thread.Sleep(1500);
                }

                if (!InputHelper.ConfirmAction("Are you sure you want to delete this Customer?"))
                {
                    Console.WriteLine("Deletion canceled.");
                    return;
                }

                _customerService.DeleteCustomer(customerId);
                Console.WriteLine("Customer deleted successfully.");
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
