

using Spectre.Console;

namespace HotelAppDb.Utilities
{
    public class InputHandler
    {
        public static class InputHelper
        {
            /// <summary>
            /// Hämta användarinmatning med validering och kontextvisning.
            /// </summary>
            /// <param name="prompt">Meddelande som visas för användaren.</param>
            /// <param name="errorMessage">Felmeddelande vid ogiltig inmatning.</param>
            /// <param name="displayContext">En action för att visa relevant kontext (t.ex. redan angivna värden).</param>
            /// <param name="validation">Valideringsfunktion för inmatningen (valfritt).</param>
            /// <returns>Validerad användarinmatning.</returns>
            public static T GetInputWithValidation<T>(
                string prompt,
                string errorMessage,
                Action displayContext,
                Func<string, bool>? validation = null,
                Func<string, T>? transform = null,
                bool allowEmpty = false // Ny parameter för att tillåta tomt värde
            )
            {
                while (true)
                {
                    Console.Clear();
                    displayContext();
                    AnsiConsole.MarkupLine("[Gray](Type 'Cancel' to abort)[/]");
                    Console.Write(prompt);
                    var input = Console.ReadLine();

                    // Hantera avbrytning
                    if (string.Equals(input, "Cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new OperationCanceledException("Action was canceled by the user.");
                    }

                    if (allowEmpty && string.IsNullOrWhiteSpace(input))
                    {
                        return default!; // Tillåt tomt värde
                    }

                    if (validation == null || validation(input!))
                    {
                        return transform != null ? transform(input!) : (T)Convert.ChangeType(input, typeof(T));
                    }

                    Console.WriteLine(errorMessage);
                    Thread.Sleep(2000);
                }
            }

            public static bool IsValidRoomType(string input) =>
                !string.IsNullOrWhiteSpace(input) &&
                (input.Equals("Single", StringComparison.OrdinalIgnoreCase) ||
                 input.Equals("Double", StringComparison.OrdinalIgnoreCase) ||
                 input.Equals("Suite", StringComparison.OrdinalIgnoreCase));
            public static bool IsPositiveDecimal(string input) =>
                decimal.TryParse(input, out decimal num) && num > 0;
            public static bool IsPositiveInt(string input) =>
                int.TryParse(input, out int num) && num > 0;
            public static bool ConfirmAction(string message)
            {
                Console.WriteLine(message + " (yes/no)");
                var confirmation = Console.ReadLine()?.Trim();

                // Validera input och returnera true endast om användaren skriver "yes"
                return confirmation != null && confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase);
            }

        }
    }
}
