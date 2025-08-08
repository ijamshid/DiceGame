using DiceGameApplication.Classes;

namespace DiceGameApplication.User;

public class ValidationError
{
    private string Message { get; }

    protected ValidationError(string message)
    {
        Message = message;
    }

    public static readonly ValidationError InvalidDiceCountLength =
        new("Please specify at least three dice.");

    public static readonly ValidationError InvalidFaceCount =
        new("Each die must have exactly the same number of faces.");

    public static readonly ValidationError InvalidSelection =
        new("Invalid selection. Please choose a valid option.");

    public static readonly ValidationError InvalidFaceFormat =
        new("All die faces must be numbers.");

    public override string ToString()
    {
        return string.Join("\n", "Argument error.", Message);
    }

    public static bool ValidateDiceInput(string[] args, out List<Dice> dice)
    {
        dice = [];

        if (!ValidateDiceCount(args)) return false;

        List<int[]> parsedDice = [];
        if (!ParseDiceFaces(args, parsedDice)) return false;

        if (!ValidateFaceCount(parsedDice)) return false;

        dice = parsedDice.Select(faces => new Dice(faces)).ToList();
        return true;
    }

    private static bool ValidateDiceCount(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine(InvalidDiceCountLength);
            return false;
        }
        return true;
    }

    private static bool ParseDiceFaces(string[] args, List<int[]> parsedDice)
    {
        foreach (var arg in args)
        {
            var faces = arg.Split(',');
            if (!faces.All(f => int.TryParse(f, out _)))
            {
                Console.WriteLine(InvalidFaceFormat);
                return false;
            }
            parsedDice.Add(faces.Select(int.Parse).ToArray());
        }
        return true;
    }

    private static bool ValidateFaceCount(List<int[]> parsedDice)
    {
        int faceCount = parsedDice[0].Length;
        if (parsedDice.Any(d => d.Length != faceCount))
        {
            Console.WriteLine(InvalidFaceCount);
            return false;
        }
        return true;
    }
}
