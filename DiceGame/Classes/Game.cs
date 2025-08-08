using System.Numerics;
using DiceGameApplication.User;

namespace DiceGameApplication.Classes;

public class Game(List<Dice> dice)
{
    private readonly List<Dice> dice = dice;
    private readonly Player user = new("User", false);
    private readonly Player computer = new("Computer", true);
    private readonly List<int> roundResults = [];

    public void Start()
    {
        ProbabilityTable.CreateProbabilityTable(dice);

        bool userMovesFirst = DetermineFirstMove();
        ChooseDiceForPlayers(userMovesFirst);
        PlayRounds();
    }

    private static bool DetermineFirstMove()
    {
        Console.WriteLine("Let's determine who makes the first move.");
        var (hmac, key, computerChoice) = HmacGenerator.GenerateHmac(2);
        Console.WriteLine($"I selected a random value in the range 0..1 (HMAC={hmac}).");
        Console.WriteLine("Try to guess my selection.");
        Console.WriteLine("0 - 0\n1 - 1\nX - exit\n? - help");
        int range = 2;
        int userChoice = UserInput.GetUserChoice(range);
        Console.WriteLine($"My number was {computerChoice} (KEY={key}).");
        bool userMovesFirst = userChoice == computerChoice;
        Console.WriteLine(userMovesFirst ? "You make the first move!" : "I make the first move!");

        return userMovesFirst;
    }

    private void ChooseDiceForPlayers(bool userMovesFirst)
    {
        if (userMovesFirst)
        {
            if (user.ChooseDie(dice) == -1) Environment.Exit(0);
            int excludeIndex = user.ChosenDie != null ? dice.IndexOf(user.ChosenDie) : -1;
            computer.ChooseDie(dice, excludeIndex);
        }
        else
        {
            computer.ChooseDie(dice);
            int excludeIndex = computer.ChosenDie != null ? dice.IndexOf(computer.ChosenDie) : -1;
            if (user.ChooseDie(dice, excludeIndex) == -1) Environment.Exit(0);
        }
    }

    public bool GetDiceFace(Player player, out int result)
    {
        result = 0;
        int roundIndex = player.IsComputer ? 0 : 1;
        if (player.ChosenDie != null && roundIndex < roundResults.Count)
        {
            result = player.ChosenDie.Faces[roundResults[roundIndex]];
            return true;
        }
        return false;
    }

    private void GetRoundResult(Player first, Player second)
    {
        if (first.LastThrow.HasValue && second.LastThrow.HasValue)
        {
            int result = (first.LastThrow.Value + second.LastThrow.Value) % 6;
            Console.WriteLine($"The final dice face index is {first.LastThrow.Value} + {second.LastThrow.Value} = {result} (mod 6).");
            roundResults.Add(result);
        }
        else
        {
            Console.WriteLine("One of the players did not throw a valid number.");
        }
    }

    private void GetFinalResult()
    {
        GetDiceFace(user, out int userResult);
        GetDiceFace(computer, out int computerResult);
        Console.WriteLine($"Final results: User - {userResult}, Computer - {computerResult}");
        if (userResult > computerResult)
            Console.WriteLine("You win!");
        else if (userResult < computerResult)
            Console.WriteLine("Computer wins!");
        else
            Console.WriteLine("It's a tie!");
    }

    private void PlayRounds()
    {
        for (int round = 0; round < 2; round++)
        {
            computer.AnnounceTurn(round);
            user.MakeMove();
            computer.ShowKey();
            GetRoundResult(computer, user);
        }
        GetFinalResult();
    }
}
