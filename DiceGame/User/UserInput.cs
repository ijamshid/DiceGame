using DiceGameApplication.Classes;

namespace DiceGameApplication.User
{
    public static class UserInput
    {
        public static int GetUserChoice(int maxRange)
        {
            while (true)
            {
                string? input = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (input == "x") Environment.Exit(0);
                if (input == "?")
                {
                    ProbabilityTable.DisplayProbabilityTable();
                    continue;
                }
                if (int.TryParse(input, out int choice) && choice >= 0 && choice < maxRange)
                    return choice;
                Console.WriteLine(ValidationError.InvalidSelection);
            }
        }
    }
}
